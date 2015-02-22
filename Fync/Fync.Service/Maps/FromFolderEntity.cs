using Fync.Common;
using Fync.Common.Models;
using Fync.Service.Models.Data;

namespace Fync.Service.Maps
{
    internal static class FromFolderEntity
    {

        public static Folder ToFolder(FolderEntity folderEntity, int depth)
        {
            var subFolders = depth == 0
                ? null
                : folderEntity.SubFolders.MapToList(x => ToFolder(x, depth - 1));

            return new Folder
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = subFolders,
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted
            };
        }

        public static Folder ToFolder(FolderEntity folderEntity)
        {
            return new Folder
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = folderEntity.SubFolders.MapToList(ToFolder),
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted
            };
        }
    }
}
