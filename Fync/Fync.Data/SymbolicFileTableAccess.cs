using System;
using System.Collections.Generic;
using System.Linq;
using Fync.Common;
using Fync.Data.Entities.Table;
using Fync.Data.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fync.Data
{
    internal class SymbolicFileTableAccess : ISymbolicFileTableAccess
    {
        private readonly CloudTable _cloudTable;

        public SymbolicFileTableAccess(Func<string, CloudTable> cloudTableFactory)
        {
            _cloudTable = cloudTableFactory(CloudTableName.SymbolicFile);
        }

        public IList<SymbolicFileTableEntity> GetSymbolicFilesInFolder(Guid folderId)
        {
            var query = new TableQuery<SymbolicFileTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, folderId.ToString()));
            return _cloudTable.ExecuteQuery(query).ToList();
        }

        public SymbolicFileTableEntity GetSymbolicFileOrDefault(Guid folderId, string fileName)
        {
            var partition = folderId.ToString();
            var row = fileName;

            return _cloudTable.Execute(TableOperation.Retrieve<SymbolicFileTableEntity>(partition, row)).Result as SymbolicFileTableEntity;
        }

        public void DeleteSymbolicFileFromFolder(SymbolicFileTableEntity symbolicFile)
        {
            _cloudTable.Execute(TableOperation.Delete(symbolicFile));
        }

        public void DeleteSymbolicFilesFromFolder(IEnumerable<SymbolicFileTableEntity> symbolicFiles)
        {
            foreach (var symbolicFileBatch in symbolicFiles.Split(100))
            {
                var batchOperation = new TableBatchOperation();
                symbolicFileBatch.ForEach(x => batchOperation.Delete(x));
                _cloudTable.ExecuteBatch(batchOperation);
            }
        }

        public void AddSymbolicFileToFolder(Guid folderId, string hash, string fileName, DateTime createdDate)
        {
            var symbolicFile = new SymbolicFileTableEntity(folderId, fileName, hash, createdDate);
            _cloudTable.Execute(TableOperation.InsertOrReplace(symbolicFile));
        }
    }
}