using System;
using System.Collections.Generic;
using System.Linq;
using Fync.Common;
using Fync.Data.Entities.Table;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fync.Data
{
    internal class DeletedSymbolicFileTableAccess : IDeletedSymbolicFileTableAccess
    {
        private readonly CloudTable _cloudTable;

        public DeletedSymbolicFileTableAccess(Func<string, CloudTable> cloudTableFactory)
        {
            _cloudTable = cloudTableFactory(CloudTableName.DeletedSymbolicFile);
        }
        
        public void AddDeletedSymbolicFilesInFolderToFolder(IList<SymbolicFileTableEntity> symbolicFiles, DateTime deletedDate)
        {
            foreach (var batch in symbolicFiles.Split(100))
            {
                var batchOperation = new TableBatchOperation();
                batch
                    .Select(x => new DeletedSymbolicFileTableEntity(x.FolderId, x.FileName, x.Hash, x.DateCreated, deletedDate))
                    .ForEach(x => batchOperation.Insert(x));
                
                _cloudTable.ExecuteBatch(batchOperation);
            }
        }

        public void AddDeletedSymbolicFileToFolder(SymbolicFileTableEntity symbolicFile, DateTime dateDeleted)
        {
            var deletedSymbolicFile = new DeletedSymbolicFileTableEntity(symbolicFile.FolderId, symbolicFile.FileName, symbolicFile.Hash, symbolicFile.DateCreated, dateDeleted);
            _cloudTable.Execute(TableOperation.Insert(deletedSymbolicFile));
        }
    }
}