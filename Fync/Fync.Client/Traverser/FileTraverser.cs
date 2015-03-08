using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node.Contracts;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    class FileTraverser<TFileNode, TVisitor1, TVisitor2> : FileTraverser<TFileNode, TVisitor1> 
        where TFileNode : class, IFileNode
        where TVisitor1 : IVisitor<TFileNode>
        where TVisitor2 : IVisitor<TFileNode>
    {
        public FileTraverser(TVisitor1 visitor1, TVisitor2 visitor2)
            :base(visitor1)
        {
            _visitors.Add(visitor2);
        }
    }
    class FileTraverser<TFileNode, TVisitor> : ITraverser
        where TFileNode : class, IFileNode
        where TVisitor : IVisitor<TFileNode>
    {
        protected readonly IList<IVisitor<TFileNode>> _visitors;

        public FileTraverser(TVisitor visitor)
        {
            _visitors = new List<IVisitor<TFileNode>>
            {
                visitor
            };
        }

        public  async Task Traverse(IFolderNode folderNode)
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