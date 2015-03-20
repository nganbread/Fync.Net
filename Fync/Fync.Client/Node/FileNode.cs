using System;
using System.Diagnostics;
using System.IO;
using Fync.Common.Models;

namespace Fync.Client.Node.Base
{
    [DebuggerDisplay("{FileInfo.FullName}")]
    internal class FileNode : IFileNode
    {
        private readonly IFolderNode _parent;
        private readonly FileInfo _fileInfo;
        private SymbolicFile _file;

        public FileNode(SymbolicFile file, IFolderNode parent, FileInfo fileInfo)
        {
            _file = file;
            _parent = parent;
            _fileInfo = fileInfo;
        }
        public SymbolicFile File
        {
            get { return _file;}
            set
            {
                //not null;
                _file = value;
            }
        }

        public IFolderNode Parent
        {
            get { return _parent;}
        }

        public FileInfo FileInfo
        {
            get { return _fileInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherFile = obj as FileNode;
            return otherFile == null
                ? false
                : _fileInfo.FullName.Equals(otherFile._fileInfo.FullName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return _fileInfo.FullName.GetHashCode();
        }

        public override string ToString()
        {
            return _fileInfo.FullName;
        }
    }
}