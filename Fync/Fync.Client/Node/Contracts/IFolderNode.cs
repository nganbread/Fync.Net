using System.Collections.Generic;
using System.IO;

namespace Fync.Client.Node.Contracts
{
    interface IFolderNode : INode
    {
        ICollection<INode> Contents { get; }
        DirectoryInfo DirectoryInfo { get; }
    }
}