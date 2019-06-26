using System.Web.Routing;

namespace NGZB.Models.Class
{
    public class PublishMethod : RouteData
    {
        public static string GetModelID(RouteData route)
        {
            string controller = route.Values["controller"].ToString();
            string active = route.Values["action"].ToString();
            string where = "modelControllers='" + controller + "' AND modelAction='" + active + "'";
            string modelID = DbHelp.GetDbItem("NGZB_Model", "modelID", where, null);
            return modelID;
        }
    }
}