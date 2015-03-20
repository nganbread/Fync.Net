using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Visitors;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client
{
    internal class DeleteExistingFolderStrategy : StrategyBase<IFolderNode>
    {
        private readonly IHttpClient _httpClient;
        private readonly CreateExistingSubFoldersStrategy _createExistingSubFoldersStrategy;

        public DeleteExistingFolderStrategy(IHttpClient httpClient, CreateExistingSubFoldersStrategy createExistingSubFoldersStrategy)
        {
            _httpClient = httpClient;
            _createExistingSubFoldersStrategy = createExistingSubFoldersStrategy;
        }

        public async override Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            var folder = await _httpClient.DeleteAsync<FolderWithChildren>("{0}/Folder", node.Folder.Id);
            node.Folder = folder;
            node.RecursivelyDo(x =>
            {
                x.SubFolders.Clear();
                x.Files.Clear();
            },
            x => x.SubFolders);

            await _createExistingSubFoldersStrategy.Perform(node);
        }
    }
}