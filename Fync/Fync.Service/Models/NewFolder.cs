using System;
using System.Collections.Generic;

namespace Fync.Service.Models
{
    public class NewFolder
    {
        public NewFolder()
        {
            SubFolders = new List<NewFolder>();
        }

        public string Name { get; set; }
        public IList<NewFolder> SubFolders { get; set; }
    }
}