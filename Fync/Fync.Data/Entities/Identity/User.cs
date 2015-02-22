using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fync.Data.Identity
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public Guid? RootFolderId { get; set; }
    }
}
