using System;
using Fync.Data.Identity;
using Fync.Service.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Fync.Service
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<UserManager<User, int>> _userManagerFactory;

        public AuthenticationService(Func<IAuthenticationManager> authenticationManagerFactory, Func<UserManager<User, int>> userManagerFactory)
        {
            _authenticationManagerFactory = authenticationManagerFactory;
            _userManagerFactory = userManagerFactory;
        }

        public bool Login(string emailAddress, string password)
        {
            _authenticationManagerFactory().SignOut();
            var user = _userManagerFactory().Find(emailAddress, password);

            if (user == null) return false;

            Login(user);

            return true;
        }

        private void Login(User user)
        {
            var identity = _userManagerFactory().CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            _authenticationManagerFactory().SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, identity);
        }

        public bool Register(string email, string password)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
            };

            var result = _userManagerFactory().Create(user, password);
            if (!result.Succeeded) return false;

            Login(user);

            return true;
        }

        public void Logout()
        {
            _authenticationManagerFactory().SignOut();
        }
    }
}