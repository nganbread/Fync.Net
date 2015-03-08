using System.Threading.Tasks;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Visitors
{
    internal interface IVisitor<TNode>
        where TNode: class, INode
    {
        bool WantsToVisit(TNode node);
        Task Visit(TNode node);
    }
}