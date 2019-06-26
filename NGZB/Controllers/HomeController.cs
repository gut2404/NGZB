using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;
using NGZB.Models.Object;

namespace NGZB.Controllers
{
    public class HomeController : Controller
    {
        [LoginPass]
        [CheckUrl]
        public ActionResult NewFile()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        [LoginPass]
        [CheckUrl]
        public ActionResult PublishMessage()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public ActionResult _LoginWindow()
        {
            return View();
        }

        [LoginPass]
        public ActionResult _Index()
        {
            ViewBag.Tabs = Sys.DesktopTabs();
            return View();
        }

        [FirstCheck]
        public ActionResult _Login()
        {
            SessionHelp session = new SessionHelp();
            string userCode = session.GetSessionUser();
            LoginUser loginuser = new LoginUser();
            if (userCode != null && loginuser.LoginUserCode != null)
            {
                TokenDic.SetLoginUser(userCode, null, null);
                ViewBag.Tabs = Sys.DesktopTabs();
                return View("_Index");
            }
            return View();
        }

        [HttpPost]
        public int _Login(string usercode, string password, string remember)
        {
            if (usercode != null && password != null)
            {
                remember = remember.ToLower();
                return TokenDic.SetLoginUser(usercode.ToLower(), password, remember);
            }
            return -1;
        }

        public ActionResult __LoginOut()
        {
            TokenDic.ClearLoginUser(1);
            return RedirectToAction("_Login");
        }

        public ActionResult _ErrorUrl()
        {
            return View();
        }

        public ActionResult _ErrorUrlLogin()
        {
            return View();
        }

        public ActionResult _About()
        {
            return View("_About");
        }

        [ChildActionOnly]
        public string __GetMenu()
        {
            string tokenkey = "";
            if (RouteData.Values["tokenkey"] != null)
            {
                tokenkey = RouteData.Values["tokenkey"].ToString();
            }
            LoginUser loginuser = new LoginUser();
            if (string.Equals(tokenkey, loginuser.Tokenkey))
            {
                if (null != loginuser.LoginUserCode)
                {
                    return Menu.Get(loginuser.LoginUserCode);
                }
                else
                {
                    return "未加载菜单......";
                }
            }
            else
            {
                return "非法地址请求.....";
            }
        }

        [LoginPass]
        [CheckUrl]
        public void __CloserBrower()
        {
            TokenDic.ClearLoginUser(0);
        }

        [CheckUrl]
        public string __GetMsg()
        {
            LoginUser loginuser = new LoginUser();
            return loginuser.MsgCunt;
        }
    }
}