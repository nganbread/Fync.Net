using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fync.Client.Node.Contracts;
using Fync.Client.Visitors;

namespace Fync.Client.Traverser
{
    class FolderTraverser<TFolderNode, TVisitor1, TVisitor2, TVisitor3> : FolderTraverser<TFolderNode, TVisitor1, TVisitor2>
        where TFolderNode : class, IFolderNode
        where TVisitor1 : IVisitor<TFolderNode>
        where TVisitor2 : IVisitor<TFolderNode>
        where TVisitor3 : IVisitor<TFolderNode>
    {
        public FolderTraverser(TVisitor1 visitor, TVisitor2 visitor2, TVisitor3 visitor3)
            : base(visitor, visitor2)
        {
            _visitors.Add(visitor3);
        }    
    }

    class FolderTraverser<TFolderNode, TVisitor1, TVisitor2> : FolderTraverser<TFolderNode, TVisitor1>
        where TFolderNode : class, IFolderNode
        where TVisitor1 : IVisitor<TFolderNode>
        where TVisitor2 : IVisitor<TFolderNode>
    {
        public FolderTraverser(TVisitor1 visitor, TVisitor2 visitor2)
            :base(visitor)
        {
            _visitors.Add(visitor2);
        }    
    }

    class FolderTraverser<TFolderNode, TVisitor> : ITraverser
        where TFolderNode : class, IFolderNode
        where TVisitor : IVisitor<TFolderNode>
    {
        protected readonly IList<IVisitor<TFolderNode>> _visitors;

        public FolderTraverser(TVisitor visitor)
        {
            _visitors = new List<IVisitor<TFolderNode>>
            {
                visitor
            };
        }

        public async Task Traverse(IFolderNode node)
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