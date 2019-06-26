using System.Data;
using System.Text;

namespace NGZB.Models.Class
{
    public class EasyUiTreeJson
    {
        private StringBuilder result = new StringBuilder();
        private StringBuilder sb = new StringBuilder();

        #region 返回有效组织机构树数据

        /// <summary>
        /// 组织机构树生成递归函数
        /// </summary>
        /// <param name="tabel">数据DataTable</param>
        /// <param name="idCol">树节点ID字段</param>
        /// <param name="txtCol">树节点名称字段</param>
        /// <param name="rela">父级ID字段</param>
        /// <param name="pId">根节点默认值</param>
        private void GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            string icon = "groupIcon";
            result.Append(sb);
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}={1}", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                int rowlength = 0;
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        rowlength = tabel.Select(string.Format("{0}={1}", rela, row[idCol])).Length;
                        if (string.IsNullOrEmpty(row[icon].ToString()))
                        {
                            if (rowlength > 0)
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\"");
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                            }
                        }
                        else
                        {
                            if (rowlength > 0)
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"closed\"");
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"open\"");
                            }
                        }
                        if (tabel.Select(string.Format("{0}={1}", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
                            result.Append(sb);
                            sb.Clear();
                        }
                        result.Append(sb);
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb);
                sb.Clear();
            }
        }

        /// <summary>
        /// 返回有效组织机构树数据
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="treeID">树主键字段名</param>
        /// <param name="treeName">节点名称字段</param>
        /// <param name="treeParentID">父级字段名</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <returns>返回json字符串</returns>
        public string ReturnJson(string sql, string where, string orderby, string treeID, string treeName, string treeParentID, string parentDefault)
        {
            DataTable datatable = DbHelp.ExcuteTable(sql, where, orderby);
            GetTreeJsonByTable(datatable, treeID, treeName, treeParentID, parentDefault);
            return result.ToString();
        }

        #endregion 返回有效组织机构树数据

        #region 返回所有组织机构树数据，该方法仅用于组织机构管理页面

        private void GetTreeJsonByTableAll(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            string icon = "groupIcon";
            result.Append(sb);
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}={1}", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                int rowLength = 0;
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        rowLength = tabel.Select(string.Format("{0}={1}", rela, row[idCol])).Length;
                        if (string.IsNullOrEmpty(row[icon].ToString()))
                        {
                            if (row["groupIsDel"].ToString() == "1")
                            {
                                if (rowLength > 0)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-no\",\"state\":\"closed\",\"attributes\":{\"isdel\":1}");
                                }
                                else
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-no\",\"state\":\"open\",\"attributes\":{\"isdel\":1}");
                                }
                            }
                            else
                            {
                                if (rowLength > 0)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\",\"attributes\":{\"isdel\":0}");
                                }
                                else
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\",\"attributes\":{\"isdel\":0}");
                                }
                            }
                        }
                        else
                        {
                            if (row["groupIsDel"].ToString() == "1")
                            {
                                if (rowLength > 0)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-no\",\"state\":\"closed\",\"attributes\":{\"isdel\":1}");
                                }
                                else
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-no\",\"state\":\"open\",\"attributes\":{\"isdel\":1}");
                                }
                            }
                            else
                            {
                                if (rowLength > 0)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row
                                    [icon] + "\",\"state\":\"closed\",\"attributes\":{\"isdel\":0}");
                                }
                                else
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row
                                    [icon] + "\",\"attributes\":{\"isdel\":0}");
                                }
                            }
                        }
                        if (rowLength > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTableAll(tabel, idCol, txtCol, rela, row[idCol]);
                            result.Append(sb);
                            sb.Clear();
                        }
                        result.Append(sb);
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb);
                sb.Clear();
            }
        }

        /// <summary>
        /// 返回所有组织机构树数据，该方法仅用于组织机构管理页面
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="treeID">树主键字段名</param>
        /// <param name="treeName">节点名称字段</param>
        /// <param name="treeParentID">父级字段名</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <returns>返回json字符串</returns>
        public string ReturnJsonAll(string sql, string where, string orderby, string treeID, string treeName, string treeParentID, string parentDefault)
        {
            DataTable datatable = DbHelp.ExcuteTable(sql, where, orderby);
            GetTreeJsonByTableAll(datatable, treeID, treeName, treeParentID, parentDefault);
            return result.ToString();
        }

        #endregion 返回所有组织机构树数据，该方法仅用于组织机构管理页面

        #region 文件目录树生成递归函数

        /// <summary>
        /// 文件目录树生成递归函数
        /// </summary>
        /// <param name="tabel">数据DataTable</param>
        /// <param name="idCol">树节点ID字段</param>
        /// <param name="txtCol">树节点名称字段</param>
        /// <param name="rela">父级ID字段</param>
        /// <param name="pId">根点默认值</param>
        /// <param name="userCode">目录管理员用户名</param>
        private void GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string userCode)
        {
            result.Append(sb);
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}={1}", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                int rowlength = 0;
                if (rows.Length > 0)
                {
                    string pass = "0|0|0";
                    int documentid = 0;
                    foreach (DataRow row in rows)
                    {
                        rowlength = tabel.Select(string.Format("{0}={1}", rela, row[idCol])).Length;
                        int.TryParse(row[idCol].ToString(), out documentid);
                        pass = FileSystem.DocumentPass(documentid, userCode);
                        if (rowlength > 0)
                        {
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\",\"attributes\":{\"pass\":\"" + pass + "\"}");
                        }
                        else
                        {
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\",\"attributes\":{\"pass\":\"" + pass + "\"}");
                        }
                        if (rowlength > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], userCode);
                            result.Append(sb);
                            sb.Clear();
                        }
                        result.Append(sb);
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb);
                sb.Clear();
            }
        }

        /// <summary>
        /// 返回文件目录权限树Json数据
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="treeID">树主键字段名</param>
        /// <param name="treeName">节点名称字段</param>
        /// <param name="treeParentID">父级字段名</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <param name="userCode">用户名</param>
        /// <returns>返回json字符串</returns>
        public string ReturnJson(string sql, string where, string orderby, string treeID, string treeName, string treeParentID, string parentDefault, string userCode)
        {
            DataTable datatable = DbHelp.ExcuteTable(sql, where, orderby);
            GetTreeJsonByTable(datatable, treeID, treeName, treeParentID, parentDefault, userCode);
            return result.ToString();
        }

        #endregion 文件目录树生成递归函数

        #region 返回通用树结构数据

        /// <summary>
        /// 通用树json数据
        /// </summary>
        /// <param name="tableName">要生成树的表</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="treeID">树主键字段名</param>
        /// <param name="treeName">节点名称字段</param>
        /// <param name="treeParentID">父级字段名</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <param name="icon">图标字段</param>
        /// <param name="isDel">是否删除字段</param>
        /// <param name="nodeOpen">节点打开状态控制字段</param>
        /// <param name="myattributes">附加一个自定义属性</param>
        /// <returns>返回json字符串</returns>
        public string TreeJson(string tableName, string where, string orderBy, string treeID, string treeName, string treeParentID, string parentDefault, string icon, string isDel, string nodeOpen, string myattributes)
        {
            string _sql;
            if (myattributes == null)
            {
                _sql = string.Format("SELECT {0},{1},{2},{3},{4},{5} FROM {6}", treeID, treeName, treeParentID, icon, isDel, nodeOpen, tableName);
            }
            else
            {
                _sql = string.Format("SELECT {0},{1},{2},{3},{4},{5},{7} FROM {6}", treeID, treeName, treeParentID, icon, isDel, nodeOpen, tableName, myattributes);
            }
            DataTable datatable = DbHelp.ExcuteTable(_sql, where, orderBy);
            _TreeJson(datatable, treeID, treeName, treeParentID, parentDefault, icon, isDel, nodeOpen, myattributes);
            return result.ToString();
        }

        private void _TreeJson(DataTable tabel, string idCol, string txtCol, string rela, object pId, string icon, string isdel, string nodeOpen, string myattributes)
        {
            result.Append(sb);
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}={1}", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                int rowlength = 0;
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        rowlength = tabel.Select(string.Format("{0}={1}", rela, row[idCol])).Length;
                        if (string.IsNullOrEmpty(row[icon].ToString().Trim()))
                        {
                            if (rowlength > 0)
                            {
                                if (nodeOpen == null)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                                }
                                else
                                {
                                    string _nodeOpen = row[nodeOpen].ToString().Trim();
                                    if (_nodeOpen != "open" && _nodeOpen != "closed")
                                    {
                                        _nodeOpen = "open";
                                    }
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"" + _nodeOpen + "\"");
                                }
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                            }
                        }
                        else
                        {
                            if (rowlength > 0)
                            {
                                if (nodeOpen == null)
                                {
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"open\"");
                                }
                                else
                                {
                                    string _nodeOpen = row[nodeOpen].ToString().Trim();
                                    if (_nodeOpen != "open" && _nodeOpen != "closed")
                                    {
                                        _nodeOpen = "open";
                                    }
                                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"" + _nodeOpen + "\"");
                                }
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"open\"");
                            }
                        }
                        string isdelvalue = "0";
                        if (!string.IsNullOrEmpty(row[isdel].ToString()))
                        {
                            isdelvalue = row[isdel].ToString();
                        }
                        string _myattributes = "";
                        if (myattributes != null)
                        {
                            _myattributes = row[myattributes].ToString();
                        }
                        sb.Append(",\"attributes\":{\"isdel\":\"" + isdelvalue + "\",\"myattributes\":\"" + _myattributes + "\"}");
                        if (rowlength > 0)
                        {
                            sb.Append(",\"children\":");
                            _TreeJson(tabel, idCol, txtCol, rela, row[idCol], icon, isdel, nodeOpen, myattributes);
                            result.Append(sb);
                            sb.Clear();
                        }
                        result.Append(sb);
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb);
                sb.Clear();
            }
        }

        #endregion 返回通用树结构数据

        #region 返回通用ComboTree的json数据

        /// <summary>
        /// 递归返回通用ComboTree的json数据
        /// </summary>
        /// <param name="tabel">数据DataTable</param>
        /// <param name="id">ComboTreeID字段</param>
        /// <param name="txt">ComboTree名称字段</param>
        /// <param name="parentID">父级ID字段</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <param name="icon">图标</param>
        private void GetComboTreeByTable(DataTable tabel, string id, string txt, string parentID, object parentDefault, string icon)
        {
            result.Append(sb);
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}={1}", parentID, parentDefault);
                DataRow[] rows = tabel.Select(filer);
                int rowlength = 0;
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        rowlength = tabel.Select(string.Format("{0}={1}", parentID, row[id])).Length;
                        if (string.IsNullOrEmpty(row[icon].ToString()))
                        {
                            if (rowlength > 0)
                            {
                                sb.Append("{\"id\":\"" + row[id] + "\",\"text\":\"" + row[txt] + "\",\"state\":\"closed\"");
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[id] + "\",\"text\":\"" + row[txt] + "\",\"state\":\"open\"");
                            }
                        }
                        else
                        {
                            if (rowlength > 0)
                            {
                                sb.Append("{\"id\":\"" + row[id] + "\",\"text\":\"" + row[txt] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"closed\"");
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + row[id] + "\",\"text\":\"" + row[txt] + "\",\"iconCls\":\"icon-" + row[icon] + "\",\"state\":\"open\"");
                            }
                        }
                        if (tabel.Select(string.Format("{0}={1}", parentID, row[id])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetComboTreeByTable(tabel, id, txt, parentID, row[id], icon);
                            result.Append(sb);
                            sb.Clear();
                        }
                        result.Append(sb);
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb);
                sb.Clear();
            }
        }

        /// <summary>
        /// 返回通用ComboTree的json数据
        /// </summary>
        /// <param name="tableName">查询sql语句</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="id">树主键字段名</param>
        /// <param name="text">节点名称字段</param>
        /// <param name="parentID">父级字段名</param>
        /// <param name="parentDefault">根节点默认值</param>
        /// <param name="icon">图标字段名称</param>
        /// <returns>返回json字符串</returns>
        public string ReturnJsonComboTree(string tableName, string where, string orderby, string id, string text, string parentID, object parentDefault, string icon)
        {
            string _sql = "SELECT " + id + "," + text + "," + parentID + "," + icon + " FROM " + tableName;
            DataTable datatable = DbHelp.ExcuteTable(_sql, where, orderby);
            GetComboTreeByTable(datatable, id, text, parentID, parentDefault, icon);
            return result.ToString();
        }

        #endregion 返回通用ComboTree的json数据
    }
}