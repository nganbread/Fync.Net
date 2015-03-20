using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Client.Node.Base;
using Fync.Client.Web;
using Fync.Common.Common;

namespace Fync.Client.Visitors
{
    internal class SyncExistingFolderStrategy : StrategyBase<IFolderNode>
    {
        private readonly IHttpClient _httpClient;

        public SyncExistingFolderStrategy(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            if (!node.DirectoryInfo.Exists && !node.Folder.Deleted)
            {
                Logger.Instance.Log("\tdoesnt exist and not deleted");
                node.DirectoryInfo.Create();
            }
            else if (node.DirectoryInfo.Exists && node.Folder.Deleted)
            {
                Logger.Instance.Log("\texists and deleted");
                if (node.DirectoryInfo.LastWriteTimeUtc < node.Folder.ModifiedDate)
                {
                    Logger.Instance.Log("\t\tdelete");
                    node.DirectoryInfo.Delete();
                    await _httpClient.DeleteAsync("Folder/{0}", node.Folder.Id);
                }
            }
        }
    }
}