using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Client.Visitors;
using Fync.Client.Web;
using Fync.Common.Models;

namespace Fync.Client
{
    internal class DeleteExistingFileStrategy : StrategyBase<IFileNode>
    {
        private readonly IHttpClient _httpClient;

        public DeleteExistingFileStrategy(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Perform(IFileNode node)
        {
            await base.Perform(node);

            var file = await _httpClient.DeleteAsync<SymbolicFile>("{0}/SymbolicFile?fileName={1}", node.File.FolderId, node.FileInfo.Name);
            node.File = file;
        }
    }
}