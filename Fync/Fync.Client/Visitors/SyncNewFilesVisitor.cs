using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class SyncNewFilesVisitor : TypeVisitorBase<NewFileNode>
    {
        private readonly IHasher _hasher;
        private readonly IHttpClient _httpClient;

        public SyncNewFilesVisitor(IHasher hasher, IHttpClient httpClient)
        {
            _hasher = hasher;
            _httpClient = httpClient;
        }

        public override async Task Visit(NewFileNode node)
        {
            await base.Visit(node);
            var parent = (FolderNode)node.Parent;
            SymbolicFile file;

            var hash = _hasher.Hash(node.FileInfo);
            var fileAlreadyExistsOnServer = await _httpClient.GetAsync<bool>("File?hash={0}", hash);

            if (fileAlreadyExistsOnServer)
            {
                Logger.Instance.Log("\tFile exists on server");
                var newSymbolicFile = new NewSymbolicFile
                {
                    Hash = hash,
                    Name = node.FileInfo.Name,
                };
                file = await _httpClient.PutAsync<SymbolicFile>("{0}/SymbolicFile".FormatWith(parent.Folder.Id), newSymbolicFile);
            }
            else
            {
                Logger.Instance.Log("\tFile doesnt exists on server");
                file = await _httpClient.PostStreamAsync<SymbolicFile>(node.FileInfo.OpenRead(), new NewFile { Name = node.FileInfo.Name }, "{0}/SymbolicFile", parent.Folder.Id);
            }

            var fileNode = new FileNode(node.Parent, file, node.FileInfo);
            fileNode.Parent.SwapChild(node, fileNode);
        }
    }
}