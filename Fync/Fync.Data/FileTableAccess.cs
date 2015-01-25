using System;
using System.Collections.Generic;
using System.Linq;
using Fync.Data.Entities.Table;
using Fync.Data.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fync.Data
{
    internal class FileTableAccess : IFileTableAccess
    {
        private readonly CloudTable _cloudTable;

        public FileTableAccess(Func<string, CloudTable> cloudTableFactory)
        {
            _cloudTable = cloudTableFactory(CloudTableName.File);
        }

        public bool FileExists(string hash)
        {
            return GetFileOrDefault(hash) != null;
        }

        public FileTableEntity GetFileOrDefault(string hash)
        {
            var partition = hash.FileHashPartitionKey();
            var row = hash.FileHashRowKey();

            return _cloudTable.Execute(TableOperation.Retrieve<FileTableEntity>(partition, row)).Result as FileTableEntity;
        }

        public FileTableEntity CreateFile(string hash, string blobName)
        {
            var file = new FileTableEntity(hash, blobName);
            _cloudTable.Execute(TableOperation.Insert(file));
            return file; 
        }
    }
}