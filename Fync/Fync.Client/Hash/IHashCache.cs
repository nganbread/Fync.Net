using System;
using System.Collections.Generic;
using System.IO;

namespace Fync.Client.Hash
{
    public interface IHashCache : IDisposable
    {
        IList<FileInfo> FilePathsOfCachedHash(string hash);
        void RemoveHash(string hash, string filePath);
        void InsertHash(string hash, string filePath);
    }
}