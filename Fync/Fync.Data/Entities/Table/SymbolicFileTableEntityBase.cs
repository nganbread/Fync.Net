using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fync.Data.Entities.Table
{
    public abstract class SymbolicFileTableEntityBase : TableEntity
    {
        public Guid FolderId { get; set; }
        public string Hash { get; set; }
        public string FileName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}