using Fync.Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Fync.Service
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User, int> _userManager;

        public AuthenticationService(IAuthenticationManager authenticationManager, UserManager<User, int> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public bool Login(string emailAddress, string password)
        {
            _authenticationManager.SignOut();
            var user = _userManager.Find(emailAddress, password);

            if (user == null) return false;

            Login(user);

            return true;
        }

        private void Login(User user)
        {
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            _authenticationManager.SignIn(new AuthenticationProperties
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

            var result = _userManager.Create(user, password);
            if (!result.Succeeded) return false;

            Login(user);
            return true;
        }
    }
}