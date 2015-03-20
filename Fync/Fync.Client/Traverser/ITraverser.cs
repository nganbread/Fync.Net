using System.Threading.Tasks;
using Fync.Client.Node.Base;

namespace Fync.Client.Traverser
{
    internal interface ITraverser
    {
        Task Traverse(IFolderNode root);
    }
}