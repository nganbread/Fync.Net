using System;
using System.Data.Entity;
using Fync.Data.Models;

namespace Fync.Data
{
    public interface IContext : IDisposable
    {
        IDbSet<Folder> Folders { get; }
        Folder GetTree(Folder root);
        void SaveChanges();
    }
}