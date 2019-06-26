using System.Web.Mvc;
using System.Web.Routing;
using NGZB.Models.Class;

namespace NGZB.Filter
{
    public class FirstCheck : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RouteValueDictionary Install = new RouteValueDictionary(new
            {
                controller = "Install",
                action = "_Install"
            });
            XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
            if (xmlhelp.FileExists == true)
            {
                filterContext.Result = new RedirectToRouteResult(Install);
            }
        }
    }
}