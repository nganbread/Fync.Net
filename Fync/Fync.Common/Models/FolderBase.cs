using System;

namespace Fync.Common.Models
{
    public abstract class FolderBase
    {
        //[NotEqual(default(DateTime))]
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
    }
}