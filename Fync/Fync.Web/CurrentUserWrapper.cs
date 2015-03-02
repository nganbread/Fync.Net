using System;
using Fync.Data.Identity;
using Fync.Service;

namespace Fync.Web
{
    internal class CurrentUserWrapper : ICurrentUser
    {
        private readonly Func<CurrentUser> _currentUser;

        public CurrentUserWrapper(Func<CurrentUser> currentUser)
        {
            _currentUser = currentUser;
        }

        public User User
        {
            get { return _currentUser().User; }
        }

        public int UserId
        {
            get { return _currentUser().UserId; }
        }
    }
}