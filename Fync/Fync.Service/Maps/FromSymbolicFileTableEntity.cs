using Fync.Data.Entities.Table;
using Fync.Service.Models;

namespace Fync.Service.Maps
{
    internal static class FromSymbolicFileTableEntity
    {
        public static SymbolicFile ToSymbolicFile(SymbolicFileTableEntity file)
        {
            return new SymbolicFile
            {
                Hash = file.Hash,
                DateCreated = file.DateCreated,
                Name = file.FileName,
            };
        }
    }
}