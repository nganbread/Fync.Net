using System.IO;
using Fync.Client.Node.Base;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Node
{
    class NewFolderNode : FolderNodeBase
    {
        private readonly IFolderNode _parent;
        private readonly DirectoryInfo _directoryInfo;

        public NewFolderNode(IFolderNode parent, DirectoryInfo directoryInfo)
            : base(parent, directoryInfo)
        {
            _parent = parent;
            _directoryInfo = directoryInfo;
        }
    }
}