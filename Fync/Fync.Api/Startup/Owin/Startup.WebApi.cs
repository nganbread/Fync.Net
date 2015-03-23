using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Fync.Api.App_Start;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Fync.Api
{
    internal partial class Startup
    {
        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            TinyIocConfig.Configure(config);
            RouteConfig.Configure(config);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(config);
        }
    }
}