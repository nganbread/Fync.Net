using System;
using System.Collections.Generic;
using Fync.Data.Entities.Table;

namespace Fync.Data
{
    public interface IDeletedSymbolicFileTableAccess
    {
        void AddDeletedSymbolicFilesInFolderToFolder(IList<SymbolicFileTableEntity> symbolicFiles, DateTime deletedDate);
        void AddDeletedSymbolicFileToFolder(SymbolicFileTableEntity symbolicFile, DateTime dateDeleted);
    }
}