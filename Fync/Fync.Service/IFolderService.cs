using System;
using Fync.Common.Models;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service
{
    public interface IFolderService
    {
        Folder GetFullTree(Guid root);
        void UpdateRootFolder(NewFolder updatedRootFolder, DateTime updateDate);
        Folder CreateFolder(Guid folderId, string folderName, DateTime createDate);
        void DeleteFolder(Guid folderId, DateTime deletedDate);
        Folder GetFullTree();
    }
}