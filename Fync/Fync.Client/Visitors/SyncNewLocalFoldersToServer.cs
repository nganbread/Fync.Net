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
    internal class SyncNewLocalFoldersToServer : StrategyBase<IFolderNode>
    {
        private readonly IHttpClient _httpClient;

        public SyncNewLocalFoldersToServer(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            if (!node.DirectoryInfo.Exists) return;
            if (node.Folder.Deleted) return;

            var newDirectories = node
                .DirectoryInfo
                .GetDirectories()
                .Except(node.SubFolders.Select(x => x.DirectoryInfo), new DirectoryInfoEqualityComparer());

            foreach (var newDirectory in newDirectories)
            {
                var newFolderNode = await CreateFolderNode(newDirectory, node);
                node.SubFolders.Add(newFolderNode);
            }
        }

        private async Task<FolderNode> CreateFolderNode(DirectoryInfo directoryInfo, IFolderNode node)
        {
            var serverFolder = await _httpClient.PostAsync<FolderWithChildren>("{0}/Folder".FormatWith(node.Folder.Id, directoryInfo.Name), new { directoryInfo.Name });

            return new FolderNode(serverFolder, node, directoryInfo);
        }
    }
}