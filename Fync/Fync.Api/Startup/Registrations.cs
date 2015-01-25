using System.Web;
using Fync.Service;
using Microsoft.Owin.Security;
using TinyIoC;

namespace Fync.Api.App_Start
{
    internal static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            container.Register<IAuthenticationManager>((cContainer, overloads) => HttpContext.Current.GetOwinContext().Authentication);
            container.Register<ICurrentUser, CurrentUser>();
        }
    }
}