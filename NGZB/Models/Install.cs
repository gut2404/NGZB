using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class Install
    {
        public static string GetXmlModel()
        {
            string initFile = AppDomain.CurrentDomain.BaseDirectory + @"Content\init.xml";
            var xdoc = XElement.Load(initFile);
            var xmlmodel = from items in xdoc.Descendants("initmodel") select new { modelname = items.Element("modelname").Value, modelinfo = items.Element("modelinfo").Value, modelactive = items.Element("modelactive").Value, isinit = items.Element("isinit").Value, orderbys = int.Parse(items.Element("orderbys").Value) };
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + xmlmodel.Count().ToString() + ",\"rows\":");
            return sb.ToString() + JsonConvert.SerializeObject(xmlmodel.Where(p => p.isinit == "0").OrderBy(s => s.orderbys)) + "}";
        }

        public static int TestConn(string server, string uid, string pwd)
        {
            string Sqlconn = "Data Source=" + server + ";Initial Catalog=NGZB_db;User ID=" + uid + ";Password=" + pwd + "";
            return Class.DbHelp.SearchNum(Sqlconn, "master.sys.sysdatabases", "name='NGZB_db'");
        }

        public static int SaveConn(string server, string uid, string pwd)
        {
            string Sqlconn = "Data Source=" + server + ";Initial Catalog=NGZB_db;User ID=" + uid + ";Password=" + pwd + "";
            string SqlProvide = "System.Data.SqlClient";
            if (TestConn(server, uid, pwd) == 1)
            {
                try
                {
                    Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                    config.ConnectionStrings.ConnectionStrings["NGZB_dbConnectionString"].ConnectionString = Sqlconn;
                    config.ConnectionStrings.ConnectionStrings["NGZB_dbConnectionString"].ProviderName = SqlProvide;
                    XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                    if (xmlhelp.FileExists == true)
                    {
                        if (xmlhelp.Edit("initmodel", "modelactive", "Dbconn", "isinit", "1") == true)
                        {
                            config.Save();
                            return 1;
                        }
                        return 0;
                    }
                    return 0;
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        public static int DeleteInitFile()
        {
            XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
            if (xmlhelp.DelXmlFile() == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int InitGroup(string groupname)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_Group(groupname, ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "Group", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public static int InitUser(string userCode, string userName, string passWord, int groupID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_User(userCode, userName, passWord, groupID, ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "User", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public static int InitModel()
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_Model(ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "Model", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public static int InitMenu()
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_Menu(ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "Menu", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public static int InitRole()
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_Role(ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "Role", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public static int InitUserRole()
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.C_NGZB_Init_UserRole(ref rt);
            if (rt == 1)
            {
                XmlHelp xmlhelp = new XmlHelp(@"Content\init.xml");
                if (xmlhelp.Edit("initmodel", "modelactive", "UserRole", "isinit", "1") == true)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }
    }
}