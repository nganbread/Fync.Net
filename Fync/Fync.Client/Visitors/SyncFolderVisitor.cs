using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Client.Web;

namespace Fync.Client.Visitors
{
    internal class SyncFolderVisitor : TypeVisitorBase<FolderNode>
    {
        private readonly IHttpClient _httpClient;

        public SyncFolderVisitor(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Visit(FolderNode node)
        {
            await base.Visit(node);
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