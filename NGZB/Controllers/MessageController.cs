using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class MessageController : Controller
    {
        public ActionResult AllMessage()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult DelMessage()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __GetMessageList()
        {
            int msgcount = SysController.__MessageListCount();
            return Message.GetMessageList(msgcount, null);
        }

        public string __GetMessageAll(FormCollection form)
        {
            string key = null;
            if (form["keys"] != null)
            {
                key = form["keys"];
            }
            string date = null;
            string sdate = "";
            string edate = "";
            if (form["sdate"] != null && form["edate"] != null)
            {
                sdate = form["sdate"] + " 00:00:00";
                edate = form["edate"] + " 23:59:59";
            }
            else
            {
                sdate = System.DateTime.Now.AddDays(-30).ToShortDateString() + " 00:00:00";
                edate = System.DateTime.Now.ToShortDateString() + " 23:59:59";
            }
            date = string.Format("(msgDate BETWEEN '{0}' AND '{1}')", sdate, edate);
            string where = string.Format("(msgTitle LIKE '%{0}%' OR msgInfo LIKE '%{1}%')", key, key);
            where = where + " AND " + date;
            int msgcount = 500;
            return Message.GetMessageList(msgcount, where);
        }

        public string __GetMessage()
        {
            if (Request.QueryString["msgid"] != null)
            {
                int msgid = 0;
                int.TryParse(Request.QueryString["msgid"], out msgid);
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                Message.MessageReadRecord(msgid, loginUserCode.ToString());
                return Message.GetMsgInfo(msgid);
            }
            else
            {
                return "";
            }
        }

        [ValidateInput(false)]
        public int __AddMessage(FormCollection form)
        {
            if (form["msgtitle"] != null && form["msg"] != null)
            {
                string msgtitle = form["msgtitle"];
                string msg = form["msg"];
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                return Message.AddMessage(msgtitle, msg, loginUserCode);
            }
            else
            {
                return -1;
            }
        }

        public int __MessageAddCheck()
        {
            if (SysController.__MessageAllowType() == 1)
            {
                return 1;
            }
            else
            {
                if (Session["loginUserCode"] == null)
                {
                    return 0;
                }
                else
                {
                    return Message.MessageAddCheck(Session["loginUserCode"].ToString());
                }
            }
        }

        public int __MessageDelCheck()
        {
            if (Session["loginUserCode"] == null)
            {
                return 0;
            }
            else
            {
                return Message.MessageDelCheck(Session["loginUserCode"].ToString());
            }
        }

        public int __DelMessage(FormCollection form)
        {
            if (form["msgid"] != null)
            {
                int msgid = 0;
                int.TryParse(form["msgid"], out msgid);
                return Message.DelMessage(msgid);
            }
            else
            {
                return 0;
            }
        }

        public ActionResult _MessageInfo()
        {
            if (Request.QueryString["msgid"] != null)
            {
                int msgid = 0;
                int.TryParse(Request.QueryString["msgid"], out msgid);
                ViewBag.msg = Message.GetMsgInfo(msgid);
                ViewBag.msgtitle = Message.GetMsgTitle(msgid);
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                if (Message.MessageIsMy(msgid, loginUserCode) > 0)
                {
                    ViewBag.ismy = "1";
                }
                else
                {
                    ViewBag.ismy = "0";
                }
                ViewBag.msgid = msgid;
            }
            else
            {
                ViewBag.msg = "";
                ViewBag.msgtitle = "";
                ViewBag.ismy = "0";
                ViewBag.msgid = "";
            }
            return View();
        }
    }
}