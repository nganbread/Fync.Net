using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Hash;
using Fync.Client.Node;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class SyncExistingFilesVisitor : TypeVisitorBase<FileNode>
    {
        private readonly IHasher _hasher;
        private readonly IHashCache _cachedHash;
        private readonly IHttpClient _httpClient;
        private readonly IClientConfiguration _clientConfiguration;

        public SyncExistingFilesVisitor(IHasher hasher, IHttpClient httpClient, IClientConfiguration clientConfiguration, IHashCache cachedHash)
        {
            _hasher = hasher;
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
            _cachedHash = cachedHash;
        }

        public override async Task Visit(FileNode node)
        {
            await base.Visit(node);
            if (!node.FileInfo.Exists)
            {
                Logger.Instance.Log("\tDoesnt exist");
                if (!node.File.DateDeletedUtc.HasValue)
                {
                    Logger.Instance.Log("\tDownload file");
                    await DownloadFile(node);
                }
            }
            else if (node.File.DateDeletedUtc > node.FileInfo.LastWriteTimeUtc)
            {
                Logger.Instance.Log("\tDelete");
                node.FileInfo.Delete();
            }
            else if (node.FileInfo.LastWriteTimeUtc > node.File.DateCreatedUtc)
            {
                Logger.Instance.Log("\tUploading existing file");
                await UploadExistingFile(node);
            }
            else if (node.File.Hash == _hasher.Hash(node.FileInfo))
            {
                //hashes are the same, file hasnt changed
                Logger.Instance.Log("\tNo change - Equal hashes");
            }
            else
            {
                Logger.Instance.Log("\tdefault: download");
                await DownloadFile(node);
            }
        }

        private async Task UploadExistingFile(FileNode node)
        {
            var parent = (FolderNode)node.Parent;

            var hash = _hasher.Hash(node.FileInfo);

            if (hash == node.File.Hash)
            {
                //file hasnt changed. no point updating it - should we update the last write time?
                Logger.Instance.Log("\t No change - Equal hashes");
                return;
            }

            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);

            if (fileAlreadyExistsOnServer)
            {
                Logger.Instance.Log("\tFile exists on server - PUT");
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = node.FileInfo.Name,
                };
                await _httpClient.PutAsync("{0}/SymbolicFile".FormatWith(parent.Folder.Id), newSymbolicFile);
            }
            else
            {
                Logger.Instance.Log("\t\tFile doesnt exists on server - POST");
                await _httpClient.PostStreamAsync(node.FileInfo.OpenRead(), new NewFile { Name = node.FileInfo.Name }, "{0}/SymbolicFile", parent.Folder.Id);
            }
        }

        private async Task DownloadFile(FileNode node)
        {
            Logger.Instance.Log("Download File");
            //check local hashes first
            var filePaths = _cachedHash.FilePathsOfCachedHash(node.File.Hash);
            var existingFile = filePaths.FirstOrDefault(x => x.Exists);
            if (existingFile != null)
            {
                //double check that the existing file is the same
                var freshHash = _hasher.Hash(existingFile.FullName);
                if (freshHash == node.File.Hash)
                {
                    //file exists on disk already
                    Logger.Instance.Log("\tFile exists, Copy it over");
                    existingFile.CopyTo(node.FileInfo.FullName);
                    return;
                }
            }

            //Download the file
            Logger.Instance.Log("\tDownload file...");
            var fileStream = await _httpClient.GetStreamAsync(node.FileInfo.GetRelativePath(_clientConfiguration.BaseDirectory));
            node.FileInfo.SaveToDisk(fileStream);
            _hasher.Hash(node.FileInfo); //TODO:hash it while its in memory instead of from disk
        }
    }
}