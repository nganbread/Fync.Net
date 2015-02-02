using Fync.Common;
using Fync.Common.Models;
using Fync.Service.Models;
using Fync.Service.Models.Data;

namespace Fync.Service.Maps
{
    internal static class FromNewFolder
    {
        public static FolderEntity ToFolderEntity(NewFolder folder)
        {
            return new FolderEntity
            {
                Name = folder.Name,
                SubFolders = folder.SubFolders.MapToList(ToFolderEntity),
            };
        }
    }
}