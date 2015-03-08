using System.IO;
using Fync.Client.Node.Base;
using Fync.Client.Node.Contracts;
using Fync.Common.Models;

namespace Fync.Client.Node
{
    class FolderNode : FolderNodeBase
    {
        private readonly IFolderNode _parent;
        private readonly DirectoryInfo _directoryInfo;
        private readonly FolderWithChildren _folder;
 
        public FolderNode(IFolderNode parent, DirectoryInfo directoryInfo, FolderWithChildren folder)
            : base(parent, directoryInfo)
        {
            _parent = parent;
            _directoryInfo = directoryInfo;
            _folder = folder;
        }

        public FolderWithChildren Folder
        {
            get { return _folder; }
        }
    }
}