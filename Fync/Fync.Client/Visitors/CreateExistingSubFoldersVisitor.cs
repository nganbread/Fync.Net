using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node;
using Fync.Client.Node.Contracts;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    class CreateExistingSubFoldersVisitor : TypeVisitorBase<FolderNode>
    {
        public override async Task Visit(FolderNode node)
        {
            await base.Visit(node);
            node.Contents.Clear();

            var subFolderNodes = node.Folder.SubFolders.Select(x => CreateFolderNode(node, x)).ToList();
            node.Contents.AddRange(subFolderNodes);
        }

        private INode CreateFolderNode(FolderNode parentNode, FolderWithChildren folder)
        {
            var subdirectoryInfo = parentNode.DirectoryInfo.CreateSubdirectoryInfo(folder.Name);
            return new FolderNode(parentNode, subdirectoryInfo, folder);
        }
    }
}