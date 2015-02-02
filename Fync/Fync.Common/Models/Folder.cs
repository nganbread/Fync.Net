using System;
using System.Collections.Generic;

namespace Fync.Common.Models
{
    public class Folder
    {
        public Folder()
        {
            SubFolders = new List<Folder>();
        }

        //[NotEqual(default(DateTime))]
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public IList<Folder> SubFolders { get; set; }
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
    }
}
