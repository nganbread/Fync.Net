using System.Web.Mvc;
using Fync.Service;
using Fync.Web.Models.Account;

namespace Fync.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginPost()
        {
            return RedirectToAction("Login");
        }
    }
}