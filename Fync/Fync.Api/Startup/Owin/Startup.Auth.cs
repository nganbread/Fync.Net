using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Fync.Api
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(30, 0, 0, 0, 0),
                Provider = new CookieAuthenticationProvider
                {
                    OnResponseSignIn = context =>
                    {
                        //Dont 'expire' the cookie
                        context.Properties.ExpiresUtc = DateTime.UtcNow.AddYears(10);
                    }
                }
            });
        }
    }
}
