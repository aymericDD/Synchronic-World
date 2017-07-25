using Synchronic_World.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Synchronic_World
{
    [IsLogginIn]
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
