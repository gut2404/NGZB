using System.Collections.Generic;
using NGZB.Models.Class;

namespace NGZB.Models.Object
{
    /// <summary>
    /// 在线用户对象
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginUserCode { set; get; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string LoginUserName { set; get; }

        /// <summary>
        /// 所在部门代码
        /// </summary>
        public string LoginGroup { set; get; }

        /// <summary>
        /// 所在部门
        /// </summary>
        public string LoginGroupName { set; get; }

        /// <summary>
        /// 消息数量
        /// </summary>
        public string MsgCunt { set; get; }

        /// <summary>
        /// 识别码
        /// </summary>
        public string Tokenkey { set; get; }

        /// <summary>
        /// cookie是否保存
        /// </summary>
        public string CookieRemember { get; set; }

        public System.DateTime LoginTime { get; set; }

        /// <summary>
        /// 设置登录对象
        /// </summary>
        public LoginUser()
        {
            SessionHelp session = new SessionHelp();
            string userCode = session.GetSessionUser();
            if (userCode != null)
            {
                if (TokenDic.Dics.ContainsKey(userCode))
                {
                    LoginUserCode = TokenDic.Dics[userCode].LoginUserCode;
                    LoginUserName = TokenDic.Dics[userCode].LoginUserName;
                    LoginGroup = TokenDic.Dics[userCode].LoginGroup;
                    LoginGroupName = TokenDic.Dics[userCode].LoginGroupName;
                    MsgCunt = TokenDic.Dics[userCode].MsgCunt;
                    Tokenkey = TokenDic.Dics[userCode].Tokenkey;
                    CookieRemember = TokenDic.Dics[userCode].CookieRemember;
                    LoginTime = TokenDic.Dics[userCode].LoginTime;
                }
            }
        }
    }

    /// <summary>
    /// 用户登录、在线状态操作
    /// </summary>
    public class TokenDic
    {
        public static Dictionary<string, LoginUser> Dics = new Dictionary<string, LoginUser>();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        public static int SetLoginUser(string userCode, string password, string remember)
        {
            int rt = 0;
            string msgwhere = string.Format("recUserCode='{0}' AND readState=0", userCode);
            string tokenkey = StringHelp.getMd5(StringHelp.GetRandnum(8) + userCode);
            System.DateTime dt = System.DateTime.Now;
            if (Dics.ContainsKey(userCode))
            {
                Dics[userCode].MsgCunt = DbHelp.SearchNum("NGZB_PersonMsg", msgwhere).ToString();
                Dics[userCode].Tokenkey = tokenkey;
                Dics[userCode].LoginTime = System.DateTime.Parse(dt.ToString("G"));
                rt = 1;
            }
            else
            {
                if (password != null)
                {
                    string where = "userIsDelFlag=0 AND userCode='" + userCode + "' AND userPass='" + StringHelp.getMd5(password) + "'";
                    if (DbHelp.SearchNum("NGZB_User", where) == 1)
                    {
                        var loginuser = new LoginUser()
                        {
                            LoginUserCode = userCode,
                            LoginUserName = DbHelp.GetDbItem("V_NGZB_User", "userName", where, null).Trim(),
                            LoginGroup = DbHelp.GetDbItem("V_NGZB_User", "groupID", where, null).Trim(),
                            LoginGroupName = DbHelp.GetDbItem("V_NGZB_User", "groupName", where, null).Trim(),
                            MsgCunt = DbHelp.SearchNum("NGZB_PersonMsg", msgwhere).ToString(),
                            Tokenkey = tokenkey,
                            CookieRemember = remember,
                            LoginTime = System.DateTime.Parse(dt.ToString("G"))
                        };
                        Dics.Add(userCode, loginuser);
                        rt = 1;
                    }
                }
            }
            if (rt == 1)
            {
                SessionHelp session = new SessionHelp();
                SaveLoginInfo(userCode, session.BrowerInfo(), tokenkey, remember, dt);
                session.SetSessionUser(userCode, remember);
            }
            return rt;
        }

        /// <summary>
        /// 用户注销,关闭浏览器操作
        /// </summary>
        public static void ClearLoginUser(int actionType)
        {
            SessionHelp session = new SessionHelp();
            session.ClearSessionUser(actionType);
        }

        /// <summary>
        /// 保存登录用户浏览器信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="browerinfo"></param>
        private static void SaveLoginInfo(string userCode, BrowerInfo browerinfo, string tokenKey, string remember, System.DateTime dt)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            bool? isAutoLogin = true;
            if (remember != null)
            {
                isAutoLogin = false;
            }
            ctx.I_NGZB_UserLoginItem(userCode, browerinfo.Type, browerinfo.Browser, browerinfo.Version, browerinfo.VBScript, browerinfo.Cookies, browerinfo.ActiveXControls, browerinfo.AOL, browerinfo.UserHostAddress, browerinfo.UserHostName, browerinfo.Port, tokenKey, isAutoLogin, dt, ref rt);
        }
    }
}