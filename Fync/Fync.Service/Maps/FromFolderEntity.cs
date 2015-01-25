using Fync.Common;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service.Maps
{
    internal static class FromFolderEntity
    {
        public static Folder ToFolder(FolderEntity folderEntity)
        {
            return new Folder
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = folderEntity.SubFolders.MapToList(ToFolder),
                DateCreated = folderEntity.DateCreated
            };
        }
    }
}
