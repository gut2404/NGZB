using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;
using NGZB.Models.Object;

namespace NGZB.Models.Class
{
    public class SessionHelp
    {
        private HttpSessionState _session = HttpContext.Current.Session;
        private HttpRequest _request = HttpContext.Current.Request;
        private HttpResponse _response = HttpContext.Current.Response;
        private readonly string cookiesKey = StringHelp.getMd5("NGZBC00KIE");
        private readonly string cookiesUser = StringHelp.getMd5("L0CALUSER");
        private readonly string cookiesUserMd5 = StringHelp.getMd5("LoCALUSERMD5");

        /// <summary>
        /// 保存当前登录用户
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="remember"></param>
        public void SetSessionUser(string userCode, string remember)
        {
            if (remember != null)
            {
                if (remember == "true")
                {
                    HttpCookie cookie = new HttpCookie(cookiesKey);
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = new TimeSpan(Controllers.SysController.__CookiesTime(), 0, 0, 0, 0);//过期时间(天)
                    cookie.Expires = dt.Add(ts);//设置过期
                    cookie.Values.Add(cookiesUser, userCode);
                    cookie.Values.Add(cookiesUserMd5, StringHelp.getMd5(userCode));
                    _response.AppendCookie(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(cookiesKey)
                    {
                        Expires = DateTime.Now.AddHours(-1)
                    };
                    _response.Cookies.Add(cookie);
                }
            }
            _session["loginUserCode"] = userCode;
        }

        /// <summary>
        /// 返回当前登录用户的用户名
        /// </summary>
        /// <returns>当前登录用户名</returns>
        public string GetSessionUser()
        {
            HttpCookie cokie = new HttpCookie(cookiesKey);
            if (_request.Cookies[cookiesKey] != null)
            {
                if (StringHelp.getMd5(_request.Cookies[cookiesKey][cookiesUser]) != _request.Cookies[cookiesKey][cookiesUserMd5])
                {
                    return null;
                }
                return _request.Cookies[cookiesKey][cookiesUser];
            }
            else
            {
                if (_session["loginUserCode"] != null)
                {
                    return _session["loginUserCode"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///注销当前登录用户
        /// </summary>
        /// <param name="actionType">1为主动注销，0位关闭浏览器</param>
        public void ClearSessionUser(int actionType)
        {
            SessionHelp session = new SessionHelp();
            string userCode = session.GetSessionUser();
            if (userCode != null)
            {
                if (actionType == 1 || (actionType == 0 && TokenDic.Dics[userCode].CookieRemember != "true"))
                {
                    if (TokenDic.Dics.ContainsKey(userCode))
                    {
                        SqlParameter[] ps = { new SqlParameter("@userCode", System.Data.SqlDbType.VarChar) };
                        ps[0].Value = userCode;
                        DbHelp.ExcuteNoQuery("DELETE NGZB_UserOnLine WHERE userCode=@userCode", ps);
                        TokenDic.Dics.Remove(userCode);
                    }
                    HttpCookie cookie = new HttpCookie(cookiesKey)
                    {
                        Expires = DateTime.Now.AddHours(-1)
                    };
                    _response.Cookies.Add(cookie);
                    _session.Abandon();
                }
            }
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.I_NGZB_UserLoginOutType(userCode, actionType, _request.UserHostAddress, ref rt);
        }

        /// <summary>
        /// 获取当前登录用户浏览器信息
        /// </summary>
        /// <returns></returns>
        public BrowerInfo BrowerInfo()
        {
            BrowerInfo _brower = new BrowerInfo()
            {
                Type = _request.Browser.Type,
                Browser = _request.Browser.Browser,
                Version = _request.Browser.Version,
                VBScript = _request.Browser.VBScript,
                Cookies = _request.Browser.Cookies,
                ActiveXControls = _request.Browser.ActiveXControls,
                AOL = _request.Browser.AOL,
                UserHostAddress = _request.UserHostAddress,
                UserHostName = _request.UserHostName,
                DnsSafeHost = _request.Url.DnsSafeHost,
                Port = _request.Url.Port
            };
            return _brower;
        }
    }
}