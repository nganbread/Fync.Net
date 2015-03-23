using System.Collections.Generic;
using Fync.Common.Models;

namespace Fync.Web.Models
{
    public class FyncModel
    {
        public FolderWithParentAndChildren Folder { get; set; }
        public IList<SymbolicFile> Files { get; set; }
    }
}