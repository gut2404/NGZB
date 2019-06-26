using System.Web.Mvc;

namespace NGZB.Filter
{
    public class AllowCheck : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Response.Write(filterContext.RouteData.Values["controller"].ToString());
            //filterContext.HttpContext.Response.Write(filterContext.RouteData.Values["action"].ToString());
        }
    }
}