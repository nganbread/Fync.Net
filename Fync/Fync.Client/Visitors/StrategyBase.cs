using System.Threading.Tasks;
using Fync.Client.Node.Base;
using Fync.Common.Common;

namespace Fync.Client.Visitors
{
    internal abstract class StrategyBase<TNode> : IStrategy<TNode>
        where TNode: class, INode
    {
        public virtual Task Perform(TNode node)
        {
            Logger.Instance.Log("{0} {1}", GetType().Name, node.ToString());
            return Task.FromResult(true);
        }
    }
}