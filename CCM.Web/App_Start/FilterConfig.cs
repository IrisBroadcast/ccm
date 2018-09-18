using System.Web.Mvc;
using CCM.Web.Infrastructure.MvcFilters;

namespace CCM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorsAttribute());
        }
    }
}
