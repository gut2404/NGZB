using System.Data;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class Message
    {
        public static int AddMessage(string msgtitle, string msg, string userCode)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_Message(msgtitle, msg, userCode, ref rt);
            return (int)rt;
        }

        public static string GetMsgInfo(int msgid)
        {
            string upReadMsgNum = string.Format("UPDATE [NGZB_Message] SET [readNum] =[readNum]+1 WHERE msgID={0}", msgid);
            DbHelp.ExcuteNoQuery(upReadMsgNum, null);
            return DbHelp.GetDbItem("NGZB_Message", "msginfo", "msgid=" + msgid, null);
        }

        public static void MessageReadRecord(int msgid, string userCode)
        {
            string readRecordSQL = string.Format("INSERT INTO NGZB_MessageReadRecord(msgid,userCode,readDate)VALUES({0},'{1}',GETDATE())", msgid, userCode);
            DbHelp.ExcuteNoQuery(readRecordSQL);
        }

        public static string GetMessageList(int msgcount, string where)
        {
            string top = "";
            if (msgcount > 0)
            {
                top = "TOP " + msgcount;
            }
            string sql = "SELECT " + top + " [msgID],[msgID] AS [delmsgID],[msgUserCode],[msgTitle],[userName],[groupName],CONVERT(varchar(100), [msgDate], 120) AS [msgDate] FROM [V_NGZB_Message]";
            string orderby = "msgDate DESC";
            DataTable dt = DbHelp.ExcuteTable(sql, where, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        public static int MessageAddCheck(string userCode)
        {
            string where = string.Format("allowType=1 AND userCode='{0}'", userCode);
            if (DbHelp.SearchNum("NGZB_MessageAllow", where) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int MessageDelCheck(string userCode)
        {
            string where = string.Format("allowType=0 AND userCode='{0}'", userCode);
            if (DbHelp.SearchNum("NGZB_MessageAllow", where) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int DelMessage(int msgid)
        {
            string delSql = "DELETE NGZB_Message WHERE msgID=" + msgid;
            return DbHelp.ExcuteNoQuery(delSql, null);
        }

        public static string GetMsgTitle(int msgid)
        {
            string where = "msgID=" + msgid;
            return DbHelp.GetDbItem("NGZB_Message", "msgTitle", where, null);
        }

        public static int MessageIsMy(int msgid, string usercode)
        {
            string where = string.Format("msgUserCode='{0}' AND msgID={1}", usercode, msgid);
            return DbHelp.SearchNum("NGZB_Message", where);
        }
    }
}