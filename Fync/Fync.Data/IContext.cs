using System;
using System.Data.Entity;
using Fync.Service.Models.Data;

namespace Fync.Data
{
    public interface IContext : IDisposable
    {
        IDbSet<FolderEntity> Folders { get; }
        FolderEntity GetTree(FolderEntity root);
        void SaveChanges();
    }
}