using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NGZB.Models.Class;
using System.Data;
using Newtonsoft.Json;
using System.Data.Linq;
using Dapper;
using System.Data.SqlClient;

namespace NGZB.Models
{
    public class Test
    {
        public static string treejson()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string sql = "SELECT documentID AS id,documentParentID AS pId,documentName AS [name],'icon06' AS iconSkin FROM  NGZB_F_Document";
            DataTable dt = DbHelp.ExcuteTable(sql, null, null);
            return JsonConvert.SerializeObject(dt);
        }
        public static string linqtosqlSelet()
        {
            int eqID = 1503;
            string RT = "";
            ctxEqDbDataContext eqCtx = new ctxEqDbDataContext();
            ISingleResult<S_NGZB_EQ_BaseTree_GetChildResult> s = eqCtx.S_NGZB_EQ_BaseTree_GetChild(eqID);
            foreach (S_NGZB_EQ_BaseTree_GetChildResult row in s)
            {
                RT = RT + row.eqID + "-" + row.eqName + ",";
            }
            return RT;
        }
    }
}