using System;
using System.IO;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Node.Base
{
    abstract class FileNodeBase : NodeBase, IFileNode
    {
        private readonly FileInfo _fileInfo;

        protected FileNodeBase(IFolderNode parent, FileInfo fileInfo)
            :base(parent)
        {
            _fileInfo = fileInfo;
        }

        public FileInfo FileInfo
        {
            get { return _fileInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherFile = obj as FileNodeBase;
            return otherFile == null
                ? false
                : _fileInfo.FullName.Equals(otherFile._fileInfo.FullName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return _fileInfo.FullName.GetHashCode();
        }
    }
}