using Fync.Common.Models;
using Fync.Data.Entities.Table;

namespace Fync.Service.Maps
{
    internal static class FromSymbolicFileTableEntity
    {
        public static SymbolicFile ToSymbolicFile(SymbolicFileTableEntity file)
        {
            return new SymbolicFile
            {
                Hash = file.Hash,
                DateCreatedUtc = file.DateCreated,
                Name = file.FileName,
                FolderId = file.FolderId,
                Deleted = file.Deleted,
                DateDeletedUtc = file.DateDeleted
            };
        }
    }
}