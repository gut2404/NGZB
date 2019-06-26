using System.Data;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class Role
    {
        public static int Del(string[] roleID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int suc = 0;
            int? rt = 0;
            for (int i = 0; i < roleID.Length; i++)
            {
                int _roleID = 0;
                int.TryParse(roleID[i], out _roleID);
                ctx.D_NGZB_Role(_roleID, ref rt);
                if (rt == 1)
                {
                    suc = suc + 1;
                }
            }
            return suc;
        }

        public static int Pass(string roleName)
        {
            return DbHelp.SearchNum("NGZB_Role", "roleName='" + roleName + "'");
        }

        public static int Add(string roleName, string roleInfo)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_Role(roleName, roleInfo, 0, ref rt);
            if (rt == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string GetComboBox()
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT roleID,roleName FROM NGZB_Role", null, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static string Get(int selectFlag, string key)
        {
            string where = "";
            switch (selectFlag)
            {
                case 1:
                    where = string.Format("(roleName LIKE '%{0}%')", key);
                    break;

                case 2:
                    where = string.Format("roleID IN (SELECT roleID FROM V_NGZB_Role_Item WHERE menuName LIKE '%{0}%')", key);
                    break;

                case 3:
                    where = string.Format("roleID IN (SELECT roleID FROM V_NGZB_Role_User WHERE userName LIKE '%{0}%')", key);
                    break;

                case 4:
                    where = string.Format("roleID IN (SELECT roleID FROM V_NGZB_Role_User WHERE userCode LIKE '%{0}%')", key);
                    break;

                default:
                    where = string.Format("(roleName LIKE '%{0}%')", key);
                    break;
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT * FROM [NGZB_Role]", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="menuID">菜单ID</param>
        /// <param name="appendUserCode">授权人用户名</param>
        /// <returns></returns>
        public static int AddRoleItem(int roleId, int menuID, string appendUserCode)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_Role_Item(roleId, menuID, appendUserCode, ref rt);
            return rt == 1 ? 1 : 0;
        }

        public static string GetRoleItem(string roleid)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [roleItemID],[menuName],[menuInfo] FROM [V_NGZB_Role_Item]", string.Format("[roleID]={0}", roleid), null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int DelRoleItem(int roleitemID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZB_Role_Item(roleitemID, ref rt);
            if (rt == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string GetUserRole(int roleID)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT DISTINCT [roleUserID],[userName],[groupName],[roleAddUser],CONVERT(varchar(100), [addDate], 23) AS [addDate] FROM [V_NGZB_Role_UserInfo]", string.Format("[roleID]={0}", roleID), null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int AddUserRole(int roleid, string[] userCode, string opUser)
        {
            int suc = 0;
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            for (int i = 0; i < userCode.Length; i++)
            {
                ctx.I_NGZB_Role_User(roleid, userCode[i], opUser, ref rt);
                if (rt == 1)
                {
                    suc = suc + 1;
                }
            }
            return suc;
        }

        public static int DelUserRole(int roleUserID, string opUser)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZB_Role_User(roleUserID, opUser, ref rt);
            if (rt == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string GetRoleInfo(int roleid)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [menuName] FROM [V_NGZB_Role_Item]", string.Format("[roleID]={0}", roleid), null);
            string rtStr = "";
            DbHelp.ExcuteNoQuery(string.Format("UPDATE NGZB_Role SET roleItemCount={0} WHERE roleID={1}", dt.Rows.Count, roleid));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    rtStr = rtStr + "<li style=\"color:blue\">" + row["menuName"] + "</li>";
                }
                return "<ol>" + rtStr + "</ol>";
            }
            else
            {
                return "没有业务";
            }
        }

        public static string GetRoleUser(int roleid)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [userName] FROM [V_NGZB_Role_User]", string.Format("[roleID]={0}", roleid), null);
            string rtStr = "";
            DbHelp.ExcuteNoQuery(string.Format("UPDATE NGZB_Role SET roleUserCount={0} WHERE roleID={1}", dt.Rows.Count, roleid));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    rtStr = rtStr + "<li style=\"color:blue\">" + row["userName"] + "</li>";
                }
                return "<ol>" + rtStr + "</ol>";
            }
            else
            {
                return "没有用户";
            }
        }
    }
}