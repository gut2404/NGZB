using System.Data;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class MyMsg
    {
        public static string MsgBox(string userCode, string msgBox)
        {
            string where = "";
            if (msgBox.ToLower() == "send")
            {
                where = string.Format("sendUserCode='{0}'", userCode);
            }
            else
            {
                where = string.Format("recUserCode='{0}'", userCode);
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT [personMsgID],[msgtitle],[msgInfo],[sendUserCode],[recUserCode],CONVERT(varchar(16),[sendDate],120) AS [sendDate],CONVERT(varchar(16),[readDate],120) AS [readDate],[readState],[fileUrl],[sendUserName],[recUserName] FROM [V_NGZB_PersonMsg]", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int SendMessage(string msgtitle, string msginfo, string sendUserCode, string[] recUserCode, string fileUrl)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            int suc = 0;
            for (int i = 0; i < recUserCode.Length; i++)
            {
                ctx.I_NGZB_PersonMsg(msgtitle, msginfo, sendUserCode, recUserCode[i], fileUrl, ref rt);
                if (rt == 1)
                {
                    suc++;
                }
            }
            return suc;
        }

        public static string GetMsgInfo(int personMsgID, string userCode)
        {
            string where = string.Format("personMsgID={0} AND (recUserCode='{1}' OR sendUserCode='{2}')", personMsgID, userCode, userCode);
            return DbHelp.GetDbItem("NGZB_PersonMsg", "msgInfo", where, null);
        }

        public static int GetNoReadMsg(int personMsgID, string userCode)
        {
            string isread = string.Format("personMsgID={0} AND [readState]=0", personMsgID);
            if (DbHelp.SearchNum("NGZB_PersonMsg", isread) > 0)
            {
                string upsql = string.Format("UPDATE NGZB_PersonMsg SET [readDate] = GETDATE(),[readState] = 1 WHERE [personMsgID]={0}", personMsgID);
                DbHelp.ExcuteNoQuery(upsql);
            }
            string noreadSqlWhere = string.Format("[recUserCode]='{0}' AND readState=0", userCode);
            return DbHelp.SearchNum("NGZB_PersonMsg", noreadSqlWhere);
        }

        public static string GetUserGroup(int groupID, string selfUserCode)
        {
            string where = string.Format("userIsDelFlag=0 AND groupID={0} AND userCode<>'{1}'", groupID, selfUserCode);
            DataTable dt = DbHelp.ExcuteTable("SELECT userCode,userName FROM NGZB_User", where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static int DelMsg(string[] msgid, string userCode, string delType)
        {
            int suc = 0;
            int _msg = 0;
            string delusercode = "";
            if (delType == "R")
            {
                delusercode = "recUserCode";
            }
            else
            {
                delusercode = "sendUserCode";
            }
            for (int i = 0; i < msgid.Length; i++)
            {
                string delsql = "";
                if (int.TryParse(msgid[i], out _msg) == true)
                {
                    delsql = string.Format("UPDATE NGZB_PersonMsg SET {0}=NULL WHERE personMsgID={1}", delusercode, _msg);
                    if (DbHelp.ExcuteNoQuery(delsql) > 0)
                    {
                        suc = suc + 1;
                    }
                }
            }
            return suc;
        }

        public static string MsgBox(string userCode, string msgBox, string key, string st, string et, int readtype)
        {
            string where = "";
            if (msgBox == "send")
            {
                where = string.Format("(sendUserCode='{0}') AND (msgtitle LIKE '%{1}%' OR msgInfo LIKE '{2}' OR recUserName LIKE '%{3}%') AND (sendDate BETWEEN '{4}'  AND '{5}') AND (readState={6})", userCode, key, key, key, st, et, readtype);
            }
            else
            {
                where = string.Format("(recUserCode='{0}') AND (msgtitle LIKE '%{1}%' OR msgInfo LIKE '{2}' OR sendUserName LIKE '%{3}%') AND (sendDate BETWEEN '{4}'  AND '{5}') AND (readState={6})", userCode, key, key, key, st, et, readtype);
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT [personMsgID],[msgtitle],[msgInfo],[sendUserCode],[recUserCode],CONVERT(varchar(16),[sendDate],120) AS [sendDate],CONVERT(varchar(16),[readDate],120) AS [readDate],[readState],[fileUrl],[sendUserName],[recUserName] FROM [V_NGZB_PersonMsg]", where, null);
            return JsonConvert.SerializeObject(dt);
        }
    }
}