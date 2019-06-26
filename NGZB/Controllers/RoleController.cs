using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class RoleController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult UserRole()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __GetComboBox()
        {
            return Role.GetComboBox();
        }

        public string __Get(FormCollection form)
        {
            string key = "";
            int selectFlag = 0;
            if (form["key"] != null)
            {
                key = form["key"];
            }
            if (form["f"] != null)
            {
                int.TryParse(form["f"], out selectFlag);
            }
            return Role.Get(selectFlag, key);
        }

        [HttpPost]
        public int __Del(FormCollection form)
        {
            if (form["roleID"] == null)
            {
                return 0;
            }
            else
            {
                string[] roleID = form["roleID"].Split('|');
                return Role.Del(roleID);
            }
        }

        public ActionResult _Add()
        {
            return View();
        }

        [HttpPost]
        public int __Add(FormCollection form)
        {
            string roleName = "";
            string roleInfo = "";
            if (form["roleName"] != null)
            {
                if (form["roleInfo"] != null)
                {
                    roleInfo = form["roleInfo"];
                }
                roleName = form["roleName"];
                return Role.Add(roleName, roleInfo);
            }
            else
            {
                return 0;
            }
        }

        public ActionResult _Edit()
        {
            return View();
        }

        [HttpPost]
        public int __Edit(FormCollection form)
        {
            return 0;
        }

        public int __Pass(FormCollection form)
        {
            if (form["roleName"] != null)
            {
                return Role.Pass(form["roleName"]);
            }
            else
            {
                return 1;
            }
        }

        public string __RoleItem(FormCollection form)
        {
            if (form["roleid"] != null)
            {
                return Role.GetRoleItem(form["roleid"]);
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        public int __AddRoleItem(FormCollection form)
        {
            if (form["roleid"] != null && form["menuid"] != null)
            {
                NGZB.Models.Object.LoginUser loginuser = new Models.Object.LoginUser();
                string appendUserCode = loginuser.LoginUserName + "(" + loginuser.LoginUserCode + ")";
                int roleid = 0;
                int menuid = 0;
                int suc = 0;
                string[] _menuid = form["menuid"].Split('|');
                if (_menuid.Length > 0 && true == int.TryParse(form["roleid"], out roleid))
                {
                    for (int i = 0; i < _menuid.Length; i++)
                    {
                        if (true == int.TryParse(_menuid[i], out menuid))
                        {
                            if (Role.AddRoleItem(roleid, menuid, appendUserCode) > 0)
                            {
                                suc = suc + 1;
                            }
                        }
                    }
                    return suc;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public int __DelRoleItem(FormCollection form)
        {
            if (form["roleItemID"] != null)
            {
                int suc = 0;
                string[] _roleitemid = form["roleItemID"].Split('|');
                if (_roleitemid.Length > 0)
                {
                    int roleitemid = 0;
                    for (int i = 0; i < _roleitemid.Length; i++)
                    {
                        int.TryParse(_roleitemid[i], out roleitemid);
                        if (Role.DelRoleItem(roleitemid) == 1)
                        {
                            suc = suc + 1;
                        }
                    }
                    return suc;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public string __GetUserRole(FormCollection form)
        {
            int roleid = 0;
            if (form["roleid"] != null)
            {
                int.TryParse(form["roleid"], out roleid);
            }
            return Role.GetUserRole(roleid);
        }

        public int __AddUserRole(FormCollection form)
        {
            if (form["usercode"] != null && form["roleid"] != null)
            {
                int roleid = 0;
                int.TryParse(form["roleid"], out roleid);
                string[] userCode = form["usercode"].Split('|');
                Models.Object.LoginUser loginuser = new Models.Object.LoginUser();
                string opUser = loginuser.LoginUserName + "(" + loginuser.LoginUserCode + ")";
                return Role.AddUserRole(roleid, userCode, opUser);
            }
            else
            {
                return -1;
            }
        }

        public int __DelUserRole(FormCollection form)
        {
            int suc = 0;
            if (form["roleUserID"] != null)
            {
                string[] _roleUserID = form["roleUserID"].Split('|');
                int roleUserID = 0;
                Models.Object.LoginUser loginuser = new Models.Object.LoginUser();
                string opUser = loginuser.LoginUserName + "(" + loginuser.LoginUserCode + ")";
                for (int i = 0; i < _roleUserID.Length; i++)
                {
                    int.TryParse(_roleUserID[i], out roleUserID);

                    if (Role.DelUserRole(roleUserID, opUser) == 1)
                    {
                        suc = suc + 1;
                    }
                }
            }
            else
            {
                suc = -1;
            }
            return suc;
        }

        public string __GetRoleInfo(FormCollection form)
        {
            if (Request.QueryString["roleID"] != null)
            {
                int roleid = 0;
                int.TryParse(Request.QueryString["roleID"], out roleid);
                return Role.GetRoleInfo(roleid);
            }
            else
            {
                return "";
            }
        }

        public string __GetRoleUser(FormCollection form)
        {
            if (Request.QueryString["roleID"] != null)
            {
                int roleid = 0;
                int.TryParse(Request.QueryString["roleID"], out roleid);
                return Role.GetRoleUser(roleid);
            }
            else
            {
                return "";
            }
        }
    }
}