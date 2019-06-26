using System;
using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class MyMsgController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __GetUser()
        {
            if (Request.QueryString["groupid"] != null)
            {
                int groupID = 0;
                int.TryParse(Request.QueryString["groupid"], out groupID);
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                return MyMsg.GetUserGroup(groupID, loginUserCode);
            }
            else
            {
                return "[]";
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public int __SendMessage(FormCollection form)
        {
            string msgtitle = "";
            if (form["msgtitle"] != null)
            {
                msgtitle = form["msgtitle"];
            }
            else
            {
                return 0;
            }
            string msginfo = "";
            if (form["msginfo"] != null)
            {
                msginfo = form["msginfo"];
            }
            string[] recUsercode = null;
            if (form["recUsercode"] != null)
            {
                recUsercode = form["recUsercode"].Split('|');
            }
            else
            {
                return 0;
            }
            if (recUsercode != null)
            {
                string fileUrl = "";//预留附件功能
                if (recUsercode.Length > 0)
                {
                    Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                    string loginUserCode = session.GetSessionUser();
                    return MyMsg.SendMessage(msgtitle, msginfo, loginUserCode, recUsercode, fileUrl);
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

        public string _GetMsgInfo()
        {
            if (Request.QueryString["personMsgID"] != null)
            {
                int personMsgID = 0;
                int.TryParse(Request.QueryString["personMsgID"], out personMsgID);
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                return MyMsg.GetMsgInfo(personMsgID, loginUserCode);
            }
            else
            {
                return "";
            }
        }

        public int __GetNoReadMsg()
        {
            int personMsgID = 0;
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            if (Request.QueryString["personMsgID"] != null)
            {
                int.TryParse(Request.QueryString["personMsgID"], out personMsgID);
            }
            else
            {
                return 0;
            }
            int noread = MyMsg.GetNoReadMsg(personMsgID, loginUserCode);
            return noread;
        }

        public int __DelRecMsg(FormCollection form)
        {
            if (form["msgid"] != null)
            {
                string[] msgid = form["msgid"].Split('|');
                if (msgid.Length > 0)
                {
                    Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                    string loginUserCode = session.GetSessionUser();
                    return MyMsg.DelMsg(msgid, loginUserCode, "R");
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        public int __DelSendMsg(FormCollection form)
        {
            if (form["msgid"] != null)
            {
                string[] msgid = form["msgid"].Split('|');
                if (msgid.Length > 0)
                {
                    Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                    string loginUserCode = session.GetSessionUser();
                    return MyMsg.DelMsg(msgid, loginUserCode, "S");
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        [HttpPost]
        public string __RecMsgBox(FormCollection form)
        {
            string keys = "";
            DateTime _st = DateTime.Now.AddDays(-7);
            DateTime _et = DateTime.Now;
            int readtype = -1;
            if (form["keys"] != null)
            {
                keys = form["keys"];
            }
            if (form["sdate"] != null)
            {
                if (DateTime.TryParse(form["sdate"], out _st) == false)
                {
                    _st = DateTime.Now.AddDays(-7);
                }
            }
            if (form["edate"] != null)
            {
                if (DateTime.TryParse(form["edate"], out _et) == false)
                {
                    _et = DateTime.Now;
                }
            }
            if (form["readtype"] != null)
            {
                int.TryParse(form["readtype"], out readtype);
            }
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            if (readtype != -1)
            {
                string st = _st.ToShortDateString() + " 00:00";
                string et = _et.ToShortDateString() + " 23:59";
                return MyMsg.MsgBox(loginUserCode, "rec", keys, st, et, readtype);
            }
            else
            {
                return MyMsg.MsgBox(loginUserCode, "rec");
            }
        }

        [HttpPost]
        public string __SendMsgBox(FormCollection form)
        {
            string keys = "";
            DateTime _st = DateTime.Now.AddDays(-7);
            DateTime _et = DateTime.Now;
            int readtype = -1;
            if (form["keys"] != null)
            {
                keys = form["keys"];
            }
            if (form["sdate"] != null)
            {
                if (DateTime.TryParse(form["sdate"], out _st) == false)
                {
                    _st = DateTime.Now.AddDays(-7);
                }
            }
            if (form["edate"] != null)
            {
                if (DateTime.TryParse(form["edate"], out _et) == false)
                {
                    _et = DateTime.Now;
                }
            }
            if (form["readtype"] != null)
            {
                int.TryParse(form["readtype"], out readtype);
            }
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            if (readtype != -1)
            {
                string st = _st.ToShortDateString() + " 00:00";
                string et = _et.ToShortDateString() + " 23:59";
                return MyMsg.MsgBox(loginUserCode, "send", keys, st, et, readtype);
            }
            else
            {
                return MyMsg.MsgBox(loginUserCode, "send");
            }
        }
    }
}