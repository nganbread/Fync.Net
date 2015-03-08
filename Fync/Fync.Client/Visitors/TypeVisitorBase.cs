using System.Threading.Tasks;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Visitors
{
    abstract class TypeVisitorBase<TNode> : IVisitor<TNode>
        where TNode: class, INode
    {
        public virtual bool WantsToVisit(TNode node)
        {
            return true;
        }

        public virtual Task Visit(TNode node)
        {
            if (node.Parent != null)
            {
                Logger.Instance.Log("{0} {1}", GetType().Name, node.Parent.DirectoryInfo.FullName);
            }
            return Task.FromResult(true);
        }
    }
}