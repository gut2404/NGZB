using System.Web.Mvc;
using NGZB.Models;
using NGZB.Models.Class;
using NGZB.Models.Object;

namespace NGZB.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        //使用参数自动装填技术
        public string treejson(string id)
        {
            return Test.treejson();
        }

        public ActionResult Test2()
        {
            return View(EquipMent.EqModel(1));
        }

        public string GetSession()
        {
            for (int j = 0; j < Session.Count; j++)
            {

            }
            return Session.Count.ToString();
        }
        public void SetCookies(string usercode)
        {
            Response.Cookies["usercode"].Value = usercode;
        }
        public void GetCookies(string usercode)
        {
            if (Request.Cookies["usercode"] != null)
            {
                Response.Write(Server.HtmlEncode(Request.Cookies["usercode"].Value));
            }
            else
            {
                Response.Write("没有用户");
            }
        }
        public  string getall()
        {
            string rt = "";
            SessionHelp session = new SessionHelp();
            //foreach(var item in session.BrowerInfo())
            //{
            //    rt = rt + item.ToString() + "|";
            //}
            BrowerInfo browerinfo = session.BrowerInfo();
            rt = StringHelp.getMd5("::1");
            return rt;
        }
    }
}