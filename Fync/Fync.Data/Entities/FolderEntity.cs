using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fync.Data.Identity;
using Fync.Data.Models;

namespace Fync.Service.Models.Data
{
    public class FolderEntity : DatabaseEntity
    {
        public FolderEntity()
        {
            SubFolders = new List<FolderEntity>();    
        }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        public virtual FolderEntity Parent { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<FolderEntity> SubFolders { get; set; }
        public bool Deleted { get; set; }
    }
}
