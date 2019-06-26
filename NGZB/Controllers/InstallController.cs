using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [InstallCheck]
    public class InstallController : Controller
    {
        public ActionResult _Install()
        {
            return View();
        }

        public string __GetInstallModelList()
        {
            return Install.GetXmlModel();
        }

        public ActionResult _InstallHelp()
        {
            return View();
        }

        public ActionResult __ActiveShow(string active)
        {
            active = "_Init_" + active;
            return View(active);
        }

        public int __TestConn(string server, string uid, string pwd)
        {
            if (server != null && uid != null && pwd != null)
            {
                return Install.TestConn(server, uid, pwd);
            }
            else
            {
                return -1;
            }
        }

        public int __saveConn(string server, string uid, string pwd)
        {
            if (server != null && uid != null && pwd != null)
            {
                return Install.SaveConn(server, uid, pwd);
            }
            else
            {
                return -1;
            }
        }

        public int __DeleteInitFile()
        {
            return Install.DeleteInitFile();
        }

        public string __GetGroup()
        {
            return Group.Get();
        }

        public int __InitGroup(string groupname)
        {
            if (groupname == null)
            {
                return -1;
            }
            return Install.InitGroup(groupname);
        }

        public int __InitUser(string usercode, string username, string password, string groupid)
        {
            if (usercode == null || username == null || groupid == null || password == null)
            {
                return -1;
            }
            int _groupid = 0;
            int.TryParse(groupid, out _groupid);
            if (_groupid == 0)
            {
                return -1;
            }
            return Install.InitUser(usercode, username, StringHelp.getMd5(password), _groupid);
        }

        public int __InitModel()
        {
            return Install.InitModel();
        }

        public int __InitMenu()
        {
            return Install.InitMenu();
        }

        public int __InitRole()
        {
            return Install.InitRole();
        }

        public int __InitUserRole()
        {
            return Install.InitUserRole();
        }
    }
}