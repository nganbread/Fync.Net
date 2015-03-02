using System;
using Fync.Common.Libraries;
using Fync.Common.Models;
using Fync.Data.Entities.Table;
using Fync.Data.Identity;
using Fync.Service.Maps;
using Fync.Service.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TinyIoC;

namespace Fync.Service
{
    public static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IFolderService, FolderService>().AsSingleton();
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<ISymbolicFileService, SymbolicFileService>().AsSingleton();
            container.Register<IFileService, FileService>().AsSingleton();

            RegisterMaps(container);
            RegisterIdentity(container);
        }

        private static void RegisterIdentity(TinyIoCContainer container)
        {
            container.Register<IUserStore<User, int>, UserStore<User, Role, int, UserLogin, UserRole, UserClaim>>().AsPerRequestSingleton();
            container.Register<UserManager<User, int>>().AsPerRequestSingleton();
        }

        private static void RegisterMaps(TinyIoCContainer container)
        {
            container.Register<Func<FolderEntity, int, FolderWithParentAndChildren>>(FromFolderEntity.ToFolderWithParentAndChildren);
            container.Register<Func<FolderEntity, FolderWithChildren>>(FromFolderEntity.ToFolderWithChildren);
            container.Register<Func<FolderEntity, int, FolderWithChildren>>(FromFolderEntity.ToFolderWithChildren);
            container.Register<Func<NewFolder, FolderEntity>>(FromNewFolder.ToFolderEntity);
            container.Register<Func<SymbolicFileTableEntity, SymbolicFile>>(FromSymbolicFileTableEntity.ToSymbolicFile);
        }
    }
}
