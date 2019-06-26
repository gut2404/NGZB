using System;
using System.Web.Mvc;
using NGZB.Filter;
using NGZB.Models;
using NGZB.Models.Class;

namespace NGZB.Controllers
{
    [LoginPass]
    [CheckUrl]
    public class EquipMentController : Controller
    {
        public ActionResult OperatingRules()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        public ActionResult ManageEqTree()
        {
            ViewBag.modelID = PublishMethod.GetModelID(RouteData);
            return View();
        }

        //-----------------------分割线，以下为无界面或者为子界面的方法，要添加主界面的方法，在此线以上添加--------------------
        public string __GetEqTree()
        {
            return EquipMent.GetEqTree();
        }

        public string __GetChildTree(FormCollection form)
        {
            if(form["eqID"] != null)
            {
                int parentID = 0;
                int.TryParse(form["eqID"].ToString(), out parentID);
                string key = null;
                if(!string.IsNullOrEmpty(form["searchkey"]))
                {
                    key = form["searchkey"].ToString().Trim();
                }
                return EquipMent.GetChildTree(parentID, key);
            }
            else
            {
                return EquipMent.GetChildTree(0, null);
            }
        }

        public ActionResult _AddEq()
        {
            int eqParentID = 0;
            string eqName = "根节点";
            string eqcode = "";
            if(!string.IsNullOrEmpty(Request.QueryString["eqParentID"]))
            {
                int.TryParse(Request.QueryString["eqParentID"].ToString(), out eqParentID);
                V_NGZB_EQ_BaseTree eqinfo = EquipMent.EqModel(eqParentID);
                eqName = eqinfo == null ? "根节点" : eqinfo.eqName;
                eqcode = eqinfo == null ? "" : eqinfo.eqCode;
            }
            ViewBag.eqParentID = eqParentID;
            ViewBag.eqName = eqName;
            ViewBag.eqcode = eqcode;
            return View();
        }

        public int __AddEq(FormCollection form)
        {
            if(string.IsNullOrEmpty(form["parentid"]) ||
                string.IsNullOrEmpty(form["treename"]) ||
                string.IsNullOrEmpty(form["opeartetype"]) ||
                form["codename"] == null ||
                form["px"] == null ||
                form["geticon"] == null ||
                form["treeinfo"] == null ||
                form["nodeopen"] == null ||
                form["installdate"] == null ||
                form["opearteType"] == null)
            {
                return -1;
            }
            int parentId = 0;
            int.TryParse(form["parentid"].ToString(), out parentId);
            string treeName = form["treename"].ToString().Trim();
            string codeName = form["codename"].ToString().Trim();
            int orderby = 0;
            int.TryParse(form["px"].ToString(), out orderby);
            string getIcon = form["geticon"].ToString().Trim();
            if(getIcon == "")
            {
                getIcon = "my_eq";
            }
            string info = form["treeinfo"].ToString().Trim();
            string nodeOpen = "open";
            nodeOpen = form["nodeopen"].ToString().Trim();
            if(nodeOpen != "open" && nodeOpen != "closed")
            {
                nodeOpen = "open";
            }
            DateTime _installDateTime;
            DateTime? installDateTime;
            if(DateTime.TryParse(form["installdate"].ToString().Trim(), out _installDateTime) == false)
            {
                installDateTime = null;
            }
            else
            {
                installDateTime = _installDateTime;
            }
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            int opearteType = int.Parse(form["opeartetype"].ToString());
            int _eqClassID = 0;
            int? eqClassID = null;
            string eqTypeSpecification = null;
            string eqSupplier = null;
            string eqManufacturer = null;
            int eqIsVirtual = 1;
            try
            {
                if(int.TryParse(form["eqclassid"], out _eqClassID) == true && _eqClassID > 0)
                {
                    eqClassID = _eqClassID;
                }
                if(int.TryParse(form["eqisvirtual"], out eqIsVirtual) == false)
                {
                    eqIsVirtual = 1;
                }
                eqTypeSpecification = form["eqtypetpecification"].ToString().Trim();
                eqSupplier = form["eqsupplier"].ToString().Trim();
                eqManufacturer = form["eqmanufacturer"].ToString().Trim();
            }
            catch { }
            int eqSpareparts = 0, eqMajor = 0;
            if(form["eqspareparts"] != null)
            {
                int.TryParse(form["eqspareparts"].ToString(), out eqSpareparts);
            }
            if(form["eqmajor"] != null)
            {
                int.TryParse(form["eqmajor"].ToString(), out eqMajor);
            }
            return EquipMent.Add(parentId, treeName, codeName, orderby, getIcon, info, installDateTime, nodeOpen, eqClassID, eqTypeSpecification, eqSupplier, eqManufacturer, loginUserCode, opearteType, eqIsVirtual, eqSpareparts, eqMajor);
        }

        public ActionResult _EditEq()
        {
            int eqID = 0;
            string eqName = "根节点";
            string eqCode = "";
            string eqOrderBy = "";
            string eqInstallDate = "";
            string eqIcon = "my_eq";
            string Info = "";
            string nodeOpen = "open";
            string eqClassID = "-1";
            string eqTypeSpecification = "";
            string eqSupplier = "";
            string eqManufacturer = "";
            string eqIsVirtual = "1";
            string eqSpareparts = "0";
            string eqMajor = "0";
            V_NGZB_EQ_BaseTree eqModel = null;
            if(!string.IsNullOrEmpty(Request.QueryString["eqID"]))
            {
                int.TryParse(Request.QueryString["eqID"].ToString(), out eqID);
                eqModel = EquipMent.EqModel(eqID);
                if(eqModel != null)
                {
                    eqCode = eqModel.eqCode;
                    eqName = eqModel.eqName;
                    Info = eqModel.eqInfo;
                    eqInstallDate = eqModel.eqInstallDate.ToString();
                    eqOrderBy = eqModel.eqOrderBy.ToString();
                    eqIcon = eqModel.eqIcon;
                    nodeOpen = eqModel.nodeOpen;
                    eqClassID = eqModel.eqClassID.ToString() == "" ? "-1" : eqModel.eqClassID.ToString();
                    eqTypeSpecification = eqModel.eqTypeSpecification;
                    eqSupplier = eqModel.eqSupplier;
                    eqManufacturer = eqModel.eqManufacturer;
                    eqIsVirtual = eqModel.eqIsVirtual.ToString() == "" ? "1" : eqModel.eqIsVirtual.ToString();
                    eqSpareparts = eqModel.eqSpareparts.ToString() == "" ? "0" : eqModel.eqSpareparts.ToString();
                    eqMajor = eqModel.eqMajor.ToString() == "" ? "0" : eqModel.eqMajor.ToString();
                }
            }
            ViewBag.eqID = eqID;
            ViewBag.eqName = eqName;
            ViewBag.eqCode = eqCode;
            ViewBag.eqOrderBy = eqOrderBy;
            ViewBag.eqInstallDate = eqInstallDate;
            ViewBag.eqIcon = eqIcon;
            ViewBag.eqInfo = Info;
            ViewBag.nodeOpen = nodeOpen;
            ViewBag.eqClassID = eqClassID;
            ViewBag.eqTypeSpecification = eqTypeSpecification;
            ViewBag.eqSupplier = eqSupplier;
            ViewBag.eqManufacturer = eqManufacturer;
            ViewBag.eqIsVirtual = eqIsVirtual;
            ViewBag.eqSpareparts = eqSpareparts;
            ViewBag.eqMajor = eqMajor;
            return View();
        }

        public int __EditEq(FormCollection form)
        {
            if(string.IsNullOrEmpty(form["eqid"]) ||
                string.IsNullOrEmpty(form["treename"]) ||
                string.IsNullOrEmpty(form["opeartetype"]) ||
                form["codename"] == null ||
                form["px"] == null ||
                form["geticon"] == null ||
                form["treeinfo"] == null ||
                form["nodeopen"] == null ||
                form["installdate"] == null)
            {
                return -1;
            }
            int eqID = 0;
            int.TryParse(form["eqid"].ToString(), out eqID);
            string treeName = form["treename"].ToString().Trim();
            string codeName = form["codename"].ToString().Trim();
            int orderby = 0;
            int.TryParse(form["px"].ToString(), out orderby);
            string getIcon = form["geticon"].ToString().Trim();
            if(getIcon == "")
            {
                getIcon = "my_eq";
            }
            string info = form["treeinfo"].ToString().Trim();
            string nodeOpen = "open";
            nodeOpen = form["nodeopen"].ToString().Trim();
            if(nodeOpen != "open" && nodeOpen != "closed")
            {
                nodeOpen = "open";
            }
            DateTime _installDateTime;
            DateTime? installDateTime;
            if(DateTime.TryParse(form["installdate"].ToString().Trim(), out _installDateTime) == false)
            {
                installDateTime = null;
            }
            else
            {
                installDateTime = _installDateTime;
            }
            int _eqClassID = 0;
            int? eqClassID = null;
            string eqTypeSpecification = null;
            string eqSupplier = null;
            string eqManufacturer = null;
            int eqIsVirtual = 1;
            try
            {
                if(int.TryParse(form["eqclassid"], out _eqClassID) == true && _eqClassID > 0)
                {
                    eqClassID = _eqClassID;
                }
                if(int.TryParse(form["eqisvirtual"], out eqIsVirtual) == false)
                {
                    eqIsVirtual = 1;
                }
                eqTypeSpecification = form["eqtypetpecification"].ToString().Trim();
                eqSupplier = form["eqsupplier"].ToString().Trim();
                eqManufacturer = form["eqmanufacturer"].ToString().Trim();
            }
            catch { }
            int eqSpareparts = 0, eqMajor = 0;
            if(form["eqspareparts"] != null)
            {
                int.TryParse(form["eqspareparts"].ToString(), out eqSpareparts);
            }
            if(form["eqmajor"] != null)
            {
                int.TryParse(form["eqmajor"].ToString(), out eqMajor);
            }
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            int opearteType = int.Parse(form["opeartetype"].ToString());
            return EquipMent.Edit(eqID, codeName, treeName, info, installDateTime, getIcon, orderby, nodeOpen, eqClassID, eqTypeSpecification, eqSupplier, eqManufacturer, loginUserCode, opearteType, eqIsVirtual, eqSpareparts, eqMajor);
        }

        public ActionResult _MoveEq(string eqid)
        {
            if(eqid != null)
            {
                int _eqid;
                int.TryParse(eqid, out _eqid);
                V_NGZB_EQ_BaseTree eQ_BaseTree = EquipMent.EqModel(_eqid);
                if(eQ_BaseTree != null)
                {
                    ViewBag.eqid = eQ_BaseTree.eqID;
                    ViewBag.eqname = eQ_BaseTree.eqName;
                }
                else
                {
                    ViewBag.eqid = "";
                    ViewBag.eqname = "";
                }
            }
            return View();
        }

        public int __MoveEq(FormCollection form)
        {
            if(string.IsNullOrWhiteSpace(form["targetId"]) || string.IsNullOrWhiteSpace(form["eqid"]) || string.IsNullOrWhiteSpace(form["moveinfo"]) || string.IsNullOrWhiteSpace(form["op"]))
            {
                return -1;
            }
            else
            {
                int eqID;
                int.TryParse(form["eqid"].ToString(), out eqID);
                int eqParentID;
                int.TryParse(form["targetId"].ToString(), out eqParentID);
                string moveWhy = form["moveinfo"].ToString().Trim();
                int opearterType;
                int.TryParse(form["op"].ToString().Trim(), out opearterType);
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                return EquipMent.EqMove(eqID, eqParentID, loginUserCode, opearterType, moveWhy);
            }
        }

        public int __OperateUpDown(FormCollection form)
        {
            if(string.IsNullOrEmpty(form["eqid"]) ||
                string.IsNullOrEmpty(form["deltype"]) ||
                string.IsNullOrEmpty(form["opwhy"]) ||
                string.IsNullOrEmpty(form["optype"]))
            {
                return -1;
            }
            int eqID = 0;
            int.TryParse(form["eqid"].ToString(), out eqID);
            string updownwhy = form["opwhy"].ToString().Trim();
            Models.Class.SessionHelp session = new Models.Class.SessionHelp();
            string loginUserCode = session.GetSessionUser();
            int opearteType = int.Parse(form["optype"].ToString());
            int delType = 0;
            int.TryParse(form["deltype"].ToString().Trim(), out delType);
            return EquipMent.OpearteDownUpDelLine(eqID, updownwhy, loginUserCode, opearteType, delType);
        }

        public string __EqClass()
        {
            return EquipMent.EqClass();
        }

        public ActionResult _UpDownEq(string eqid)
        {
            if(eqid != null)
            {
                int _eqid;
                int.TryParse(eqid, out _eqid);
                V_NGZB_EQ_BaseTree eQ_BaseTree = EquipMent.EqModel(_eqid);
                if(eQ_BaseTree != null)
                {
                    ViewBag.eqid = eQ_BaseTree.eqID;
                    ViewBag.eqname = eQ_BaseTree.eqName;
                }
                else
                {
                    ViewBag.eqid = "";
                    ViewBag.eqname = "";
                }
            }
            return View();
        }

        public string __GetEqComboTree()
        {
            return EquipMent.GetEqComboTree(null);
        }

        public ActionResult _EqInfo()
        {
            return View();
        }

        public string __GetEqInfo(int eqid)
        {
            return EquipMent.GetEqInfo(eqid);
        }

        public ActionResult _PassABC(int eqID)
        {
            V_NGZB_EQ_BaseTree eq = EquipMent.EqModel(eqID);
            ViewBag.eqID = eq.eqID;
            ViewBag.eqName = eq.eqName;
            return View();
        }

        public string __PropertyGridABC(string tokenkey)
        {
            return EquipMent.PropertyGridABC(tokenkey);
        }

        public string __ABCItem(int abcid)
        {
            return EquipMent.ABCItem(abcid);
        }

        public int __PassABC(string eqid, string abc, string opeartetype)
        {
            if(string.IsNullOrEmpty(eqid) || string.IsNullOrWhiteSpace(abc) || string.IsNullOrWhiteSpace(opeartetype))
            {
                return -1;
            }
            else
            {
                int eqID = 0;
                if(int.TryParse(eqid, out eqID) == false)
                {
                    return -1;
                }
                int opearteType = 8;
                if(int.TryParse(opeartetype, out opearteType) == false)
                {
                    opearteType = 8;
                }
                string[] abcItem = abc.Split(',');
                string[] abcID = new string[abcItem.Length];
                int _index = -1;
                string[] abcNum = new string[abcItem.Length];
                for(int i = 0; i < abcID.Length; i++)
                {
                    _index = abcItem[i].IndexOf(":");
                    abcID[i] = abcItem[i].Substring(0, _index);
                    abcNum[i] = abcItem[i].Split('|')[1];
                }
                Models.Class.SessionHelp session = new Models.Class.SessionHelp();
                string loginUserCode = session.GetSessionUser();
                return EquipMent.PassABC(eqID, abcID, abcNum, opearteType, loginUserCode);
            }
        }

        public string __OperatingRules(string eqParendID = null, string searchkey = null)
        {
            int _eqParendID = 0;
            if(eqParendID != null)
            {
                int.TryParse(eqParendID, out _eqParendID);
            }
            return EquipMent.OperatingRules(_eqParendID, searchkey);
        }

        public ActionResult _RuleInfo(string eqid)
        {
            return View();
        }

        public ActionResult _ManageRule(string optype, string eqid)
        {
            if(eqid == null || optype == null)
            {
                ViewBag.eqid = "";
                ViewBag.optype = "";
            }
            else
            {
                int _eqid = 0;
                if(int.TryParse(eqid, out _eqid) == false)
                {
                    ViewBag.eqid = "";
                }
                else
                {
                    ViewBag.eqid = _eqid.ToString();
                }
                switch(optype)
                {
                    case "1":
                        ViewBag.optype = "添加";
                        ViewBag.op = 1;
                        ViewBag.eqmodel = EquipMent.EqModel(_eqid);
                        break;

                    case "2":
                        ViewBag.optype = "修改";
                        ViewBag.op = 2;
                        ViewBag.model = EquipMent.OperateRuleModel(_eqid);
                        break;

                    default:
                        ViewBag.optype = "";
                        break;
                }
            }
            return View();
        }

        public static string __EqRuleTool()
        {
            return EquipMent.EqRuleTool();
        }

        [ValidateInput(false)]
        public int __SaveRule(NGZB_EQ_O_OperateRule model)
        {
            if(model != null)
            {
                SessionHelp session = new SessionHelp();
                model.createUserCode = session.GetSessionUser();
                return EquipMent.SaveRule(model); ;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult _AddDayCheck(string eqid)
        {
            ViewBag.eqid = eqid;
            ViewBag.checktitle = DbHelp.GetDbItem("NGZB_EQ_O_DayCheckStandTitle", "dayCheckTitle", "eqid=" + eqid, null);
            return View();
        }

        public ActionResult _AddRepair(string eqid)
        {
            ViewBag.eqid = eqid;
            ViewBag.repairtitle = DbHelp.GetDbItem("NGZB_EQ_O_RepairStandTitle", "repairTitle", "eqid=" + eqid, null);
            return View();
        }

        public ActionResult _AddDebug(string eqid)
        {
            ViewBag.eqid = eqid;
            return View();
        }

        public ActionResult _AddOil(string eqid)
        {
            ViewBag.eqid = eqid;
            return View();
        }

        public int __CheckAuthorityUserCode(string eqid, string authorityusercode)
        {
            if(eqid != null && authorityusercode != null)
            {
                int _eqid = 0;
                if(int.TryParse(eqid, out _eqid) == false)
                {
                    return 0;
                }
                return EquipMent.CheckAuthorityUserCode(_eqid, authorityusercode);
            }
            return 0;
        }

        public string __GetDayCheckMethod()
        {
            return EquipMent.GetDayCheckMethod();
        }

        public string __GetDayCheckState()
        {
            return EquipMent.GetDayCheckState();
        }

        public string __GetDayCheckType()
        {
            return EquipMent.GetDayCheckType();
        }

        public int __SaveDayCheck(int eqid, string checktitle, string daycheckpart, string daycheckstand, double daychecktime, int unit, string daycheckmethod, string daycheckinfo, string daycheckstate, int checktypeid, int daycheckid)
        {
            return EquipMent.SaveDayCheck(eqid, checktitle, daycheckpart, daycheckstand, daychecktime, unit
                , daycheckmethod, daycheckinfo, daycheckstate, checktypeid, daycheckid);
        }

        public string __DayCheckStand(int eqid)
        {
            return EquipMent.DayCheckStand(eqid);
        }

        public int __DayCheckStandDelRedo(int daycheckid, int dotype)
        {
            return EquipMent.DayCheckStandDelRedo(daycheckid, dotype);
        }

        public string __RepairStand(int eqid)
        {
            return EquipMent.RepairStand(eqid);
        }

        public int __SaveRepair(int repairid, int eqid, int repairtypeid, double repairtime, int unitid, string repairinfo, double worknum, int workUnitid, string acceptstand,string repairtitle)
        {
            return EquipMent.SaveRepair(repairid, eqid, repairtypeid, repairtime, unitid, repairinfo, worknum, workUnitid, acceptstand, repairtitle);
        }

        public int __RepairDelRedo(int repairID, int doType)
        {
            return EquipMent.RepairDelRedo(repairID, doType);
        }

        public string __GetRepairType()
        {
            return EquipMent.GetRepairType();
        }
    }
}