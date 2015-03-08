using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node;
using Fync.Client.Web;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class SyncNewFolderVisitor : TypeVisitorBase<NewFolderNode>
    {
        private readonly IHttpClient _httpClient;
        private readonly IClientConfiguration _clientConfiguration;

        public SyncNewFolderVisitor(IHttpClient httpClient, IClientConfiguration clientConfiguration)
        {
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
        }

        public override async Task Visit(NewFolderNode node)
        {
            await base.Visit(node);

            var relativePath = node.DirectoryInfo.GetRelativePath(_clientConfiguration.BaseDirectory);
            var serverFolder = await _httpClient.PostAsync<FolderWithChildren>("Folder/{0}".FormatWith(relativePath), new { node.DirectoryInfo.Name });

            var folder = new FolderNode(node.Parent, node.DirectoryInfo, serverFolder);
            folder.Contents.AddRange(node.Contents);
            node.Parent.SwapChild(node, folder);
        }
    }
}