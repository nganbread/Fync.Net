using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fync.Data.Models
{
    public class Folder : DatabaseEntity
    {
        public Folder()
        {
            SubFolders = new List<Folder>();    
        }

        public string Name { get; set; }
        [Required]
        public DateTime? LastModified { get; set; }

        [ForeignKey("Parent")]
        public Guid? Parent_Id { get; set; }   
        public virtual Folder Parent { get; set; }
        public virtual ICollection<Folder> SubFolders { get; set; } 
    }
}
