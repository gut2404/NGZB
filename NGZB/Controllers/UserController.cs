using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;
using NGZB.Filter;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        [HttpPost]
        public string __Get(FormCollection form)
        {
            string key = "";
            string where = "";
            if (form["key"] != null)
            {
                key = form["key"];
                where = string.Format("(userCode LIKE '%{0}%' OR userName LIKE '%{1}%' OR groupName LIKE '%{2}%')", key, key, key);
            }
            string flagWhere = "";
            if (form["f"] != null)
            {
                int userIsDelFlag;
                int.TryParse(form["f"], out userIsDelFlag);
                switch (userIsDelFlag)
                {
                    case 1:
                        flagWhere = "(userIsDelFlag=0) AND ";
                        break;

                    case 2:
                        flagWhere = "(userIsDelFlag=1) AND ";
                        break;
                }
            }
            where = flagWhere + where;
            DataTable dt = DbHelp.ExcuteTable("SELECT [userCode],[userName],[groupName],[loginNum],[userIsDelFlag] FROM [V_NGZB_User]", where, "userOrderBy ASC");
            return JsonConvert.SerializeObject(dt);
        }

        public ActionResult _Add()
        {
            return View();
        }

        [HttpPost]
        public int __Add(FormCollection form)
        {
            if (form["userCode"] == null)
            {
                return -1;
            }
            if (form["userName"] == null)
            {
                return -1;
            }
            if (form["userPassWord"] == null)
            {
                return -1;
            }
            if (form["groupId"] == null)
            {
                return -1;
            }
            int groupID;
            if (int.TryParse(form["groupId"], out groupID) == false)
            {
                return -1;
            }
            string userCode = form["userCode"];
            string userName = form["userName"];
            string passWord = StringHelp.getMd5(form["userPassWord"]);
            return Models.User.Add(userCode, userName, passWord, groupID);
        }

        [HttpPost]
        public int __Del(FormCollection form)
        {
            int rtResut = 0;
            if (form["userCode"] != null)
            {
                string[] userCode = form["userCode"].Split('|');
                rtResut = Models.User.Del(userCode);
            }
            return rtResut;
        }

        [HttpPost]
        public int __Redo(FormCollection form)
        {
            int rtResut = 0;
            if (form["userCode"] != null)
            {
                string[] userCode = form["userCode"].Split('|');
                rtResut = Models.User.Redo(userCode);
            }
            return rtResut;
        }

        [HttpPost]
        public int __Pass(FormCollection form)
        {
            int rtResut = 0;
            if (form["userCode"] != null)
            {
                string userCode = form["userCode"];
                rtResut = Models.User.Pass(userCode);
            }
            return rtResut;
        }

        public ActionResult _Edit()
        {
            string usercode = "";
            string userName = "";
            string groupid = "";
            if (Request.QueryString["userCode"] != null)
            {
                usercode = Request.QueryString["userCode"].ToString();
                userName = DbHelp.GetDbItem("NGZB_User", "userName", "userCode='" + usercode + "'", null);
                groupid = DbHelp.GetDbItem("NGZB_User", "groupID", "userCode='" + usercode + "'", null);
            }
            ViewBag.userCode = usercode;
            ViewBag.userName = userName;
            ViewBag.GroupID = groupid;
            return View();
        }

        [HttpPost]
        public int __Edit(FormCollection form)
        {
            if (form["userCode"] == null)
            {
                return -1;
            }
            if (form["userName"] == null)
            {
                return -1;
            }
            if (form["userPassWord"] == null)
            {
                return -1;
            }
            if (form["groupId"] == null)
            {
                return -1;
            }
            int groupID;
            if (int.TryParse(form["groupId"], out groupID) == false)
            {
                return -1;
            }
            string userCode = form["userCode"];
            string userName = form["userName"];
            string passWord = StringHelp.getMd5(form["userPassWord"]);
            return Models.User.Edit(userCode, userName, passWord, groupID, 1);
        }

        [HttpPost]
        public int __PassWordRedo(FormCollection form)
        {
            if (form["userCode"] == null)
            {
                return -1;
            }
            if (form["userPassWord"] == null)
            {
                return -1;
            }
            string userCode = form["userCode"];
            string passWord = StringHelp.getMd5(form["userPassWord"]);
            return Models.User.Edit(userCode, null, passWord, 0, 0);
        }

        public string __GetUserGroup(string roleID, string groupID)
        {
            int _groupID = 0;
            int _roleID = 0;

            if (groupID != null && roleID != null)
            {
                if (int.TryParse(groupID, out _groupID) == true && int.TryParse(roleID, out _roleID) == true)
                {
                    return Models.User.GetUserGroup(_groupID, _roleID);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        public int __ChangePassWord(FormCollection form)
        {
            if (form["oldpassword"] == null || form["userPassWord"] == null)
            {
                return -1;
            }
            string oldpassword = form["oldpassword"];
            string userPassWord = form["userPassWord"];
            SessionHelp session = new SessionHelp();
            string loginUserCode = session.GetSessionUser();
            return Models.User.ChangePassWord(loginUserCode, oldpassword, userPassWord);
        }

        public ActionResult _ChangePassWord()
        {
            return View();
        }
    }
}