using System.Web;
using System.Web.Mvc;
using SQLSolutions.Infrastructure;

namespace SQLSolutions
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TransactionFilter());
        }
    }
}