using System;
using Fync.Common;
using Fync.Data;
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
        private readonly IContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(Func<IAuthenticationManager> authenticationManagerFactory, Func<UserManager<User, int>> userManagerFactory, IContext context, IConfiguration configuration)
        {
            _authenticationManagerFactory = authenticationManagerFactory;
            _userManagerFactory = userManagerFactory;
            _context = context;
            _configuration = configuration;
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

            var rootFolder = new FolderEntity
            {
                Name = _configuration.RootFolderName,
                ModifiedDate = DateTime.UtcNow,
                Owner = user
            };
            _context.Folders.Add(rootFolder);
            var result = _userManagerFactory().Create(user, password);
            if (!result.Succeeded) return false;

            Login(user);

            user.RootFolderId = rootFolder.Id;
            _context.SaveChanges();

            return true;
        }

        public void Logout()
        {
            _authenticationManagerFactory().SignOut();
        }
    }
}