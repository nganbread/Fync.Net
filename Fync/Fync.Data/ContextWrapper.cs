using System;
using System.Data.Entity;
using Fync.Data.Identity;
using Fync.Service.Models.Data;

namespace Fync.Data
{
    internal class ContextWrapper : IContext
    {
        private readonly Func<Context> _context;

        public ContextWrapper(Func<Context> context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context().Dispose();
        }

        public IDbSet<FolderEntity> Folders
        {
            get { return _context().Folders; }
        }

        public IDbSet<User> Users
        {
            get { return _context().Users; }
        }

        public FolderEntity GetTree(Guid folderId)
        {
            return _context().GetTree(folderId);
        }

        public void SaveChanges()
        {
            _context().SaveChanges();
        }

        public FolderEntity GetTree(int userId)
        {
            return _context().GetTree(userId);
        }
    }
}