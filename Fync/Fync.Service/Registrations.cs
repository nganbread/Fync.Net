using System;
using System.Data.Entity;
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

            var a = container.CanResolve<DbContext>();
            var b = container.CanResolve<IUserStore<User, int>>();
            var c = container.CanResolve<UserManager<User, int>>();
        }

        private static void RegisterIdentity(TinyIoCContainer container)
        {
            container.Register<IUserStore<User, int>, UserStore<User, Role, int, UserLogin, UserRole, UserClaim>>();
            container.Register<UserManager<User, int>>();
        }

        private static void RegisterMaps(TinyIoCContainer container)
        {
            container.Register<Func<FolderEntity, Folder>>(FromFolderEntity.ToFolder);
            container.Register<Func<Folder, FolderEntity>>(FromFolder.ToFolderEntity);
        }
    }
}
