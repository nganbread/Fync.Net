using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node.Contracts;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    class FolderTraverser<TFolderNode> : ITraverser
        where TFolderNode : class, IFolderNode
    {
        private readonly IFolderNode _root;
        private readonly IVisitor<TFolderNode>[] _visitors;

        public FolderTraverser(IFolderNode root, params IVisitor<TFolderNode>[] visitors)
        {
            _root = root;
            _visitors = visitors;
        }

        public async Task Traverse()
        {
            await Traverse(_root);
        }

        private async Task Traverse(IFolderNode node)
        {
            var folderNode = node as TFolderNode;
            if (folderNode != null)
            {
                foreach (var visitor in _visitors)
                {
                    if (visitor.WantsToVisit(folderNode))
                    {
                        await visitor.Visit(folderNode);
                    }
                }
            }

            foreach (var childNode in node.Contents.OfType<TFolderNode>())
            {
                await Traverse(childNode);
            }
        }
    }
}