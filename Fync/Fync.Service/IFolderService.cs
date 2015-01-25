using System;
using Fync.Service.Models;

namespace Fync.Service
{
    public interface IFolderService
    {
        Folder GetFullTree(Guid root);
        void UpdateRootFolder(NewFolder updatedRootFolder);
        Folder GetFullTree();
    }
}