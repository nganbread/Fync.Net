using System.Web;
using System.Web.Http;
using Fync.Api.App_Start;

namespace Fync.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            TinyIocConfig.Configure();
        }
    }
}
