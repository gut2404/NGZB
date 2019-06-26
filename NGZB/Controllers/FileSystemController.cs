using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class FileSystemController : Controller
    {
        public ActionResult DoucmentManage()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult Manage()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __GetDocumentTree()
        {
            return FileSystem.Get(Session["loginUserCode"].ToString());
        }

        public string __GetFileList(FormCollection form)
        {
            string where = null;

            if (form["documentID"] != null)
            {
                int documentID;
                int.TryParse(form["documentID"], out documentID);
                if (documentID != 0)
                {
                    where = string.Format("documentID={0}", documentID);
                }
                if (form["keys"] != null)
                {
                    string keys = form["keys"];
                    if (where == null)
                    {
                        where = string.Format("(fileName LIKE '%{0}%'  OR fileInfo LIKE '%{1}%')", keys, keys);
                    }
                    else
                    {
                        where = where + " AND " + string.Format("(fileName LIKE '%{0}%'  OR fileInfo LIKE '%{1}%')", keys, keys);
                    }
                }
                return FileSystem.GetFileList(where);
            }
            else
            {
                return "[]";
            }
        }

        public string __GetManageFileList(FormCollection form)
        {
            if (form["documentID"] != null)
            {
                int documentID;
                int.TryParse(form["documentID"], out documentID);
                string where = string.Format("documentID={0}", documentID);
                if (form["keys"] != null)
                {
                    string keys = form["keys"];
                    where = where + " AND " + string.Format("(fileName LIKE '%{0}%'  OR fileInfo LIKE '%{1}%')", keys, keys);
                }
                return FileSystem.GetFileList(where);
            }
            else
            {
                return "[]";
            }
        }

        public string __GetNewFileList()
        {
            int rowCount = SysController.__NewFileListCount();
            return FileSystem.GetNewFileList(rowCount);
        }

        public ActionResult __DownFile(string fileid)
        {
            int _fileid;
            int.TryParse(fileid, out _fileid);
            string filename = DbHelp.GetDbItem("NGZB_F_File", "fileName", string.Format("fileID={0}", _fileid), null);
            string fileurl = DbHelp.GetDbItem("NGZB_F_File", "fileUrl", string.Format("fileID={0}", _fileid), null);
            if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(fileurl))
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "UpFile\\PublicFile\\";
                FileInfo fileio = new FileInfo(path + fileurl);
                if (!fileio.Exists)
                {
                    return View("_DownErr");
                }
                return File(new FileStream(path + fileurl, FileMode.Open), "text/plain", filename);
            }
            else
            {
                return View("_DownErr");
            }
        }

        public string __DocumentPass(FormCollection form)
        {
            if (form["documentID"] != null)
            {
                int documentID;
                int.TryParse(form["documentID"], out documentID);
                SessionHelp session = new SessionHelp();
                string loginUserCode = session.GetSessionUser();
                if (loginUserCode == null)
                {
                    return "0|0|0";
                }
                return FileSystem.DocumentPass(documentID, loginUserCode);
            }
            else
            {
                return "0|0|0";
            }
        }

        public int __DelFile(FormCollection form)
        {
            if (form["deldoucment"] != null)
            {
                string[] deldocumet = form["deldoucment"].Split('|');
                SessionHelp session = new SessionHelp();
                string loginUserCode = session.GetSessionUser();

                if (loginUserCode == null)
                {
                    return -1;
                }
                int suc = 0;
                string where = string.Format("documentID={0} AND userCode='{1}' AND (allowType=0 OR allowType=2)", deldocumet[0], loginUserCode);
                if (DbHelp.SearchNum("NGZB_F_DocumentAllow", where) > 0)
                {
                    for (int i = 1; i < deldocumet.Length; i++)
                    {
                        int fileid;
                        int.TryParse(deldocumet[i], out fileid);
                        string fileurl = DbHelp.GetDbItem("NGZB_F_File", "fileUrl", "fileID=" + fileid + "", null);
                        if (FileSystem.DelFile(fileid) == 1)
                        {
                            string path = AppDomain.CurrentDomain.BaseDirectory + @"UpFile\PublicFile\";
                            FileInfo fileio = new FileInfo(path + fileurl);
                            if (fileio.Exists)
                            {
                                try { fileio.Delete(); } catch { }
                            }
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
                return -1;
            }
        }

        public ActionResult _AddFile()
        {
            if (Request.QueryString["documentID"] != null)
            {
                int documentid;
                SessionHelp session = new SessionHelp();
                string loginUserCode = session.GetSessionUser();
                if (loginUserCode == null)
                {
                    ViewBag.doucmentID = 0;
                    ViewBag.documentName = "";
                    return View();
                }
                int.TryParse(Request.QueryString["documentID"], out documentid);
                string where = string.Format("documentID={0} AND userCode='{1}' AND (allowType=0 OR allowType=1)", documentid, loginUserCode);
                if (DbHelp.SearchNum("NGZB_F_DocumentAllow", where) > 0)
                {
                    ViewBag.doucmentID = documentid;
                    ViewBag.documentName = DbHelp.GetDbItem("NGZB_F_Document", "documentName", "documentID=" + documentid, null);
                }
                else
                {
                    ViewBag.doucmentID = 0;
                    ViewBag.documentName = "";
                }
            }
            else
            {
                ViewBag.doucmentID = 0;
                ViewBag.documentName = "";
            }
            ViewBag.upok = "";
            return View();
        }

        [HttpPost]
        public int _AddFile(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files["upfile"];
            int rt = 0;
            if (file != null)
            {
                string fileurl = DateTime.Now.ToString("yyyyMMddhhmmssff") + Path.GetExtension(file.FileName);
                string path = AppDomain.CurrentDomain.BaseDirectory + "UpFile\\PublicFile\\" + fileurl;
                try
                {
                    ViewBag.doucmentID = Request.QueryString["doucmentID"];
                    ViewBag.documentName = Request.QueryString["documetName"];
                    int docmentID;
                    int.TryParse(Request.QueryString["doucmentID"], out docmentID);
                    if (docmentID != 0)
                    {
                        file.SaveAs(path);
                        string fileName = "";
                        fileName = file.FileName.LastIndexOf(@"\", StringComparison.Ordinal) > 0 ? file.FileName.Substring(file.FileName.LastIndexOf(@"\", StringComparison.Ordinal)).Replace(@"\", "") : file.FileName;
                        string fileInfo = "";
                        if (Request.QueryString["fileinfo"] != null)
                        {
                            fileInfo = Request.QueryString["fileinfo"];
                        }
                        SessionHelp session = new SessionHelp();
                        string loginUserCode = session.GetSessionUser();
                        if (FileSystem.AddFile(docmentID, fileName, fileurl, fileInfo, loginUserCode) == 1)
                        {
                            ViewBag.upok = "1";
                            rt = 1;
                        }
                        else
                        {
                            try
                            {
                                FileInfo _file = new FileInfo(path);
                                _file.Delete();
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        ViewBag.upok = "-2";
                        rt = -2;
                    }
                }
                catch
                {
                    ViewBag.doucmentID = Request.QueryString["doucmentID"];
                    ViewBag.documentName = Request.QueryString["documetName"];
                    ViewBag.upok = "0";
                }
            }
            else
            {
                ViewBag.upok = "-1";
                rt = -1;
            }
            return rt;
        }

        public string __GetParentDocumet(FormCollection form)
        {
            if (form["documentID"] != null)
            {
                string keys = "";
                if (form["key"] != null)
                {
                    keys = form["key"];
                }
                int documentID;
                int.TryParse(form["documentID"], out documentID);
                return FileSystem.GetParentDocumet(documentID, keys);
            }
            else
            {
                return FileSystem.GetParentDocumet(0, null);
            }
        }

        public int __EditDocument(FormCollection form)
        {
            if (form["docid"] != null && form["docname"] != null)
            {
                int docid;
                if (int.TryParse(form["docid"], out docid) == false || string.IsNullOrEmpty(form["docname"]))
                {
                    return 0;
                }
                else
                {
                    string docname = form["docname"];
                    string docinfo = "";
                    if (form["docinfo"] != null)
                    {
                        docinfo = form["docinfo"];
                    }
                    int orderby = 0;
                    if (form["oderby"] != null)
                    {
                        int.TryParse(form["oderby"].ToString(), out orderby);
                    }
                    return FileSystem.EditDocument(docid, docname, docinfo, orderby);
                }
            }
            else
            {
                return 0;
            }
        }

        public int __AddDocument(FormCollection form)
        {
            if (form["docid"] != null && form["docname"] != null)
            {
                int docid;
                if (int.TryParse(form["docid"], out docid) == false || string.IsNullOrEmpty(form["docname"]))
                {
                    return 0;
                }
                else
                {
                    string docname = form["docname"];
                    string docinfo = "";
                    if (form["docinfo"] != null)
                    {
                        docinfo = form["docinfo"];
                    }
                    int orderby = 0;
                    if (form["oderby"] != null)
                    {
                        int.TryParse(form["oderby"].ToString(), out orderby);
                    }
                    return FileSystem.AddDocument(docid, docname, docinfo, orderby);
                }
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __MoveDoucmet(FormCollection form)
        {
            if (form["id"] != null && form["targetId"] != null)
            {
                int ducid;
                int taggetid;
                int.TryParse(form["id"], out ducid);
                int.TryParse(form["targetId"], out taggetid);
                return FileSystem.MoveDoucmet(ducid, taggetid);
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public int __DelDocument(FormCollection form)
        {
            if (form["ducid"] != null)
            {
                int documentID;
                int.TryParse(form["ducid"], out documentID);
                return FileSystem.DelDocument(documentID);
            }
            else
            {
                return 0;
            }
        }

        public int __ReDocument(FormCollection form)
        {
            if (form["ducid"] != null)
            {
                int documentID;
                int.TryParse(form["ducid"], out documentID);
                return FileSystem.ReDocument(documentID);
            }
            else
            {
                return 0;
            }
        }

        public ActionResult _AddDoucmentAllow()
        {
            if (Request.QueryString["ducid"] != null && Request.QueryString["ducname"] != null)
            {
                ViewBag.ducid = Request.QueryString["ducid"];
                ViewBag.ducname = Request.QueryString["ducname"];
            }
            else
            {
                ViewBag.ducid = "";
                ViewBag.ducname = "";
            }
            return View();
        }

        public string __GetNoAllowUser()
        {
            if (Request.QueryString["dpid"] != null && Request.QueryString["ducid"] != null && Request.QueryString["allowtype"] != null)
            {
                int gropuID;
                int.TryParse(Request.QueryString["dpid"], out gropuID);
                int ducid;
                int.TryParse(Request.QueryString["ducid"], out ducid);
                int allowtype;
                if (int.TryParse(Request.QueryString["allowtype"], out allowtype) == false)
                {
                    allowtype = -1;
                }
                return FileSystem.GetNoAllowUser(gropuID, ducid, allowtype);
            }
            else
            {
                return "[]";
            }
        }

        public int __DocumentAddAllow(FormCollection form)
        {
            if (form["ducid"] != null && form["usercode"] != null && form["allowtype"] != null)
            {
                int ducid;
                int allowtype;
                if (int.TryParse(form["ducid"], out ducid) == false || int.TryParse(form["allowtype"], out allowtype) == false)
                {
                    return -1;
                }
                string[] _usercode = form["usercode"].Split('|');
                if (_usercode.Length > 0)
                {
                    int suc = 0;
                    foreach (string usercode in _usercode)
                    {
                        if (FileSystem.DocumentAddAllow(ducid, usercode, allowtype) == 1)
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
                return -1;
            }
        }

        public string __GetDocumentAll()
        {
            if (Request.QueryString["ducid"] != null)
            {
                int ducid;
                int.TryParse(Request.QueryString["ducid"], out ducid);
                return FileSystem.GetDocumentAllow(ducid);
            }
            else
            {
                return "[]";
            }
        }

        public int __DelDocumentAllow(FormCollection form)
        {
            if (form["ducallowid"] != null)
            {
                int ducAllowId;
                int.TryParse(form["ducallowid"], out ducAllowId);
                int i = FileSystem.DelDocumentAllow(ducAllowId);
                return i;
            }
            else
            {
                return -1;
            }
        }

        public int __SetRoot(FormCollection form)
        {
            if (form["ducid"] != null)
            {
                int ducid;
                return int.TryParse(form["ducid"], out ducid) ? FileSystem.SetRoot(ducid) : 0;
            }
            else
            {
                return 0;
            }
        }
    }
}