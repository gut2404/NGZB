using System.Data;
using System.Text;
using Newtonsoft.Json;
using NGZB.Models.Class;

namespace NGZB.Models
{
    /// <summary>
    /// 菜单管理DAL
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 菜单排序方式
        /// </summary>
        private static string orderby = "menuOrderBy ASC";

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menuParentID">父级菜单ID</param>
        /// <param name="modeID">模块ID</param>
        /// <param name="menuType">菜单类型，1为授权，0为公开</param>
        /// <param name="menuImg">菜单图标</param>
        /// <param name="colerCsscClass">菜单颜色类</param>
        /// <returns></returns>
        public static int Add(int menuParentID, int modeID, int menuType, string menuImg, string colerCsscClass)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_Menu(menuParentID, modeID, menuType, menuImg, colerCsscClass, ref rt);
            return (int)rt;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="userCode">用户名</param>
        /// <returns></returns>
        public static string Get(string userCode)
        {
            string menuName = "";
            string menuIco = "";
            string menuUrl = "";
            string colorCssClass = "c1";
            string menuTempSql = "SELECT [menuName],[menuUrl],[menuImg],[menuID],[colerCsscClass] FROM NGZB_Menu";
            StringBuilder rtSrt = new StringBuilder();
            DataTable firstdt = DbHelp.ExcuteTable(menuTempSql, "menuParentID=0", orderby);
            for (int i = 0; i < firstdt.Rows.Count; i++)
            {
                menuName = firstdt.Rows[i]["menuName"].ToString();
                menuIco = firstdt.Rows[i]["menuImg"].ToString();
                menuUrl = firstdt.Rows[i]["menuUrl"].ToString();
                colorCssClass = firstdt.Rows[i]["colerCsscClass"].ToString();
                string towWhere = string.Format("menuParentID={0} AND ((menuType=0) OR (menuID IN (SELECT  menuID FROM [V_NGZB_MenuAllow] WHERE userCode = '{1}')))", firstdt.Rows[i]["menuID"], userCode);
                DataTable towdt = DbHelp.ExcuteTable(menuTempSql, towWhere, orderby);
                if (towdt.Rows.Count > 0)
                {
                    string firstMenu = string.Format("<div title='{0}' class=\"onemenu {1}\" data-options=\"iconCls:'icon-{2}'\">", menuName, colorCssClass, menuIco);
                    rtSrt.Append(firstMenu);
                    for (int j = 0; j < towdt.Rows.Count; j++)
                    {
                        menuName = towdt.Rows[j]["menuName"].ToString();
                        menuIco = towdt.Rows[j]["menuImg"].ToString();
                        menuUrl = towdt.Rows[j]["menuUrl"].ToString();
                        colorCssClass = towdt.Rows[j]["colerCsscClass"].ToString();
                        string towMenu = string.Format("<a href=\"#\" onclick=\"addTab('{0}','{1}','{5}')\" class=\"easyui-linkbutton towmenu {2}\" data-options=\"iconCls:'icon-{3}'\">{4}</a>", menuName, menuUrl, colorCssClass, menuIco, menuName, menuIco);
                        rtSrt.Append(towMenu);
                    }
                    rtSrt.Append("</div>");
                }
            }
            return rtSrt.ToString();
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="parentID">父级菜单ID</param>
        /// <returns></returns>
        public static string GetParentMenu(int parentID)
        {
            string where = "menuParentID=" + parentID;
            string selectItem = "";
            if (parentID == 0)
            {
                selectItem = "SELECT menuID,menuName FROM [NGZB_Menu]";
            }
            else
            {
                selectItem = "SELECT [menuID],[menuName],[menuInfo],[menuImg],[menuOrderBy],[menuType],[colerCsscClass] FROM[NGZB_Menu]";
            }
            DataTable dt = DbHelp.ExcuteTable(selectItem, where, orderby);
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 根据上级菜单及角色查询查询二级菜单
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="pid">父菜单ID</param>
        /// <returns></returns>
        public static string RoleMenu(string roleid, string pid)
        {
            DataTable dt = null;
            if (roleid != "" && pid != "")
            {
                string where = string.Format("menuType=1 AND menuParentID={0} AND menuID NOT IN (SELECT menuID FROM NGZB_Role_Item WHERE roleID={1})", pid, roleid);
                dt = DbHelp.ExcuteTable("SELECT menuID,menuName FROM [NGZB_Menu]", where, orderby);
            }
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 添加菜单分组
        /// </summary>
        /// <param name="menuGroupName">分组名称</param>
        /// <param name="menuImg">图标</param>
        /// <returns></returns>
        public static int AddMenuGroup(string menuGroupName, string menuImg)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.I_NGZB_MenuGroup(menuGroupName, menuImg, ref rt);
            return (int)rt;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuID">菜单ID</param>
        /// <returns></returns>
        public static int Del(int menuID)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = 0;
            ctx.D_NGZB_Menu(menuID, ref rt);
            return (int)rt;
        }

        public static int Edit(int? menuid, int? menutype, int? modelid, string menuimg, int? menuorderby, string menucolor, int? parentid)
        {
            ctxDbDataContext ctx = new ctxDbDataContext();
            int? rt = null;
            ctx.U_NGZB_Menu(menuid, parentid, menuimg, menuorderby, menutype, modelid, menucolor, ref rt);
            return (int)rt;
        }
    }
}