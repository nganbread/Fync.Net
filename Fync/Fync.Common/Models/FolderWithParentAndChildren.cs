using System.Collections.Generic;

namespace Fync.Common.Models
{
    public class FolderWithParentAndChildren : FolderBase
    {
        public FolderWithParentAndChildren()
        {
            SubFolders = new List<FolderWithChildren>();
        }

        public IList<FolderWithChildren> SubFolders { get; set; }
        public FolderWithParent Parent { get; set; }
    }
}