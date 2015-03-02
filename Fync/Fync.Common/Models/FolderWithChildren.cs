using System.Collections.Generic;

namespace Fync.Common.Models
{
    public class FolderWithChildren : FolderBase
    {
        public FolderWithChildren()
        {
            SubFolders = new List<FolderWithChildren>();
        }

        public IList<FolderWithChildren> SubFolders { get; set; }
    }
}
