﻿using System.Web;
using System.Web.Mvc;
using Fync.Common.Libraries;
using Fync.Service;
using Microsoft.Owin.Security;
using TinyIoC;

namespace Fync.Web
{
    internal static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IAuthenticationManager>((cContainer, overloads) => HttpContext.Current.GetOwinContext().Authentication);
            container.Register<CurrentUser>().AsPerRequestSingleton();
            container.Register<ICurrentUser, CurrentUserWrapper>();

            container.Register<InjectCurrentUserIntoViewBagAttribute>();
            container.Register<IFilterConfig, FilterConfig>();

            container.Register<IDependencyResolver>((cContainer, overloads) => new TinyIocMvcResolver(container));
        }
    }
}
