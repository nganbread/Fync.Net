using System;
using System.Data.Entity;
using Fync.Data.Identity;
using Fync.Service.Models.Data;

namespace Fync.Data
{
    public interface IContext : IDisposable
    {
        IDbSet<FolderEntity> Folders { get; }
        IDbSet<User> Users { get; }
        FolderEntity GetTree(Guid id);
        void SaveChanges();
    }
}