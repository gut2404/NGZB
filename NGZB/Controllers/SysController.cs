using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;
using NGZB.Models.Object;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class SysController : Controller
    {
        public ActionResult Help()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult LoadIcon()
        {
            Sys.LoadIcon(false);
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult LoadModel()
        {
            Sys.LoadMode();
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult OnLineUser()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult Set()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        public static string __CompanyName()
        {
            return Sys.GetInfo("company");
        }

        /// <summary>
        /// 系统easyui主题样式
        /// </summary>
        /// <returns></returns>
        public static string __SystemThems()
        {
            string[] themTemp = { "default", "bootstrap", "gray", "material", "metro", "black", "ui-cupertino", "ui-dark-hive", "ui-pepper-grinder", "ui-sunny" };
            string thems = "";
            try
            {
                thems = Sys.GetInfo("systemthems");
            }
            catch
            {
                thems = "default";
            }
            if(thems == "")
            {
                return "default";
            }
            else
            {
                var quere = from _them in themTemp
                            where _them == thems
                            select _them;
                if(!quere.Any())
                {
                    thems = "default";
                }
                return thems;
            }
        }

        /// <summary>
        /// 按钮样式，平面或者立体
        /// </summary>
        /// <returns></returns>
        public static string __ThemsType()
        {
            string thems = Sys.GetInfo("themstype");
            if(thems == "")
            {
                return "true";
            }
            else
            {
                return thems;
            }
        }

        /// <summary>
        /// 允许上传文件的类型
        /// </summary>
        /// <returns></returns>
        public static string __UpFileType()
        {
            string upfiletype = Sys.GetInfo("upfiletype");
            if(upfiletype == "")
            {
                return "'.jpg', '.png', '.rar', '.txt', '.zip', '.doc', '.ppt', '.xls', '.pdf', '.docx', '.xlsx', '.docx'";
            }
            else
            {
                return upfiletype;
            }
        }

        /// <summary>
        /// 允许上传文件的大小
        /// </summary>
        /// <returns></returns>
        public static int __UpFileMax()
        {
            string _upfilemaxsize = Sys.GetInfo("upfilemaxsize");
            int upfilemaxsize = 0;
            if(int.TryParse(_upfilemaxsize, out upfilemaxsize) == false)
            {
                upfilemaxsize = 4;
            }
            return upfilemaxsize;
        }

        /// <summary>
        /// 首页显示新文件数量
        /// </summary>
        /// <returns></returns>
        public static int __NewFileListCount()
        {
            string newfilelistcount = Sys.GetInfo("newfilelistcount");
            int rowcount = 0;
            int.TryParse(newfilelistcount, out rowcount);
            if(rowcount == 0)
            {
                return 10;
            }
            else
            {
                if(rowcount > 50)
                {
                    return 50;
                }
                return 10 * (rowcount / 10);
            }
        }

        /// <summary>
        /// 首页显示公共数量
        /// </summary>
        /// <returns></returns>
        public static int __MessageListCount()
        {
            string messagelistcount = Sys.GetInfo("messagelistcount");
            int rowcount = 0;
            int.TryParse(messagelistcount, out rowcount);
            if(rowcount == 0)
            {
                return 10;
            }
            else
            {
                if(rowcount > 50)
                {
                    return 50;
                }
                return 10 * (rowcount / 10);
            }
        }

        /// <summary>
        /// 是否允许所有人发布功能
        /// </summary>
        /// <returns></returns>
        public static int __MessageAllowType()
        {
            int rt = 0;
            int.TryParse(Sys.GetInfo("allowmessage"), out rt);
            return rt;
        }

        /// <summary>
        /// 是否隐藏文件描述
        /// </summary>
        /// <returns></returns>
        public static string __FileInfoHidden()
        {
            string _tempstr = Sys.GetInfo("fileinfohidden");
            if(_tempstr == "")
            {
                return "true";
            }
            else
            {
                return _tempstr;
            }
        }

        /// <summary>
        /// 表格分页大小
        /// </summary>
        /// <returns></returns>
        public static int __PageSize()
        {
            int pageSize = 30;
            string _tempstr = "30";
            try
            {
                _tempstr = Sys.GetInfo("datagridpagesize");
            }
            catch
            {
                _tempstr = "30";
            }
            int.TryParse(_tempstr, out pageSize);
            if(pageSize == 0)
            {
                pageSize = 30;
            }
            return pageSize;
        }

        /// <summary>
        /// 是否禁止鼠标默认右键
        /// </summary>
        /// <returns></returns>
        public static int __EnableRightBtn()
        {
            string _tempstr = "";
            try
            {
                _tempstr = Sys.GetInfo("enablerightbtn");
            }
            catch
            {
                _tempstr = "0";
            }
            int rt = 0;
            if(int.TryParse(_tempstr, out rt) == false || rt != 1)
            {
                rt = 0;
            }
            else
            {
                rt = 1;
            }
            return rt;
        }

        /// <summary>
        /// 是否压缩css和js文件
        /// </summary>
        /// <returns></returns>
        public static bool __EnabelZipCssAndJs()
        {
            string _tempstr = "";
            try
            {
                _tempstr = Sys.GetInfo("enablezipcssjs");
            }
            catch
            {
                _tempstr = "1";
            }
            bool rt = true;
            if(_tempstr != "1")
            {
                rt = false;
            }
            return rt;
        }

        public static string __CopyRight()
        {
            string _tempstr = Sys.GetInfo("copyright");
            return _tempstr;
        }

        public static int __MsgAnsyTime()
        {
            string _tempstr = Sys.GetInfo("msgansytime");
            int time = 0;
            int.TryParse(_tempstr, out time);
            return time * 60000;
        }

        /// <summary>
        /// 设备树加载是否开启异步加载，由系统自动更新维护
        /// </summary>
        /// <returns></returns>
        public static bool __EqIsAnsy()
        {
            string _tempstr = Sys.GetInfo("eqisansy");
            return _tempstr.ToLower() == "true" ? true : false;
        }

        public static string __IconAlign()
        {
            string _tempstr = Sys.GetInfo("iconalign").ToLower();
            if(_tempstr == "top" || _tempstr == "right" || _tempstr == "left" || _tempstr == "bottom")
            {
                return _tempstr;
            }
            else
            {
                return "left";
            }
        }

        public static int __AllowOpenTabNum()
        {
            string _tempstr = Sys.GetInfo("allowtabnum").ToLower();
            int tabnum = 8;
            int.TryParse(_tempstr, out tabnum);
            if(tabnum == 0)
            {
                tabnum = 8;
            }
            return tabnum;
        }

        public static object __TokenKey()
        {
            LoginUser loginuser = new LoginUser();
            return new { tokenkey = loginuser.Tokenkey };
        }

        public static string __TokenKeyStr()
        {
            LoginUser loginuser = new LoginUser();
            return loginuser.Tokenkey;
        }

        public static int __CookiesTime()
        {
            string _tempstr = Sys.GetInfo("cookiestime");
            int _rt = 30;
            int.TryParse(_tempstr, out _rt);
            if(_rt == 0)
            {
                _rt = 7
;
            }
            return _rt;
        }

        public static string __PublishTool()
        {
            return "tools:'#publishtool'";
        }

        public static string __SystemTitle()
        {
            return Sys.GetInfo("systemtitle");
        }

        [HttpPost]
        public string __GetModel(FormCollection form)
        {
            if(form["c"] != null)
            {
                if(form["key"] != null)
                {
                    return Sys.GetModel(form["c"], form["key"]);
                }
                else
                {
                    return Sys.GetModel(form["c"], null);
                }
            }
            else
            {
                if(form["key"] != null)
                {
                    return Sys.GetModel(null, form["key"]);
                }
                else
                {
                    return Sys.GetModel(null, null);
                }
            }
        }

        public string __GetMenuModel()
        {
            if(Request.QueryString["controller"] != null && Request.QueryString["menugroup"] != null)
            {
                int menugroup = 0;
                if(int.TryParse(Request.QueryString["menugroup"], out menugroup) == false)
                {
                    return "";
                }
                return Sys.GetMenuModel(Request.QueryString["controller"], menugroup);
            }
            else
            {
                return "[]";
            }
        }

        public int __EditModelName(FormCollection form)
        {
            if(form["modelID"] != null && form["modelName"] != null)
            {
                int modelid = 0;
                if(int.TryParse(form["modelID"], out modelid) == true)
                {
                    string modelinfo = "";
                    if(form["modelInfo"] != null)
                    {
                        modelinfo = form["modelinfo"];
                    }
                    int homeview = 0;
                    if(form["homeview"] != null)
                    {
                        if(form["homeview"] == "是")
                        {
                            homeview = 1;
                        }
                    }
                    if(Sys.EditModelName(modelid, form["modelName"], modelinfo, homeview) > 0)
                    {
                        return 1;
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
            else
            {
                return 0;
            }
        }

        public string __GetController()
        {
            return Sys.GetController();
        }

        public string __GetIcon(string icontype, string keys)
        {
            return Sys.GetIcon(icontype, keys);
        }

        public int __WriteCss()
        {
            return Sys.WriteCss("myicon");
        }

        public string __IconList()
        {
            return Sys.IconList();
        }

        public int __UpIcon()
        {
            HttpPostedFileBase file = Request.Files["upfile"];
            if(file != null)
            {
                string myicondir = AppDomain.CurrentDomain.BaseDirectory + @"Content\themes\icons\myicon\";
                string fileName = file.FileName;
                FileInfo fileio = new FileInfo(myicondir + fileName);
                if(fileio.Exists)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddhhmmssff") + Path.GetExtension(file.FileName);
                }
                try
                {
                    file.SaveAs(myicondir + "up_" + fileName);
                    DirectoryInfo dir = new DirectoryInfo(myicondir);
                    FileInfo[] filenames = dir.GetFiles(); //获取该文件夹下面的所有文件
                    foreach(FileInfo _iconname in filenames)
                    {
                        string iconname = "my_" + _iconname.Name.Substring(0, _iconname.Name.IndexOf('.'));
                        Sys.LoadIcon(iconname, 0);
                    }
                    return 1;
                }
                catch
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
        public int __SetSave(FormCollection form)
        {
            if(form["setItme"] != null && form["setValue"] != null)
            {
                string setItme = form["setItme"].Trim();
                string setValue = form["setValue"].Trim();
                string setInfo = form["setInfo"] ?? "";
                string setName = form["setName"] ?? "";
                if(string.IsNullOrEmpty(setItme) || string.IsNullOrEmpty(setValue))
                {
                    return 0;
                }
                else
                {
                    return Sys.SetSave(setItme, setValue, setName, setInfo);
                }
            }
            else
            {
                return 0;
            }
        }

        public string __GetSet(FormCollection form)
        {
            if(form["key"] != null)
            {
                return Sys.GetSet(form["key"]);
            }
            else
            {
                return Sys.GetSet("");
            }
        }

        public ActionResult _AddIcon()
        {
            ViewBag.up = __UpIcon();
            return View();
        }

        public string __OnLineUser()
        {
            return Sys.OnLineUser();
        }

        public int __AnsyUser()
        {
            return Sys.AnsyUser();
        }

        public ActionResult _InitEnd()
        {
            return View();
        }

        public int __InitEnd()
        {
            return Sys.InitEnd();
        }

        public int __NewCreateCss()
        {
            Sys.LoadIcon(true);
            return 1;
        }

        public int __DelIcon(string iconid)
        {
            if(iconid != null)
            {
                int _iconid = 0;
                if(int.TryParse(iconid, out _iconid) == false)
                {
                    return -1;
                }
                return Sys.DelIcon(_iconid);
            }
            return -1;
        }

        public ActionResult _AddEditHelp(string modelid)
        {
            ViewBag.modelid = modelid;
            return View();
        }

        public string __GetUnit()
        {
            return Sys.GetUnit();
        }
    }
}