using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fync.Service;
using Fync.Service.Models;

namespace Fync.Api.Controllers
{
    [AllowAnonymous]
    public class RegisterController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public HttpResponseMessage Post(AuthenticationDetails model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationService.Register(model.EmailAddress, model.Password))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }                
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}