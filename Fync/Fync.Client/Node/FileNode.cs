using System.IO;
using Fync.Client.Node.Base;
using Fync.Client.Node.Contracts;
using Fync.Common.Models;

namespace Fync.Client.Node
{
    class FileNode :FileNodeBase
    {
        private readonly SymbolicFile _file;

        public FileNode(IFolderNode parent, SymbolicFile file, FileInfo fileInfo)
            :base(parent, fileInfo)
        {
            _file = file;
        }

        public SymbolicFile File
        {
            get { return _file;}
        }
    }
}