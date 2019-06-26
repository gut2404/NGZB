using System.Data;
using NGZB.Models.Class;

namespace NGZB.Models
{
    /// <summary>
    /// 组织机构管理Model
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 返回组织机构json数据
        /// </summary>
        /// <returns>用于easyui comboboxtree、tree数据填充</returns>
        public static string Get()
        {
            string sql = "SELECT [groupID],[groupName],[groupParentID],[groupIcon] FROM [NGZB_Group]";
            string where = "groupIsDel=0";
            string orderby = "groupOrderBy ASC";
            string treeID = "groupID";
            string treeName = "groupName";
            string treeParentID = "groupParentID";
            string parentDefault = "0";
            EasyUiTreeJson jsontree = new EasyUiTreeJson();
            return jsontree.ReturnJson(sql, where, orderby, treeID, treeName, treeParentID, parentDefault);
        }

        public static string GetAll()
        {
            string sql = "SELECT [groupID], '['+ RTRIM(LTRIM(STR(groupOrderBy)))+']' + [groupName] AS [groupName],[groupParentID],[groupIcon],[groupIsDel] FROM [NGZB_Group]";
            string orderby = "groupOrderBy ASC";
            string treeID = "groupID";
            string treeName = "groupName";
            string treeParentID = "groupParentID";
            string parentDefault = "0";
            EasyUiTreeJson jsontree = new EasyUiTreeJson();
            return jsontree.ReturnJsonAll(sql, null, orderby, treeID, treeName, treeParentID, parentDefault);
        }

        public static int SetRoot(int groupID)
        {
            string upsql = string.Format("UPDATE NGZB_Group SET groupParentID=0 WHERE groupID={0}", groupID);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static int MoveGroup(int groupID, int parentID)
        {
            string upsql = string.Format("UPDATE NGZB_Group SET groupParentID={0} WHERE groupID={1}", parentID, groupID);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static int Del(int groupID)
        {
            string upsql = string.Format("UPDATE NGZB_Group SET groupIsDel=1 WHERE groupID={0}", groupID);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static int Redo(int groupID)
        {
            string upsql = string.Format("UPDATE NGZB_Group SET groupIsDel=0 WHERE groupID={0}", groupID);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static string[] GetGroupInfo(int groupID)
        {
            string selectSql = string.Format("SELECT [groupID],[groupName],[groupOrderBy],[groupIcon] FROM [NGZB_Group] WHERE groupID={0}", groupID);
            DataTable dt = DbHelp.ExcuteTable(selectSql, null, null);
            string[] rt = new string[4];
            if (dt.Rows.Count > 0)
            {
                rt[0] = groupID.ToString();
                rt[1] = dt.Rows[0]["groupName"].ToString();
                rt[2] = dt.Rows[0]["groupIcon"].ToString();
                rt[3] = dt.Rows[0]["groupOrderBy"].ToString();
                return rt;
            }
            else
            {
                return null;
            }
        }

        public static int Edit(int groupid, string groupName, string icon, int orderby)
        {
            string upSql = string.Format("UPDATE [NGZB_Group]SET [groupName]='{0}',[groupIcon]='{1}',[groupOrderBy]={2} WHERE groupID={3}", groupName, icon, orderby, groupid);
            return DbHelp.ExcuteNoQuery(upSql);
        }

        public static int Add(int groupid, string groupName, string icon, int orderby)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_Group(groupid, groupName, orderby, icon, ref rt);
            return (int)rt;
        }
    }
}