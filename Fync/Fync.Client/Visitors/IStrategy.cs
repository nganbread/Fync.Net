using System.Threading.Tasks;
using Fync.Client.Node.Base;

namespace Fync.Client.Visitors
{
    internal interface IStrategy<TNode>
        where TNode: class, INode
    {
        Task Perform(TNode node);
    }
}