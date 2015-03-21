using System;
using Fync.Common.Models;

namespace Fync.Service
{
    public interface IFolderService
    {
        FolderWithChildren GetFullTree(Guid root);
        void UpdateRootFolder(NewFolder updatedRootFolder, DateTime updateDate);
        FolderWithChildren CreateFolder(Guid folderId, string folderName, DateTime createDate);
        FolderWithChildren CreateFolder(string[] pathComponents, DateTime createDate);
        FolderWithChildren DeleteFolder(Guid folderId, DateTime deletedDate);
        FolderWithChildren GetFullTree();
        FolderWithChildren GetFolder(Guid id);
        FolderWithParentAndChildren GetFolderFromPath(string[] pathComponents);
    }
}