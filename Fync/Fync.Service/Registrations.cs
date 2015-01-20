using System;
using Fync.Common.Libraries;
using Fync.Data.Identity;
using Fync.Service.Maps;
using Fync.Service.Models;
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
            container.Register<IFolderService, FolderService>();
            container.Register<IAuthenticationService, AuthenticationService>();

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
            container.Register<Func<FolderEntity, Folder>>(FromFolderEntity.ToFolder);
            container.Register<Func<Folder, FolderEntity>>(FromFolder.ToFolderEntity);
        }
    }
}
