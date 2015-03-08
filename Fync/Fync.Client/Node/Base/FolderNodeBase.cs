using System;
using System.Collections.Generic;
using System.IO;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Node.Base
{
    abstract class FolderNodeBase : NodeBase, IFolderNode
    {
        private readonly DirectoryInfo _directoryInfo;
        private readonly ISet<INode> _contents;

        protected FolderNodeBase(IFolderNode parent, DirectoryInfo directoryInfo)
            : base(parent)
        {
            _directoryInfo = directoryInfo;
            _contents = new HashSet<INode>();
        }

        public ICollection<INode> Contents
        {
            get { return _contents; }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return _directoryInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherFolder = obj as FolderNodeBase;
            return otherFolder == null 
                ? false
                : _directoryInfo.FullName.Equals(otherFolder._directoryInfo.FullName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return _directoryInfo.FullName.GetHashCode();
        }
    }
}