using System;
using Fync.Service.Maps;
using Fync.Service.Models;
using Fync.Service.Models.Data;
using TinyIoC;

namespace Fync.Service
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IFolderService, FolderService>();

            RegisterMaps(container);
        }

        private static void RegisterMaps(TinyIoCContainer container)
        {
            container.Register<Func<FolderEntity, Folder>>(FromFolderEntity.ToFolder);
            container.Register<Func<Folder, FolderEntity>>(FromFolder.ToFolderEntity);
        }
    }
}
