using System.Collections.Generic;
using System.IO;
using Fync.Client.Extensions;

namespace Fync.Client.Visitors
{
    internal class DirectoryInfoEqualityComparer : IEqualityComparer<DirectoryInfo>
    {
        public bool Equals(DirectoryInfo x, DirectoryInfo y)
        {
            return x.IsSameAs(y);
        }

        public int GetHashCode(DirectoryInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}