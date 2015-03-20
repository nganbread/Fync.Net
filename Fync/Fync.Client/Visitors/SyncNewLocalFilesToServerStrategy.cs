using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class SyncNewLocalFilesToServerStrategy : StrategyBase<IFolderNode>
    {
        private readonly IHasher _hasher;
        private readonly IHttpClient _httpClient;

        public SyncNewLocalFilesToServerStrategy(IHasher hasher, IHttpClient httpClient)
        {
            _hasher = hasher;
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            if (!node.DirectoryInfo.Exists) return;
            if (node.Folder.Deleted) return;

            var newFiles = node
                .DirectoryInfo
                .GetFiles()
                .Except(node.Files.Select(x => x.FileInfo), new FileInfoEqualityComparer());

            foreach (var newFile in newFiles)
            {
                var file = await CreateFileNode(node, newFile);
                node.Files.Add(file);
            }
        }

        private async Task<FileNode> CreateFileNode(IFolderNode node, FileInfo fileInfo)
        {
            Logger.Instance.Log("\t{0}", fileInfo.FullName);
            var hash = _hasher.Hash(fileInfo);
            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);

            SymbolicFile file;
            if (fileAlreadyExistsOnServer)
            {
                Logger.Instance.Log("\t\tFile exists on server - PUT");
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = fileInfo.Name,
                };
                file =
                    await _httpClient.PutAsync<SymbolicFile>("{0}/SymbolicFile".FormatWith(node.Folder.Id), newSymbolicFile);
            }
            else
            {
                Logger.Instance.Log("\t\tFile doesnt exists on server - POST");
                file =
                    await
                        _httpClient.PostStreamAsync<SymbolicFile>(fileInfo.OpenRead(),
                            new NewFile {Name = fileInfo.Name}, "{0}/SymbolicFile", node.Folder.Id);
            }

            return new FileNode(file, node, fileInfo);
        }
    }
}