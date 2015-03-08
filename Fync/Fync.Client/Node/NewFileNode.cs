using System.IO;
using Fync.Client.Node.Base;
using Fync.Client.Node.Contracts;

namespace Fync.Client.Node
{
    class NewFileNode :FileNodeBase
    {
        public NewFileNode(IFolderNode parent, FileInfo fileInfo)
            : base(parent, fileInfo)
        {
        }
    }
}