using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Extensions;
using Fync.Client.Node.Base;
using Fync.Common;
using Fync.Common.Models;

namespace Fync.Client.Visitors
{
    internal class CreateExistingSubFoldersStrategy : StrategyBase<IFolderNode>
    {
        public override async Task Perform(IFolderNode node)
        {
            await base.Perform(node);

            var subFolderNodes = node.Folder.SubFolders.Select(x => CreateFolderNode(node, x)).ToList();
            node.SubFolders.AddRange(subFolderNodes);
        }

        private IFolderNode CreateFolderNode(IFolderNode parentNode, FolderWithChildren folder)
        {
            var subdirectoryInfo = parentNode.DirectoryInfo.CreateSubdirectoryInfo(folder.Name);
            return new FolderNode(folder, parentNode, subdirectoryInfo);
        }
    }
}