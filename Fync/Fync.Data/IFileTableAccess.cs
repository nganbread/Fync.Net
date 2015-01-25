using System;
using Fync.Data.Entities.Table;

namespace Fync.Data
{
    public interface IFileTableAccess
    {
        FileTableEntity GetFileOrDefault(string hash);
        FileTableEntity CreateFile(string hash, string blobName);
        bool FileExists(string hash);
    }
}