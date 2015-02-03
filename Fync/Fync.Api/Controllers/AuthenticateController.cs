using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fync.Common.Models;
using Fync.Service;

namespace Fync.Api.Controllers
{
    public class AuthenticateController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        public HttpResponseMessage Post(AuthenticationDetails model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationService.Login(model.EmailAddress, model.Password))
                {
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Authorize]
        public HttpResponseMessage Delete()
        {
            _authenticationService.Logout();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
