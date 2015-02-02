using System;

namespace Fync.Common.Models
{
    public class SymbolicFile
    {
        public DateTime DateCreatedUtc { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public Guid FolderId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
    }
}