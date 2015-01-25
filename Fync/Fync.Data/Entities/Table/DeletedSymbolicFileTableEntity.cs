using System;

namespace Fync.Data.Entities.Table
{
    public class DeletedSymbolicFileTableEntity : SymbolicFileTableEntityBase
    {
        public DeletedSymbolicFileTableEntity()
        {
            
        }

        public DeletedSymbolicFileTableEntity(Guid folderId, string fileFileName, string hash, DateTime dateCreated, DateTime dateDeleted)
        {
            PartitionKey = folderId.ToString();
            RowKey = Guid.NewGuid().ToString();

            FileName = fileFileName;
            FolderId = folderId;
            Hash = hash;
            DateCreated = dateCreated;
            DateDeleted = dateDeleted;
        }

        public DateTime DateDeleted { get; set; }
    }
}