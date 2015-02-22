using System.Web.Mvc;

namespace Fync.Web
{
    internal interface IFilterConfig
    {
        void RegisterGlobalFilters(GlobalFilterCollection filters);
    }
}