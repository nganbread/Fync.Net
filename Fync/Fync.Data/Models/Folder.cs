using System;
using System.Collections.Generic;

namespace Fync.Data.Models
{
    internal sealed class Folder : DatabaseEntity
    {
        public Folder()
        {
            SubFolders = new List<Folder>();    
        }

        public string Name { get; set; }
        public DateTime LastModified { get; set; }
        
        public ICollection<Folder> SubFolders { get; set; } 
    }
}
