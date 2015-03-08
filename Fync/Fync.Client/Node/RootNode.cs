using System.IO;
using Fync.Common.Models;

namespace Fync.Client.Node
{
    class RootNode : FolderNode
    {
        public RootNode(DirectoryInfo directoryInfo, FolderWithChildren folder)
            : base(null, directoryInfo, folder)
        {
        }
    }
}