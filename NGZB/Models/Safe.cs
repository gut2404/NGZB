using System.Data;
using System.Data.Linq;
using System.Linq;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    public class Safe
    {
        public static string GetSafeStand(string serchkey,int harmtypeid)
        {
            if (serchkey == null)
            {
                serchkey = "";
            }
            ctxSfDbDataContext sf = new ctxSfDbDataContext();
            ISingleResult<S_NGZB_SF_HarmResult> harm = sf.S_NGZB_SF_Harm(serchkey, harmtypeid);
            return JsonConvert.SerializeObject(harm);
        }

        public static string GetWorkType()
        {
            DataTable dt = DbHelp.ExcuteTable("SELECT [workTypeID] AS [id] ,[wordTypeName] AS [text] FROM   [NGZB_SF_WorkType]", "isView=1", "workorderby ASC");
            return JsonConvert.SerializeObject(dt);
        }

        public static int AddHarm(int workTypeID, string securityInfo, string defendAgainst, string loginUserCode)
        {
            ctxSfDbDataContext sf = new ctxSfDbDataContext();
            int? rt = null;
            sf.I_NGZB_SF_Harm(workTypeID, securityInfo, defendAgainst, loginUserCode, ref rt);
            return (int)rt;
        }

        public static V_NGZB_Harm Harm(int safeHarmID)
        {
            ctxSfDbDataContext sf = new ctxSfDbDataContext();
            V_NGZB_Harm harm = new V_NGZB_Harm();
            harm = (from c in sf.V_NGZB_Harm where c.safeHarmID == safeHarmID select c).SingleOrDefault();
            return harm;
        }

        public static int EditHarm(int safeHarmID, int workTypeID, string securityInfo, string defendAgainst, string loginUserCode)
        {
            ctxSfDbDataContext sf = new ctxSfDbDataContext();
            int? rt = 0;
            sf.U_NGZB_SF_Harm(safeHarmID, workTypeID, securityInfo, defendAgainst, loginUserCode, ref rt);
            return (int)rt;
        }
    }
}