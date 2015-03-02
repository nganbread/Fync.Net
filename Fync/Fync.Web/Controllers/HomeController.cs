using System.Web.Mvc;
using Fync.Service;

namespace Fync.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrentUser _currentUser;

        public HomeController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return _currentUser.User == null
                ? View() as ActionResult
                : RedirectToAction("Index", "Fync") as ActionResult;
        }
	}
}