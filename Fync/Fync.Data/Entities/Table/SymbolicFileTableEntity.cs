using System;

namespace Fync.Data.Entities.Table
{
    public class SymbolicFileTableEntity : SymbolicFileTableEntityBase
    {
        public SymbolicFileTableEntity()
        {
            
        }

        public SymbolicFileTableEntity(Guid folderId, string fileFileName, string hash, DateTime dateCreated)
        {
            PartitionKey = folderId.ToString();
            RowKey = fileFileName;

            FileName = fileFileName;
            FolderId = folderId;
            Hash = hash;
            DateCreated = dateCreated;
        }
    }
}
