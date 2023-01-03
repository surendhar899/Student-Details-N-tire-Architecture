using Student_Details_N_tire.Filter;
using System.Web.Mvc;

namespace Student_Details_N_tire.ExceptionFilter
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filter)
        {
         filter.Add(new MyexceptionFilter());
        }
    }
}
