using System.Collections.Generic;
using System.IO;
using Fync.Client.Extensions;

namespace Fync.Client.Visitors
{
    internal class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.IsSameAs(y);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}