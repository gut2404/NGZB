using System.Web.Mvc;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    //[LoginPass]
    //[CheckUrl]
    public class SafeController : Controller
    {
        public ActionResult Harm()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------

        public string __GetSafeStand(string serchkey, string harmtypeid)
        {
            int _harmtypeid = 0;
            if (harmtypeid != null)
            {
                int.TryParse(harmtypeid, out _harmtypeid);
            }
            return Safe.GetSafeStand(serchkey, _harmtypeid);
        }

        [ValidateInput(false)]
        public ActionResult _AddHarm()
        {
            return View();
        }

        public string __GetWorkType()
        {
            return Safe.GetWorkType();
        }

        [ValidateInput(false)]
        public int __AddHarm(int workTypeID, string securityInfo, string defendAgainst)
        {
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            return Safe.AddHarm(workTypeID, securityInfo, defendAgainst, loginUserCode);
        }

        public ActionResult _EditHarm(int safeHarmID)
        {
            V_NGZB_Harm harm = Safe.Harm(safeHarmID);
            if (harm != null)
            {
                ViewBag.safeHarmID = harm.safeHarmID;
                ViewBag.securityInfo = harm.securityInfo;
                ViewBag.defendAgainst = harm.defendAgainst;
                ViewBag.harmTypeID = harm.workTypeID;
            }
            return View();
        }

        [ValidateInput(false)]
        public int __EditHarm(int safeHarmID, int workTypeID, string securityInfo, string defendAgainst)
        {
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            return Safe.EditHarm(safeHarmID, workTypeID, securityInfo, defendAgainst, loginUserCode);
        }
    }
}