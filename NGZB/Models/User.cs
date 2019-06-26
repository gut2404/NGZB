using System.Data;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class User
    {
        public static int Del(string[] userCode)
        {
            int suc = 0;
            string upSql = "";
            for (int i = 0; i < userCode.Length; i++)
            {
                upSql = string.Format("UPDATE NGZB_User SET userIsDelFlag=1 WHERE userCode='{0}'", userCode[i]);
                if (DbHelp.ExcuteNoQuery(upSql, null) > 0)
                {
                    suc = suc + 1;
                }
            }
            return suc;
        }

        public static int Redo(string[] userCode)
        {
            int suc = 0;
            string upSql = "";
            for (int i = 0; i < userCode.Length; i++)
            {
                upSql = string.Format("UPDATE NGZB_User SET userIsDelFlag=0 WHERE userCode='{0}'", userCode[i]);
                if (DbHelp.ExcuteNoQuery(upSql, null) > 0)
                {
                    suc = suc + 1;
                }
            }
            return suc;
        }

        public static int Pass(string userCode)
        {
            return DbHelp.SearchNum("NGZB_User", "userCode='" + userCode + "'");
        }

        public static int Add(string userCode, string userName, string passWord, int groupID)
        {
            if (DbHelp.SearchNum("NGZB_User", "userCode='" + userCode + "'") == 0)
            {
                ctxDbDataContext ctx = new ctxDbDataContext();
                int? rt = 0;
                ctx.I_NGZB_User(userCode, userName, passWord, groupID, ref rt);
                return (int)rt;
            }
            else
            {
                return 0;
            }
        }

        public static int Edit(string userCode, string userName, string password, int groupID, int opType)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.U_NGZB_User(userCode, userName, password, groupID, opType, ref rt);
            return (int)rt;
        }

        public static string GetUserGroup(int groupID, int roleID)
        {
            string where = string.Format("userIsDelFlag=0 AND groupID={0} AND userCode NOT IN (SELECT userCode FROM NGZB_Role_User WHERE roleID={1})", groupID, roleID);
            DataTable dt = DbHelp.ExcuteTable("SELECT userCode,userName FROM NGZB_User", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int ChangePassWord(string userCode, string oldPassword, string newPassword)
        {
            string where = string.Format("userCode='{0}' AND userPass='{1}'", userCode, StringHelp.getMd5(oldPassword));
            if (DbHelp.SearchNum("NGZB_User", where) > 0)
            {
                newPassword = StringHelp.getMd5(newPassword);
                string upSql = string.Format("UPDATE NGZB_User SET userPass='{0}' WHERE userCode='{1}'", newPassword, userCode);
                return DbHelp.ExcuteNoQuery(upSql, null);
            }
            else
            {
                return 0;
            }
        }

        public static string GetUserGroup(int groupID)
        {
            string where = string.Format("userIsDelFlag=0 AND groupID={0}", groupID);
            DataTable dt = DbHelp.ExcuteTable("SELECT userCode,userName FROM NGZB_User", where, null);
            return JsonConvert.SerializeObject(dt);
        }
    }
}