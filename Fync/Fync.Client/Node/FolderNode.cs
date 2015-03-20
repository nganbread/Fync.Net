using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Fync.Common.Models;

namespace Fync.Client.Node.Base
{
    [DebuggerDisplay("{DirectoryInfo.FullName}")]
    internal class FolderNode : IFolderNode
    {
        private FolderWithChildren _folder;
        private readonly IFolderNode _parent;
        private readonly DirectoryInfo _directoryInfo;
        private readonly ISet<IFolderNode> _subFolders;
        private readonly ISet<IFileNode> _files;

        public FolderNode(FolderWithChildren folder, IFolderNode parent, DirectoryInfo directoryInfo)
        {
            _folder = folder;
            _parent = parent;
            _directoryInfo = directoryInfo;
            _subFolders = new HashSet<IFolderNode>();
            _files = new HashSet<IFileNode>();
        }

        public IFolderNode Parent
        {
            get { return _parent; }
        }
        public FolderWithChildren Folder
        {
            get { return _folder; }
            set
            {
                //Not null
                _folder = value;
            }
        }

        public override string ToString()
        {
            return _directoryInfo.FullName;
        }

        public ICollection<IFolderNode> SubFolders
        {
            get { return _subFolders; }
        }
        public ICollection<IFileNode> Files
        {
            get { return _files; }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return _directoryInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherFolder = obj as FolderNode;
            return otherFolder == null 
                ? false
                : _directoryInfo.FullName.Equals(otherFolder._directoryInfo.FullName, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return _directoryInfo.FullName.GetHashCode();
        }
    }
}