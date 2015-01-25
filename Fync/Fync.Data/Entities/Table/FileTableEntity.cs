using System;
using Fync.Data.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fync.Data.Entities.Table
{
    public class FileTableEntity : TableEntity
    {
        public FileTableEntity()
        {
            
        }
        
        public FileTableEntity(string fileHash, string blobName)
        {
            PartitionKey = fileHash.FileHashPartitionKey();
            RowKey = fileHash.FileHashRowKey();
            Hash = fileHash;
            BlobName = blobName;
        }

        public string Hash { get; set; }
        public string BlobName { get; set; }
    }
}