using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class CreateExistingFilesVisitor : TypeVisitorBase<FolderNode>
    {
        private readonly IHttpClient _httpClient;

        public CreateExistingFilesVisitor(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Visit(FolderNode node)
        {
            await base.Visit(node);
            var serverFiles = await _httpClient.GetAsync<IList<SymbolicFile>>("{0}/SymbolicFile", node.Folder.Id);

            node.Contents.AddRange(serverFiles.Select(x => new FileNode(node, x, node.DirectoryInfo.CreateFileInfo(x.Name))));
        }
    }
}