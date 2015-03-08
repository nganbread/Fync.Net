using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Common;

namespace Fync.Client.Visitors
{
    internal class CreateNewFilesVisitor : TypeVisitorBase<FolderNode>
    {
        public override async Task Visit(FolderNode node)
        {
            await base.Visit(node);
            node.Contents.AddRange(node.DirectoryInfo.EnumerateFiles().Select(x => new NewFileNode(node, x)));
        }
    }
}