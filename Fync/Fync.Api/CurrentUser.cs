using System;
using System.Web;
using Fync.Data.Identity;
using Fync.Service;
using Microsoft.AspNet.Identity;

namespace Fync.Api
{
    internal class CurrentUser : ICurrentUser
    {
        private readonly Func<UserManager<User, int>> _userManager;

        public CurrentUser(Func<UserManager<User, int>> userManager)
        {
            _userManager = userManager;
            _lazyUser = new Lazy<User>(() =>
            {
                var id = HttpContext.Current.User.Identity.GetUserId<int>();
                return id == 0
                    ? null
                    : _userManager().FindById(id);
            });
        }

        private readonly Lazy<User> _lazyUser;

        public User User
        {
            get { return _lazyUser.Value; }
        }
    }
}
