using System.Web.Mvc;
using System.Web.Routing;
using NGZB.Models.Object;

namespace NGZB.Filter
{
    public class CheckUrl : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ///非法请求
            RouteValueDictionary errorUlr = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "_ErrorUrl"
            });
            ///多次登录
            RouteValueDictionary errorUrlLogin = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "_ErrorUrlLogin"
            });
            ///未登录
            RouteValueDictionary loginPage = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "_Login"
            });
            LoginUser loginuser = new LoginUser();
            if (loginuser.Tokenkey == null)
            {
                filterContext.Result = new RedirectToRouteResult(loginPage);
                return;
            }
            if (filterContext.HttpContext.Request.QueryString["tokenkey"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(errorUlr);
            }
            else
            {
                if (filterContext.HttpContext.Request.QueryString["tokenkey"] == "" || filterContext.HttpContext.Request.QueryString["tokenkey"] != loginuser.Tokenkey)
                {
                    filterContext.Result = new RedirectToRouteResult(errorUrlLogin);
                }
            }
        }
    }
}