using System.Web.Mvc;

namespace Fync.Web
{
    internal class FilterConfig : IFilterConfig
    {
        private readonly InjectCurrentUserIntoViewBagAttribute _injectCurrentUserIntoViewBagAttribute;

        public FilterConfig(InjectCurrentUserIntoViewBagAttribute injectCurrentUserIntoViewBagAttribute)
        {
            _injectCurrentUserIntoViewBagAttribute = injectCurrentUserIntoViewBagAttribute;
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(_injectCurrentUserIntoViewBagAttribute);
        }
    }
}
