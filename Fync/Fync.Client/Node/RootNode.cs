using System.IO;
using Fync.Client.Node.Base;
using Fync.Common.Models;

namespace Fync.Client.Node
{
    internal class RootNode : FolderNode
    {
        public RootNode(DirectoryInfo directoryInfo, FolderWithChildren folder)
            : base(folder, null, directoryInfo)
        {
        }
    }
}