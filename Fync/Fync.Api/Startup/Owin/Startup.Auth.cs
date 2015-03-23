using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Fync.Api
{
    internal partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(0, 0, 30, 0),
                Provider = new CookieAuthenticationProvider
                {
                    OnResponseSignIn = context =>
                    {
                        context.Properties.ExpiresUtc = DateTime.UtcNow.AddYears(10);
                    }
                },
            });
        }
    }
}
