using Fync.Client.Node.Contracts;

namespace Fync.Client.Node.Base
{
    abstract class NodeBase : INode
    {
        private readonly IFolderNode _parent;

        protected NodeBase(IFolderNode parent)
        {
            _parent = parent;
        }

        public IFolderNode Parent
        {
            get { return _parent; }
        }
    }
}