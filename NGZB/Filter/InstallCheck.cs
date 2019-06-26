using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace NGZB.Filter
{
    public class InstallCheck : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ContentResult NoServerIp = new ContentResult()
            {
                Content = "<div style=\"text-align:center;color:red;width:100%; font-size:16px\">只允许服务器本地操作，禁止客户端该操作......</div>"
            };
            ContentResult NoFile = new ContentResult()
            {
                Content = "<div style=\"text-align:center;color:red;width:100%; font-size:16px\">找不到初始配置文件，无法初始化...... </div>"
            };
            string initFile = AppDomain.CurrentDomain.BaseDirectory + @"Content\init.xml";
            FileInfo fileio = new FileInfo(initFile);
            if (!fileio.Exists)
            {
                filterContext.Result = NoFile;
            }
            else
            {
                var xdoc = XElement.Load(initFile);
                var initdata = from items in xdoc.Descendants("installbase") select new InitInfo { Installallow = items.Element("installallow").Value, Serverip = items.Element("serverip").Value, IPMD5 = items.Element("ipmd5").Value };
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                Models.Object.BrowerInfo brower = session.BrowerInfo();
                bool ipPass = false;
                bool isallow = false;
                bool ipmd5 = false;
                foreach (var item in initdata)
                {
                    if (ipPass == false || isallow == false)
                    {
                        if (isallow == false && item.Installallow == "4553A8DE-8380-4E56-AD47-D3E603AC73EC")
                        {
                            isallow = true;
                        }
                        if (ipPass == false && item.Serverip == brower.UserHostAddress)
                        {
                            ipPass = true;
                        }
                        if (item.IPMD5 == Models.Class.StringHelp.getMd5(item.Serverip))
                        {
                            ipmd5 = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (ipPass == false || isallow == false || ipmd5 == false)
                {
                    filterContext.Result = NoServerIp;
                }
            }
        }
    }

    public class InitInfo
    {
        public string Installallow { get; set; }
        public string Serverip { get; set; }
        public string IPMD5 { get; set; }
    }
}