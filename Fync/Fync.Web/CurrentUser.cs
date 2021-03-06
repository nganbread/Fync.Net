﻿using System.Web;
using Fync.Data.Identity;
using Fync.Service;
using Microsoft.AspNet.Identity;

namespace Fync.Web
{
    internal class CurrentUser : ICurrentUser
    {
        private readonly UserManager<User, int> _userManager;

        public CurrentUser(UserManager<User, int> userManager)
        {
            _userManager = userManager;
        }

        public User User
        {
            get
            {
                var id = HttpContext.Current.User.Identity.GetUserId<int>();
                return id == 0
                    ? null
                    : _userManager.FindById(id);
            }
        }

        public int UserId
        {
            get { return HttpContext.Current.User.Identity.GetUserId<int>(); }
        }
    }
}
