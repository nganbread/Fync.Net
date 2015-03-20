using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node.Base;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class CreateExistingFileStrategy : StrategyBase<IFolderNode>
    {
        private readonly IHttpClient _httpClient;

        public CreateExistingFileStrategy(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);
            var serverFiles = await _httpClient.GetAsync<IList<SymbolicFile>>("{0}/SymbolicFile", node.Folder.Id);

            node.Files.AddRange(serverFiles.Select(x => new FileNode(x, node, node.DirectoryInfo.CreateFileInfo(x.Name))));
        }
    }
}