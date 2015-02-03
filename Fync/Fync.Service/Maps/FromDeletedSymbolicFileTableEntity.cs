using Fync.Common.Models;
using Fync.Data.Entities.Table;

namespace Fync.Service.Maps
{
    internal static class FromDeletedSymbolicFileTableEntity
    {
        public static SymbolicFile ToSymbolicFile(DeletedSymbolicFileTableEntity file)
        {
            return new SymbolicFile
            {
                Hash = file.Hash,
                DateCreatedUtc = file.DateCreated,
                Name = file.FileName,
                FolderId = file.FolderId,
            };
        }
    }
}