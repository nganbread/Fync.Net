using System.Web.Mvc;
using Fync.Service;

namespace Fync.Web
{
    public class InjectCurrentUserIntoViewBagAttribute : ActionFilterAttribute
    {
        private readonly ICurrentUser _currentUser;

        public InjectCurrentUserIntoViewBagAttribute(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            filterContext.Controller.ViewBag.CurrentUser = _currentUser;
        }
    }
}