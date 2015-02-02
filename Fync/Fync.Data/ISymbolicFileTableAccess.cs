using System;
using System.Collections.Generic;
using Fync.Data.Entities.Table;

namespace Fync.Data
{
    public interface ISymbolicFileTableAccess
    {
        IList<SymbolicFileTableEntity> GetSymbolicFilesInFolder(Guid folderId);
        void InsertSymbolicFileToFolder(Guid folderId, string hash, string fileName, DateTime createdDate);
        SymbolicFileTableEntity GetSymbolicFileOrDefault(Guid folderId, string fileName);
        void DeleteSymbolicFileFromFolder(SymbolicFileTableEntity symbolicFile);
        void DeleteSymbolicFilesFromFolder(IEnumerable<SymbolicFileTableEntity> symbolicFiles, DateTime dateDeleted);
    }
}