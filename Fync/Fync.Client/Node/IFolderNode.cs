using System.Collections.Generic;
using System.IO;
using Fync.Common.Models;

namespace Fync.Client.Node.Base
{
    internal interface IFolderNode : INode
    {
        FolderWithChildren Folder { get; set; }
        ICollection<IFolderNode> SubFolders { get; }
        ICollection<IFileNode> Files { get; }
        DirectoryInfo DirectoryInfo { get; }
    }
}