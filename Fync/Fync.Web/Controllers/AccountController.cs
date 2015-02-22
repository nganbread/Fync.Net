using System.Web.Mvc;
using Fync.Service;
using Fync.Web.Models.Account;

namespace Fync.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUser _currentUser;

        public AccountController(IAuthenticationService authenticationService, ICurrentUser currentUser)
        {
            _authenticationService = authenticationService;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (_currentUser.User != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginModel());
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationService.Login(model.EmailAddress, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Login", model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterPost(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationService.Register(model.EmailAddress, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Register", model);
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogoutPost()
        {
            _authenticationService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}