using System.Collections.Generic;
using Fync.Common.Models;

namespace Fync.Web.Models
{
    public class FolderModel
    {
        public Folder Folder { get; set; }
        public IList<SymbolicFile> Files { get; set; }
    }
}