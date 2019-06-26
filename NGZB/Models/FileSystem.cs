using System.Data;
using System.Text;
using Newtonsoft.Json;
using NGZB.Models.Class;
namespace NGZB.Models
{
    public class FileSystem
    {
        public static string Get(string userCode)
        {
            string sql = "SELECT [documentID],[documentName],[documentParentID] FROM [NGZB_F_Document]";
            string where = "isDel=0";
            string orderby = "documentOrderBy ASC";
            string treeID = "documentID";
            string treeName = "documentName";
            string treeParentID = "documentParentID";
            string parentDefault = "0";
            EasyUiTreeJson jsontree = new EasyUiTreeJson();
            return jsontree.ReturnJson(sql, where, orderby, treeID, treeName, treeParentID, parentDefault, userCode);
        }

        public static string GetFileList(string where)
        {
            string orderby = "[fileID] DESC";
            DataTable dt = DbHelp.ExcuteTable("SELECT [fileID],[fileName],[userName],[fileInfo],CONVERT(varchar(16),[fileUpDate], 120) AS [fileUpDate] FROM V_NGZB_F_File", where, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetNewFileList(int rowCount)
        {
            string orderby = "[fileID] DESC";
            DataTable dt = DbHelp.ExcuteTable("SELECT TOP " + rowCount + " [fileID],[fileName],[userName],[fileInfo],CONVERT(varchar(16),[fileUpDate], 120) AS [fileUpDate] FROM V_NGZB_F_File", null, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        public static string DocumentPass(int doucmentID, string userCode)
        {
            string rt = "";
            string add = "0";
            string del = "0";
            string allwhere = string.Format("documentID={0} AND userCode='{1}' AND allowType=0", doucmentID, userCode);
            if (DbHelp.SearchNum("NGZB_F_DocumentAllow", allwhere) > 0)
            {
                rt = "1|1|1|1";
            }
            else
            {
                string addwhere = string.Format("documentID={0} AND userCode='{1}' AND allowType=1", doucmentID, userCode);
                string delwhere = string.Format("documentID={0} AND userCode='{1}' AND allowType=2", doucmentID, userCode);
                if (DbHelp.SearchNum("NGZB_F_DocumentAllow", addwhere) > 0)
                {
                    add = "1";
                }
                if (DbHelp.SearchNum("NGZB_F_DocumentAllow", delwhere) > 0)
                {
                    del = "1";
                }
                rt = string.Format("0|{0}|{1}", add, del);
            }
            return rt;
        }

        public static int DelFile(int fileID)
        {
            string delsql = string.Format("DELETE NGZB_F_File WHERE fileID={0}", fileID);
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZB_F_File(fileID, ref rt);
            if (rt == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int AddFile(int documentID, string fileName, string fileUrl, string fileInfo, string upUserCode)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_F_File(documentID, fileName, fileUrl, upUserCode, fileInfo, ref rt);
            if (rt == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string GetParentDocumet(int parentID, string keys)
        {
            string orderby = "documentOrderBy ASC";
            string where = string.Format("documentParentID={0}", parentID);
            if (keys != null)
            {
                where = string.Format("documentParentID={0} AND (documentName LIKE '%{1}%' OR documentInfo LIKE '%{2}%')", parentID, keys, keys);
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT [documentID],[documentParentID],[documentName],[documentCounts],[fileCount],[isDel],[documentOrderBy],[documentInfo] FROM [NGZB_F_Document]", where, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        public static int EditDocument(int docId, string docName, string docInfo,int orderBy)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.U_NGZB_F_Document(docId, docName, docInfo,orderBy, ref rt);
            return (int)rt;
        }

        public static int AddDocument(int docId, string docName, string docInfo,int orderBy)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_F_Document(docId, docName, docInfo, orderBy, ref rt);
            return (int)rt;
        }

        public static int DelDocument(int documentID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZ_F_Document(documentID, 1, ref rt);
            return (int)rt;
        }

        public static int ReDocument(int documentID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZ_F_Document(documentID, 0, ref rt);
            return (int)rt;
        }

        public static int MoveDoucmet(int ducID, int taggetid)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.U_NGZB_F_Document_M(ducID, taggetid, ref rt);
            return (int)rt;
        }

        public static string GetNoAllowUser(int gropuID, int ducid, int allowtype)
        {
            string where = string.Format("userIsDelFlag=0 AND groupID={0} AND userCode NOT IN (SELECT userCode FROM NGZB_F_DocumentAllow WHERE documentID={1} AND allowType={2})", gropuID, ducid, allowtype);
            DataTable dt = DbHelp.ExcuteTable("SELECT userCode,userName FROM NGZB_User", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int DocumentAddAllow(int ducid, string usercode, int allowtype)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_F_DocumentAllow(ducid, usercode, allowtype, ref rt);
            return (int)rt;
        }

        public static string GetDocumentAllow(int ducid)
        {
            string where = string.Format("documentID={0}", ducid);
            DataTable dt = DbHelp.ExcuteTable("SELECT documentAllowID,userName,allowType FROM V_NGZB_F_DocumentAllow", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int DelDocumentAllow(int ducAllowId)
        {
            string sql = string.Format("DELETE [NGZB_F_DocumentAllow] WHERE documentAllowID={0}", ducAllowId);
            return DbHelp.ExcuteNoQuery(sql);
        }

        public static int SetRoot(int ducid)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.U_NGZB_F_Document_S(ducid, ref rt);
            return (int)rt;
        }
    }
}