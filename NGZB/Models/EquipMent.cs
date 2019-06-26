using System;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class EquipMent
    {
        public static string GetEqTree()
        {
            string sql = "[V_NGZB_EQ_BaseTree]";
            string where = "";
            string orderby = "eqOrderBy ASC";
            string treeID = "eqID";
            string treeName = "eqName";
            string treeParentID = "eqParendID";
            string parentDefault = "0";
            string icon = "eqIcon";
            string isdel = "eqIsDel";
            string nodeOpen = "nodeOpen";
            string myattributes = null;
            EasyUiTreeJson jsontree = new EasyUiTreeJson();
            return jsontree.TreeJson(sql, where, orderby, treeID, treeName, treeParentID, parentDefault, icon, isdel, nodeOpen, myattributes);
        }

        public static string GetChildTree(int parentID, string keys)
        {
            string orderby = "eqOrderBy ASC";
            string where = string.Format("eqParendID={0}", parentID);
            if(keys != null)
            {
                where = string.Format("eqParendID={0} AND (eqName LIKE '%{1}%' OR eqInfo LIKE '%{2}%')", parentID, keys, keys);
            }
            DataTable dt = DbHelp.ExcuteTable("SELECT [eqID],[eqCode],[eqName],[eqABC],[eqInfo],CONVERT(varchar(10), [eqInstallDate] , 120) AS [eqInstallDate] ,[eqOrderBy],[eqIcon],[eqIsDel],[nodeOpen],[eqClassName],[eqTypeSpecification] FROM [V_NGZB_EQ_BaseTree]", where, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 返回对应id的设备信息
        /// </summary>
        /// <param name="eqid">设备id</param>
        /// <returns>对应id的设备对象</returns>
        public static V_NGZB_EQ_BaseTree EqModel(int eqid)
        {
            ctxEqDbDataContext ctx = new ctxEqDbDataContext();
            V_NGZB_EQ_BaseTree eq = new V_NGZB_EQ_BaseTree();
            eq = (from c in ctx.V_NGZB_EQ_BaseTree where c.eqID == eqid select c).SingleOrDefault();
            return eq;
        }

        public static int Add(int parentId, string treename, string codeName, int orderby, string getIcon, string info, DateTime? installDateTime, string nodeOpen, int? eqClassID, string eqTypeSpecification, string eqSupplier, string eqManufacturer, string opearteUser, int opearteType, int eqIsVirtual, int eqSpareparts, int eqMajor)
        {
            ctxEqDbDataContext ctxEq = new ctxEqDbDataContext();
            int? rt = 0;
            ctxEq.I_NGZB_EQ_BaseTree(parentId, codeName, treename, info, installDateTime, getIcon, orderby, nodeOpen, eqClassID, eqTypeSpecification, eqSupplier, eqManufacturer, eqIsVirtual, opearteUser, opearteType, eqSpareparts, eqMajor, ref rt);
            return (int)rt;
        }

        public static int Edit(int eqID, string eqCode, string eqName, string eqInfo, DateTime? eqInstallDate, string eqIcon, int eqOrderBy, string nodeOpen, int? eqClassID, string eqTypeSpecification, string eqSupplier, string eqManufacturer, string opearteUser, int opearteType, int eqIsVirtual, int eqSpareparts, int eqMajor)
        {
            ctxEqDbDataContext ctxEq = new ctxEqDbDataContext();
            int? rt = null;
            ctxEq.U_NGZB_EQ_BaseTree(eqID, eqCode, eqName, eqInfo, eqInstallDate, eqIcon, eqOrderBy, nodeOpen, eqClassID, eqTypeSpecification, eqSupplier, eqManufacturer, eqIsVirtual, opearteUser, opearteType, eqSpareparts, eqMajor, ref rt);
            return (int)rt;
        }

        public static int OpearteDownUpDelLine(int eqID, string whyinfo, string opearteUser, int opearterType, int delType)
        {
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            ISingleResult<S_NGZB_EQ_BaseTree_GetChildResult> childEqID;
            ISingleResult<S_NGZB_EQ_BaseTree_GetParentResult> parentEqID;
            int? rt = 0;
            int suc = 0;
            if(delType == 0)
            {
                parentEqID = eqCtx.S_NGZB_EQ_BaseTree_GetParent(eqID);
                foreach(S_NGZB_EQ_BaseTree_GetParentResult row in parentEqID)
                {
                    eqCtx.U_NGZB_EQ_BaseTree_OperateIsDel(row.eqID, delType, whyinfo, opearterType, opearteUser, ref rt);
                    if(rt == 1)
                    {
                        suc++;
                    }
                }
            }
            else
            {
                childEqID = eqCtx.S_NGZB_EQ_BaseTree_GetChild(eqID);
                foreach(S_NGZB_EQ_BaseTree_GetChildResult row in childEqID)
                {
                    eqCtx.U_NGZB_EQ_BaseTree_OperateIsDel(row.eqID, delType, whyinfo, opearterType, opearteUser, ref rt);
                    if(rt == 1)
                    {
                        suc++;
                    }
                }
            }
            return suc > 0 ? 1 : 0;
        }

        public static string EqClass()
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [eqClassID] AS id,[eqClassName]  AS text FROM[dbo].[NGZB_EQ_BaseTree_Class]", "isView=1", "orderBy ASC");
            return JsonConvert.SerializeObject(dt);
        }

        public static int EqMove(int eqID, int eqParentID, string opearteUser, int opearteType, string moveWhy)
        {
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            int? rt = null;
            eqCtx.U_NGZB_EQ_BaseTree_Move(eqID, eqParentID, moveWhy, opearteUser, opearteType, ref rt);
            return (int)rt;
        }

        public static string GetEqComboTree(string where)
        {
            EasyUiTreeJson easyuijson = new EasyUiTreeJson();
            string tableName = "NGZB_EQ_BaseTree";
            string orderby = "eqOrderBy ASC";
            string id = "eqID";
            string text = "eqName";
            string parentID = "eqParendID";
            string parentDefault = "0";
            string icon = "eqIcon";
            return easyuijson.ReturnJsonComboTree(tableName, null, orderby, id, text, parentID, parentDefault, icon);
        }

        public static string GetEqInfo(int eqid)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            V_NGZB_EQ_BaseTree eq = EqModel(eqid);
            DateTime dt;
            string installdate = "";
            if(DateTime.TryParse(eq.eqInstallDate.ToString(), out dt) == false)
            {
                installdate = "";
            }
            else
            {
                installdate = dt.ToShortDateString();
            }
            string _eqSpareparts = "否", _eqMajor = "否";
            if(eq.eqSpareparts == 1)
            {
                _eqSpareparts = "是";
            }
            if(eq.eqMajor == 1)
            {
                _eqMajor = "是";
            }
            sb.Append("{\"total\":13,\"rows\":[");
            sb.Append("{\"name\":\"设备ID\",\"value\":\"" + eq.eqID + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"节点类型\",\"value\":\"" + eq.eqIsVirtual + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"展开状态\",\"value\":\"" + eq.nodeOpentxt + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备编码\",\"value\":\"" + eq.eqCode + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备名称\",\"value\":\"" + eq.eqName + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"型号规格\",\"value\":\"" + eq.eqTypeSpecification + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备等级\",\"value\":\"" + eq.eqABC + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"专业类别\",\"value\":\"" + eq.eqClassName + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备排序\",\"value\":\"" + eq.eqOrderBy + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"投用日期\",\"value\":\"" + installdate + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备提供商\",\"value\":\"" + eq.eqSupplier + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备制造商\",\"value\":\"" + eq.eqManufacturer + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"是否需要备件\",\"value\":\"" + _eqSpareparts + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"关键事故件\",\"value\":\"" + _eqMajor + "\",\"group\":\"设备详情\"},");
            sb.Append("{\"name\":\"设备描述\",\"value\":\"" + eq.eqInfo + "\",\"group\":\"设备描述\"}]}");
            return sb.ToString();
        }

        public static string PropertyGridABC(string tokenKey)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [abcID] ,LTRIM(STR([abcID]))+':'+[abcName] AS [abcName] FROM[NGZB_EQ_ABC]", null, "abcID ASC");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if(dt.Rows.Count > 0)
            {
                sb.Append("{\"total\":" + dt.Rows.Count.ToString() + ",\"rows\":[");
                foreach(DataRow row in dt.Rows)
                {
                    sb.Append("{\"name\":\"" + row["abcName"].ToString() + "\",\"value\":\"\",\"group\":\"\",\"editor\":{\"type\":\"combobox\",\"options\":{\"editable\":false,");
                    sb.Append("\"url\":\"/Equipment/__ABCItem?tokenkey=" + tokenKey + "&abcid=" + row["abcID"].ToString() + "\",\"method\":\"get\",\"valueField\":\"id\",\"textField\":\"text\",\"panelHeight\":\"auto\"}}},");
                }
            }
            string str = sb.ToString();
            return str.Substring(0, str.Length - 1) + "]}";
        }

        public static string ABCItem(int abcitemid)
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [abcItemName] AS [text], [abcNum] AS[id] FROM[NGZB_EQ_ABCItem] ", "abcID=" + abcitemid + "", "abcNum DESC");
            return JsonConvert.SerializeObject(dt);
        }

        public static int PassABC(int eqID, string[] abcID, string[] abcNum, int opearteType, string operateUser)
        {
            ctxEqDbDataContext ctxEq = new ctxEqDbDataContext();
            int? rt = 0;
            int _abcID = 0;
            int _abcNum = 0;
            int suc = 0;
            for(int i = 0; i < abcID.Length; i++)
            {
                _abcID = int.Parse(abcID[i]);
                _abcNum = int.Parse(abcNum[i]);
                ctxEq.I_NGZB_EQ_PassABC(eqID, _abcID, _abcNum, opearteType, operateUser, ref rt);
                if(rt == 1)
                {
                    suc++;
                }
            }
            if(suc == abcID.Length)
            {
                ctxEq.I_NGZB_EQ_PassABC(eqID, null, null, opearteType, operateUser, ref rt);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string OperatingRules(int eqParendID, string searchkey)
        {
            string sql = "SELECT [eqID], [eqName], [operateName], [operateCode], [operateNumber],[isPass],[createUserCode],[userName],[authorityUserCode] FROM [V_NGZB_EQ_O_OperateRule]";
            string where = "eqParendID=" + eqParendID;
            if(searchkey != null)
            {
                where = where + string.Format(" AND (eqName LIKE '%{0}%' OR operateName LIKE '%{1}%' OR operateCode LIKE '%{2}%')", searchkey, searchkey, searchkey);
            }
            DataTable dt = DbHelp.ExcuteTable(sql, where, null);
            return JsonConvert.SerializeObject(dt);
        }

        public static string EqRuleTool()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<div id=\"eqruletool\">");
            string sql = "SELECT jsFunction,toolIcon,marginright FROM NGZB_EQ_O_RuleTool";
            DataTable dt = DbHelp.ExcuteTable(sql, "isUse=1", "orderby ASC");
            int rowCount = dt.Rows.Count;
            if(rowCount > 0)
            {
                for(int i = 0; i < rowCount; i++)
                {
                    sb.AppendFormat("<a href=\"javascript:; \" style=\"margin-right:{0}px\" class=\"icon-{1}\" onclick=\"{2}();\"></a>", dt.Rows[i]["marginright"], dt.Rows[i]["toolIcon"], dt.Rows[i]["jsFunction"]);
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public static NGZB_EQ_O_OperateRule OperateRuleModel(int eqid)
        {
            ctxEqDbDataContext ctx = new ctxEqDbDataContext();
            NGZB_EQ_O_OperateRule eq = new NGZB_EQ_O_OperateRule();
            eq = (from c in ctx.NGZB_EQ_O_OperateRule where c.eqID == eqid select c).SingleOrDefault();
            return eq;
        }

        public static int SaveRule(NGZB_EQ_O_OperateRule model)
        {
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            int? rt = null;
            eqCtx.C_NGZB_EQ_O_OperateRule(model.eqID, model.companTitle, model.operateName, model.operateCode, model.operateNumber, model.operateText, model.createUserCode, ref rt);
            return (int)rt;
        }

        public static int CheckAuthorityUserCode(int eqid, string authorityusercode)
        {
            string operateIsNullWhere = string.Format("eqID={0}", eqid);
            if(DbHelp.SearchNum("NGZB_EQ_O_OperateRule", operateIsNullWhere) > 0)
            {
                return 0;
            }
            string where = string.Format("eqID={0} AND userCode='{1}'", eqid, authorityusercode);
            return DbHelp.SearchNum("NGZB_EQ_EqAuthority", where);
        }

        public static string GetDayCheckMethod()
        {
            string sql = "SELECT [dayCheckMothedID] AS id,[dayCheckMothedName] AS [text] FROM [NGZB_EQ_O_DayCheckMothed]";
            DataTable dataTable = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dataTable);
        }

        public static string GetDayCheckState()
        {
            string sql = "SELECT [dayCheckStateID] AS id,[dayCheckStateName] AS [text] FROM [NGZB_EQ_O_DayCheckState]";
            DataTable dataTable = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dataTable);
        }

        public static string GetDayCheckType()
        {
            string sql = "SELECT checkTypeId AS id,checkTypeName AS [text] FROM NGZB_EQ_O_DayCheckType";
            DataTable dataTable = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dataTable);
        }

        public static int SaveDayCheck(int eqid, string checktitle, string daycheckpart, string daycheckstand, double daychecktime, int unit, string daycheckmethod, string daycheckinfo, string daycheckstate, int checktypeid, int daycheckid)
        {
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            int? rt = null;
            eqCtx.C_NGZB_EQ_O_DayCheckStand(eqid, checktitle, daycheckpart, daycheckinfo, checktypeid, daychecktime, unit, daycheckstate, daycheckmethod, daycheckstand, daycheckid, ref rt);
            return (int)rt;
        }

        public static string DayCheckStand(int eqid)
        {
            string sql = "SELECT ROW_NUMBER() OVER (ORDER BY checkTypeId ASC) AS [id],[unitName] ,[checkTypeName] ,[dayCheckID],[dayCheckStand] ,[checkParts] ,[dayCheckInfo] ,[checkTypeId] ,[dayCheckTimeNum] ,[timeNumUnitID] ,[dayCheckState] ,[dayCheckMothed],[isView] FROM [V_NGZB_EQ_O_DayCheckStand]";
            return JsonConvert.SerializeObject(DbHelp.ExcuteTable(sql, "eqid=" + eqid, null));
        }

        public static int DayCheckStandDelRedo(int daycheckid, int dotype)
        {
            string upsql = string.Format("UPDATE NGZB_EQ_O_DayCheckStand SET isView={0} WHERE dayCheckID={1}", dotype, daycheckid);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static string RepairStand(int eqid)
        {
            string sql = "SELECT ROW_NUMBER() OVER (ORDER BY repairTime ASC) AS id, [repairID], [eqID], [repairTypeID], [repairNum], [unitID], [repairInfo], [acceptStand], [repairTime], [timeUnitID], [unitName], [repairUnitName],[repairTypeName],[isView] FROM [V_NGZB_EQ_O_RepairStand]";
            return JsonConvert.SerializeObject(DbHelp.ExcuteTable(sql, "eqid=" + eqid, null));
        }

        public static int RepairDelRedo(int repairID, int doType)
        {
            string upsql = string.Format("UPDATE NGZB_EQ_O_RepairStand SET isView={0} WHERE repairID={1}", doType, repairID);
            return DbHelp.ExcuteNoQuery(upsql, null);
        }

        public static string GetRepairType()
        {
            string sql = "SELECT [repairTypeID] AS id,[repairTypeName] AS [text] FROM [NGZB_EQ_O_RepairType]";
            DataTable dataTable = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dataTable);
        }

        public static int SaveRepair(int repairid, int eqid, int repairtypeid, double repairtime, int unitid, string repairinfo, double worknum, int workUnitid, string acceptstand,string repairtitle)
        {
            int? rt = 0;
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            eqCtx.C_NGZB_EQ_O_RepairStand(repairid, eqid, repairtypeid, repairtime, unitid, repairinfo, acceptstand, worknum, workUnitid, repairtitle, ref rt);
            return (int)rt;
        }
    }
}