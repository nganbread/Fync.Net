using System;
using Fync.Service.Models;

namespace Fync.Service
{
    public interface IFolderService
    {
        Folder GetFolderTree(Guid root);
        Guid CreateTree(Folder root);
        void UpdateRootFolder(Folder updatedTree);
    }
}