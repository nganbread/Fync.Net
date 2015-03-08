using Fync.Client.Node.Contracts;

namespace Fync.Client.Node
{
    static class NodeExtensions
    {
        public static void SwapChild(this IFolderNode parent, INode old, INode @new)
        {
            parent.Contents.Remove(old);
            parent.Contents.Add(@new);
        }
    }
}