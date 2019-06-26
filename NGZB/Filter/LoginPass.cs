using NGZB.Models.Class;
using NGZB.Models.Object;
using System.Web.Mvc;
using System.Web.Routing;

namespace NGZB.Filter
{
    public class LoginPass : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionHelp session = new SessionHelp();
            RouteValueDictionary dictionary = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "_LoginWindow"
            });
            if (session.GetSessionUser() == null)
            {
                filterContext.Result = new RedirectToRouteResult(dictionary);
            }
            else
            {
                LoginUser loginuser = new LoginUser();
                if (loginuser.LoginUserCode == null)
                {
                    filterContext.Result = new RedirectToRouteResult(dictionary);
                }
            }
        }
    }
}