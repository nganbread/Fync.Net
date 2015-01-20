using System;
using Fync.Service.Models;

namespace Fync.Service
{
    public interface IFolderService
    {
        Folder GetFullTree(Guid root);
        void UpdateRootFolder(Folder updatedRootFolder);
        Folder GetFullTree();
    }
}