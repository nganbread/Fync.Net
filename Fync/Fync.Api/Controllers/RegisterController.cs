using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fync.Common.Models;
using Fync.Service;
using Fync.Service.Models;

namespace Fync.Api.Controllers
{
    [AllowAnonymous]
    public class RegisterController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IInitialisationService _initialisationService;

        public RegisterController(IAuthenticationService authenticationService, IInitialisationService initialisationService)
        {
            _authenticationService = authenticationService;
            _initialisationService = initialisationService;
        }

        public HttpResponseMessage Post(AuthenticationDetails model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationService.Register(model.EmailAddress, model.Password))
                {
                    _initialisationService.InitialiseUser();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }                
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}