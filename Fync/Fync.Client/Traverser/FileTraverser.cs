using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node.Contracts;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    class FileTraverser<TFileNode> : ITraverser
        where TFileNode : class, IFileNode
    {
        private readonly IFolderNode _root;
        private readonly IVisitor<TFileNode>[] _visitors;

        public FileTraverser(IFolderNode root, params IVisitor<TFileNode>[] visitors)
        {
            _root = root;
            _visitors = visitors;
        }

        public async Task Traverse()
        {
            await Traverse(_root);
        }

        private async Task Traverse(IFolderNode folderNode)
        {
            foreach (var node in folderNode.Contents.OfType<TFileNode>().ToList())
            {
                var closuredNode = node;
                foreach (var visitor in _visitors.Where(x => x.WantsToVisit(closuredNode)).ToList())
                {
                    await visitor.Visit(closuredNode);
                }
            }

            foreach (var folder in folderNode.Contents.OfType<IFolderNode>().ToList())
            {
                await Traverse(folder);
            }
        }
    }
}