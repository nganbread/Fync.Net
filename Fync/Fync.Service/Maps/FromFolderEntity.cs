using Fync.Common;
using Fync.Common.Models;
using Fync.Service.Models.Data;

namespace Fync.Service.Maps
{
    internal static class FromFolderEntity
    {
        public static FolderWithParent ToFolderWithParent(FolderEntity folderEntity)
        {
            return new FolderWithParent
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted,
                Parent = folderEntity.Parent.Map(ToFolderWithParent)
            };
        }

        public static FolderWithChildren ToFolderWithChildren(FolderEntity folderEntity, int depth)
        {
            var subFolders = depth == 0
                ? null
                : folderEntity.SubFolders.MapToList(x => ToFolderWithChildren(x, depth - 1));

            return new FolderWithChildren
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = subFolders,
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted
            };
        }

        public static FolderWithChildren ToFolderWithChildren(FolderEntity folderEntity)
        {
            return new FolderWithChildren
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = folderEntity.SubFolders.MapToList(ToFolderWithChildren),
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted
            };
        }

        public static FolderWithParentAndChildren ToFolderWithParentAndChildren(FolderEntity folderEntity, int depth)
        {
            var subFolders = depth == 0
                ? null
                : folderEntity.SubFolders.MapToList(x => ToFolderWithChildren(x, depth - 1));

            return new FolderWithParentAndChildren
            {
                Id = folderEntity.Id,
                Name = folderEntity.Name,
                SubFolders = subFolders,
                ModifiedDate = folderEntity.ModifiedDate,
                Deleted = folderEntity.Deleted,
                Parent = folderEntity.Parent.Map(ToFolderWithParent)
            };
        }
    }
}
