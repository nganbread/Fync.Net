using System;
using System.Collections.Generic;
using Fync.Data.Entities.Table;

namespace Fync.Data
{
    public interface ISymbolicFileTableAccess
    {
        IList<SymbolicFileTableEntity> GetSymbolicFilesInFolder(Guid folderId);
        SymbolicFileTableEntity InsertSymbolicFileToFolder(Guid folderId, string hash, string fileName, DateTime createdDate);
        SymbolicFileTableEntity GetSymbolicFileOrDefault(Guid folderId, string fileName);
        SymbolicFileTableEntity DeleteSymbolicFileFromFolder(SymbolicFileTableEntity symbolicFile, DateTime dateDeleted);
        void DeleteSymbolicFilesFromFolder(IEnumerable<SymbolicFileTableEntity> symbolicFiles, DateTime dateDeleted);
    }
}