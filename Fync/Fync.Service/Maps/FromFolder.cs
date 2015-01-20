using Fync.Common;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service.Maps
{
    internal static class FromFolder
    {
        public static FolderEntity ToFolderEntity(Folder folder)
        {
            return new FolderEntity
            {
                Id = folder.Id,
                Name = folder.Name,
                SubFolders = folder.SubFolders.MapToList(ToFolderEntity),
                LastModified = folder.LastModified
            };
        }
    }
}