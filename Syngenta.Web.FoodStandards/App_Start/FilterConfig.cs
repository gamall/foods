using System.Web;
using System.Web.Mvc;

namespace Syngenta.Web.FoodStandards
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}