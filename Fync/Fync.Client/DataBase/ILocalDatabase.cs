using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fync.Client.DataBase
{
    internal interface ILocalDatabase : IDisposable
    {
        IList<FileInfo> FilePathsOfCachedHash(string hash);
        void RemoveHash(string hash, string filePath);
        void InsertHash(string hash, string filePath);

        Task<IList<FileInfo>> FilePathsOfCachedHashAsync(string hash);
        Task RemoveHashAsync(string hash, string filePath);
        Task InsertHashAsync(string hash, string filePath);
    }
}