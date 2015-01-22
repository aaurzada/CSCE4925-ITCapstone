using System.Web;
using System.Web.Mvc;

namespace CSCE4925_ITCapstoneII
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}