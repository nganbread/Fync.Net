using Fync.Service.Models.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fync.Data.Identity
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public virtual FolderEntity RootFolder { get; set; }
    }
}
