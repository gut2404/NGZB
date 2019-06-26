using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class GroupController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __Get()
        {
            return Group.Get();
        }

        public string __GetAll()
        {
            return Group.GetAll();
        }

        [HttpPost]
        public int __SetRoot(FormCollection form)
        {
            if (form["groupid"] != null)
            {
                int groupId = 0;
                int.TryParse(form["groupid"], out groupId);
                return Group.SetRoot(groupId);
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __MoveGroup(FormCollection form)
        {
            if (form["id"] != null && form["targetId"] != null)
            {
                int groupID = 0;
                int parentID = 0;
                int.TryParse(form["id"], out groupID);
                int.TryParse(form["targetId"], out parentID);
                return Group.MoveGroup(groupID, parentID);
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __Del(FormCollection form)
        {
            if (form["groupid"] != null)
            {
                int groupID = 0;
                return int.TryParse(form["groupid"], out groupID) ? Group.Del(groupID) : 0;
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __Redo(FormCollection form)
        {
            if (form["groupid"] != null)
            {
                int groupID = 0;
                return int.TryParse(form["groupid"], out groupID) ? Group.Redo(groupID) : 0;
            }
            else
            {
                return 0;
            }
        }

        [HttpGet]
        public ActionResult _Edit()
        {
            if (Request.QueryString["groupid"] != null)
            {
                int groupID;
                if (int.TryParse(Request.QueryString["groupid"], out groupID))
                {
                    string[] rt = Group.GetGroupInfo(groupID);
                    if (rt != null)
                    {
                        ViewBag.groupid = rt[0];
                        ViewBag.groupname = rt[1];
                        ViewBag.icon = "iconCls:'icon-" + rt[2] + "'";
                        ViewBag.orderby = rt[3];
                    }
                    else
                    {
                        ViewBag.groupid = "";
                        ViewBag.groupname = "";
                        ViewBag.icon = "";
                        ViewBag.orderby = "";
                    }
                }
            }
            else
            {
                ViewBag.groupid = "";
                ViewBag.groupname = "";
                ViewBag.icon = "";
                ViewBag.orderby = "";
            }
            return View();
        }

        [HttpPost]
        public int __Edit(FormCollection form)
        {
            if (form["groupid"] == null)
            {
                return 0;
            }
            if (form["groupname"] == null || form["groupname"].Length < 2)
            {
                return 0;
            }
            string groupName = form["groupname"];
            int groupid = 0;
            if (int.TryParse(form["groupid"], out groupid) == false)
            {
                return 0;
            }
            int orderby = 0;
            if (int.TryParse(form["orderby"], out orderby) == false)
            {
                orderby = groupid;
            }
            string icon = "";
            if (form["icon"] == null || form["icon"] == "")
            {
                icon = "null";
            }
            else
            {
                icon = form["icon"].Trim();
            }
            return Group.Edit(groupid, groupName, icon, orderby);
        }

        [HttpGet]
        public ActionResult _Add()
        {
            if (Request.QueryString["groupid"] != null)
            {
                int groupID;
                if (int.TryParse(Request.QueryString["groupid"], out groupID))
                {
                    string[] rt = Group.GetGroupInfo(groupID);
                    if (rt != null)
                    {
                        ViewBag.groupid = rt[0];
                        ViewBag.groupname = rt[1];
                    }
                    else
                    {
                        ViewBag.groupid = "";
                        ViewBag.groupname = "";
                    }
                }
            }
            else
            {
                ViewBag.groupid = "";
                ViewBag.groupname = "";
            }
            return View();
        }

        [HttpPost]
        public int __Add(FormCollection form)
        {
            if (form["groupid"] == null)
            {
                return 0;
            }
            if (form["groupname"] == null || form["groupname"].Length < 2)
            {
                return 0;
            }
            string groupName = form["groupname"];
            int groupid = 0;
            if (int.TryParse(form["groupid"], out groupid) == false)
            {
                return 0;
            }
            int orderby = 0;
            if (int.TryParse(form["orderby"], out orderby) == false)
            {
                orderby = 1;
            }
            string icon = "";
            if (form["icon"] == null || form["icon"] == "")
            {
                icon = "null";
            }
            else
            {
                icon = form["icon"].Trim();
            }
            return Group.Add(groupid, groupName, icon, orderby);
        }
    }
}