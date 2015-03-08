using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node;
using Fync.Common;

namespace Fync.Client.Visitors
{
    class CreateNewSubFoldersVisitor : TypeVisitorBase<FolderNode>
    {
        public override async Task Visit(FolderNode node)
        {
            await base.Visit(node);
            if (node.DirectoryInfo.Exists)
            {
                var directories = node.DirectoryInfo.GetDirectories().Select(x => new NewFolderNode(node, x));
                node.Contents.AddRange(directories);
            }
        }
    }
}