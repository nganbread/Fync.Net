using System.IO;
using Fync.Common.Models;

namespace Fync.Client.Node.Base
{
    internal interface IFileNode : INode
    {
        SymbolicFile File { get; set; }
        FileInfo FileInfo { get; }
    }
}