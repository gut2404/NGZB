using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------
        public string __GetMenuGroup()
        {
            return Menu.GetParentMenu(0);
        }

        public int __Del(FormCollection form)
        {
            if (form["menuid"] != null)
            {
                int menuid = 0;
                int.TryParse(form["menuid"], out menuid);
                return Menu.Del(menuid);
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __Add(FormCollection form)
        {
            if (form["menuparentID"] == null || form["menuImg"] == null || form["modeid"] == null || form["colercssclass"] == null || form["menutype"] == null)
            {
                return 0;
            }
            else
            {
                int menuparentid = 0;
                int modeid = 0;
                int menutype = 0;
                if (int.TryParse(form["menuparentID"], out menuparentid) == false || int.TryParse(form["modeid"], out modeid) == false)
                {
                    return 0;
                }
                else
                {
                    if (int.TryParse(form["menutype"], out menutype) == false)
                    {
                        menutype = 1;
                    }
                    string menuimg = form["menuImg"].Trim();
                    string colercssclass = form["colercssclass"];
                    return Menu.Add(menuparentid, modeid, menutype, menuimg, colercssclass);
                }
            }
        }

        public string __RoleMenu(FormCollection form)
        {
            if (Request.QueryString["roleid"] != null && Request.QueryString["pid"] != null)
            {
                return Menu.RoleMenu(Request.QueryString["roleid"].ToString(), Request.QueryString["pid"].ToString());
            }
            else
            {
                return "";
            }
        }

        public string __GetParentMenu()
        {
            return Menu.GetParentMenu(0);
        }

        [HttpPost]
        public string __GetMenu(FormCollection form)
        {
            int parentID = 0;
            if (form["parentID"] != null)
            {
                int.TryParse(form["parentID"], out parentID);
                return Menu.GetParentMenu(parentID);
            }
            else
            {
                return "[]";
            }
        }

        [HttpPost]
        public int __AddMenuGroup(FormCollection form)
        {
            if (form["menugroupname"] != null)
            {
                string groupName = form["menugroupname"].ToString().Trim();
                string icon = "my_any";
                if (form["menuImg"] != null)
                {
                    icon = form["menuImg"].ToString().Trim();
                }
                return Menu.AddMenuGroup(groupName, icon);
            }
            else
            {
                return -1;
            }
        }

        public ActionResult _Edit(int? menuid, int? menutype)
        {
            ViewBag.menuID = menuid;
            ViewBag.menuType = menutype;
            return View();
        }

        public int __Edit(int? menuid, int? menutype, int? modelid, string menuimg, int? menuorderby, string menucolor, int? parentid)
        {
            if (menuid == null)
            {
                return -1;
            }
            else
            {
                return Menu.Edit(menuid, menutype, modelid, menuimg, menuorderby, menucolor, parentid);
            }
        }
    }
}