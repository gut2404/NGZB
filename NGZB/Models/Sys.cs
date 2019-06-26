using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NGZB.Models.Class;
using NGZB.Models.Object;

namespace NGZB.Models
{
    public class Sys
    {
        public static string GetInfo(string setValue)
        {
            return DbHelp.GetDbItem("NGZB_SystemSet", "setValue", "setItme='" + setValue + "'", null);
        }

        public static void LoadMode()
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            string viewsdir = AppDomain.CurrentDomain.BaseDirectory + @"Views\";
            DirectoryInfo dir = new DirectoryInfo(viewsdir);
            DirectoryInfo[] dirList = dir.GetDirectories();
            for (int i = 0; i < dirList.Length; i++)
            {
                if (dirList[i].Name.Substring(0, 1) != "_")
                {
                    FileInfo[] fileList = dirList[i].GetFiles();
                    string c = dirList[i].Name;
                    foreach (FileInfo nextfile in fileList)
                    {
                        string a = nextfile.Name.Substring(0, nextfile.Name.IndexOf('.'));
                        if (nextfile.Name.Substring(0, 1) != "_")
                        {
                            ctx.I_NGZB_Model(c, a, ref rt);
                        }
                    }
                }
            }
        }

        public static void LoadIcon(string iconname, int isSys)
        {
            string where = string.Format("iconClass='{0}'", iconname);
            if (DbHelp.SearchNum("NGZB_IconList", where) == 0)
            {
                string intosql = string.Format("INSERT INTO NGZB_IconList (iconId,iconClass,isSys) SELECT ISNULL(MAX(iconId)+1,1),'{0}',{1} FROM NGZB_IconList", iconname, isSys);
                DbHelp.ExcuteNoQuery(intosql);
            }
        }

        public static string GetModel(string modelControllers, string key)
        {
            string where = null;
            if (modelControllers != null)
            {
                where = string.Format("modelControllers='{0}'", modelControllers);
            }
            if (key != null)
            {
                if (where != null)
                {
                    where = where + string.Format(" AND (modelAction LIKE'%{0}%' OR modelName LIKE'%{1}%' OR modelInfo LIKE '%{2}%')", key, key, key);
                }
                else
                {
                    where = string.Format("(modelAction LIKE'%{0}%' OR modelName LIKE'%{1}%' OR modelInfo LIKE '%{2}%')", key, key, key);
                }
            }
            string sql = "SELECT a.modelID, a.modelControllers, a.modelAction, a.modelName, a.modelInfo, CASE WHEN b.isInit IS NULL THEN '否' ELSE '是' END AS homeview FROM NGZB_Model a LEFT JOIN NGZB_DesktopTabs b ON a.modelControllers = b.Controller AND a.modelAction = b.Active";
            DataTable dt = DbHelp.ExcuteTable(sql, where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int EditModelName(int modelId, string modelName, string modelinfo,int homeview)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.U_NGZB_Model(modelId, modelName, modelinfo, homeview, ref rt);
            return (int)rt;
        }

        public static string GetController()
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT DISTINCT [modelControllers] FROM [NGZB_Model]", null, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetIcon(string icontype, string keys)
        {
            if (keys == null)
            {
                keys = "";
            }
            string where = "";
            if (icontype == null || icontype == "-1")
            {
                where = "iconClass LIKE '%" + keys.ToString() + "%'";
            }
            else
            {
                where = "isSys=" + icontype + " AND iconClass LIKE '%" + keys.ToString() + "%'";
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT iconId,iconClass,SUBSTRING(iconClass,4,LEN(iconClass)-3)+'.png' AS imgname,isSys FROM NGZB_IconList", where, "iconId DESC");
            return JsonConvert.SerializeObject(dt);
        }

        public static int WriteCss(string cssName)
        {
            try
            {
                string sql = "SELECT DISTINCT [iconClass] FROM [NGZB_IconList]";
                DataTable dt = DbHelp.ExcuteTable(sql, null, null);
                int dtNum = dt.Rows.Count;
                if (dtNum > 0)
                {
                    string[] css = new string[dtNum];
                    string iconname = "";
                    string pngname = "";
                    for (int i = 0; i < dtNum; i++)
                    {
                        iconname = dt.Rows[i]["iconClass"].ToString().Trim();
                        pngname = iconname.Substring(3);
                        string s = ".icon-" + iconname + "{" + "background:url('icons/myicon/" + pngname + ".png') no-repeat center center;}";
                        css[i] = s;
                    }
                    string myicondir = AppDomain.CurrentDomain.BaseDirectory + @"Content\themes\" + cssName + ".css";
                    System.IO.File.WriteAllLines(myicondir, css, Encoding.UTF8);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static string GetMenuModel(string controller, int menugroup)
        {
            string where = string.Format("modelControllers='{0}' AND modelID NOT IN (SELECT modeID FROM NGZB_Menu WHERE menuParentID={1})", controller, menugroup);
            DataTable dt = DbHelp.ExcuteTable("SELECT modelID,modelName FROM  NGZB_Model", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetSet(string key)
        {
            string[] selectItem = { "setItme", "setName", "setValue", "setInfo" };
            string where = string.Format("(isAuto=0) AND (setName LIKE '%{0}%' OR setInfo LIKE '%{1}%')", key, key);
            DataTable dt = DbHelp.ExcuteTable(selectItem, "NGZB_SystemSet", where, "orderby ASC");
            return JsonConvert.SerializeObject(dt);
        }

        public static int SetSave(string setItme, string setValue, string setName, string setInfo)
        {
            ctxDbDataContext ctx = new Models.ctxDbDataContext();
            int? rt = 0;
            ctx.U_NGZB_SystemSet(setItme, setName, setValue, setInfo, ref rt);
            return (int)rt;
        }

        public static string IconList()
        {
            StringBuilder iconlist = new StringBuilder();
            DataTable dt = DbHelp.ExcuteTable("SELECT iconId AS [value],iconClass AS [text],'icon-'+iconClass AS iconCls FROM NGZB_IconList", null, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static string OnLineUser()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + TokenDic.Dics.Keys.Count + ",\"rows\":");
            return sb.ToString() + JsonConvert.SerializeObject(TokenDic.Dics.Values) + "}";
        }

        public static string DbConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["NGZB_dbConnectionString"].ConnectionString;
        }

        public static int AnsyUser()
        {
            string delSql = "DELETE NGZB_UserOnLine";
            DbHelp.ExcuteNoQuery(delSql, null);
            ctxDbDataContext ctx = new ctxDbDataContext();
            int suc = 0;
            int? rt = null;
            foreach (var item in TokenDic.Dics.Values)
            {
                ctx.I_NGZB_UserOnLine(item.LoginUserCode, item.LoginTime, item.Tokenkey, ref rt);
                if (rt == 1)
                {
                    suc += 1;
                }
            }
            if (suc > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static List<DesktopTabs> DesktopTabs()
        {
            var tabsList = new List<DesktopTabs>();
            string sql = "SELECT DISTINCT Controller,Active,TabsTitle,orderby  FROM NGZB_DesktopTabs";
            DataTable dt = DbHelp.ExcuteTable(sql, null, "orderby ASC");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DesktopTabs tabs = new DesktopTabs()
                    {
                        Controller = dt.Rows[i]["Controller"].ToString().Trim(),
                        Active = dt.Rows[i]["Active"].ToString().Trim(),
                        TabsTitle = dt.Rows[i]["TabsTitle"].ToString().Trim()
                    };
                    tabsList.Add(tabs);
                }
            }
            return tabsList;
        }

        public static int InitEnd()
        {
            string delinit = "DELETE NGZB_DesktopTabs WHERE isInit=1";
            DbHelp.ExcuteNoQuery(delinit, null);
            SessionHelp session = new SessionHelp();
            session.ClearSessionUser(1);
            return 1;
        }

        public static void LoadIcon(bool isNewCreate)
        {
            if (isNewCreate == true)
            {
                ctxDbDataContext ctx = new ctxDbDataContext();
                int? rt = null;
                ctx.D_NGZB_IconList(ref rt);
            }
            string myicondir = AppDomain.CurrentDomain.BaseDirectory + @"Content\themes\icons\myicon";
            DirectoryInfo dir = new DirectoryInfo(myicondir);
            FileInfo[] filenames = dir.GetFiles(); //获取该文件夹下面的所有文件
            int isSys = 1;
            foreach (FileInfo _iconname in filenames)
            {
                if (_iconname.Name.Substring(0, 3) == "up_")
                {
                    isSys = 0;
                }
                else
                {
                    isSys = 1;
                }
                string iconname = "my_" + _iconname.Name.Substring(0, _iconname.Name.IndexOf('.'));
                LoadIcon(iconname, isSys);
            }
        }

        public static int DelIcon(int iconid)
        {
            string iconfile = DbHelp.GetDbItem("NGZB_IconList", "iconClass", "iconId=" + iconid, null);
            if (iconfile != "")
            {
                iconfile = iconfile.Substring(3) + ".png";
            }
            if (DbHelp.ExcuteNoQuery("DELETE NGZB_IconList WHERE iconId=" + iconid) > 0)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Content\themes\icons\myicon\" + iconfile;
                FileInfo fileio = new FileInfo(filePath);
                if (fileio.Exists)
                {
                    fileio.Delete();
                }
                return 1;
            }
            return 0;
        }
        public static string GetUnit()
        {
            string sql = "SELECT unitID AS id,unitName AS [text] FROM NGZB_Unit";
            DataTable dataTable = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dataTable);
        }
    }
}