using System;
using Fync.Common;
using Fync.Data;
using Fync.Service.Models.Data;

namespace Fync.Service
{
    internal class InitialisationService : IInitialisationService
    {
        private readonly IContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentUser;

        public InitialisationService(ICurrentUser currentUser, IContext context, IConfiguration configuration)
        {
            _currentUser = currentUser;
            _context = context;
            _configuration = configuration;
        }

        public void InitialiseUser()
        {
            _context.Folders.Add(new FolderEntity
            {
                Name = _configuration.RootFolderName,
                ModifiedDate = DateTime.UtcNow,
                Owner = _currentUser.User
            });

            _context.SaveChanges();
        }
    }
}
