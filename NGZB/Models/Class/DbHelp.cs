using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NGZB.Models.Class
{
    public class DbHelp
    {
        private readonly static string dbcnn = ConfigurationManager.ConnectionStrings["NGZB_dbConnectionString"].ConnectionString;

        #region 获取数据库指定定表中指定字段值

        /// <summary>
        /// 获取数据库指定表制定字段的值，只返回满足条件的第一条，一般用于有主键的查询
        /// </summary>
        /// <param name="tablename">要查询的表</param>
        /// <param name="dbitem">要查询的字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="ps">参数</param>
        /// <returns></returns>
        public static string GetDbItem(string tablename, string dbitem, string where, SqlParameter[] ps)
        {
            string sql = "SELECT TOP 1 " + dbitem + " FROM " + tablename + " WHERE " + where;
            if (string.IsNullOrEmpty(where))
            {
                sql = "SELECT TOP 1" + dbitem + " FROM " + tablename;
            }
            using (SqlConnection conn = new SqlConnection(dbcnn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                if (ps != null)
                {
                    command.Parameters.AddRange(ps);
                }
                object objResult = command.ExecuteScalar();
                string rt = "";
                if (objResult != null && !Convert.IsDBNull(objResult))
                {
                    rt = objResult.ToString();
                }
                else
                {
                    rt = "";
                }
                return rt;
            }
        }

        #endregion 获取数据库指定定表中指定字段值

        #region 执行查询语句，返回一个表（DataTable）

        /// <summary>
        /// 执行查询语句，返回一个表
        /// </summary>
        /// <param name="sql">完整的select [*] from tablename语句</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序规则</param>
        /// <returns>返回一张表</returns>
        public static DataTable ExcuteTable(string sql, string where, string orderBy)
        {
            if (!string.IsNullOrEmpty(where))
            {
                sql = sql + " WHERE " + where;
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql = sql + " ORDER BY " + orderBy;
            }
            SqlDataAdapter da = new SqlDataAdapter(sql, dbcnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable ExcuteTable(string[] selectItem, string tableName, string where, string orderBy)
        {
            if (selectItem.Length > 0)
            {
                string sql = string.Join(",", selectItem);
                sql = "SELECT " + sql + " FROM " + tableName;
                if (!string.IsNullOrEmpty(where))
                {
                    sql = sql + " WHERE " + where;
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    sql = sql + " ORDER BY " + orderBy;
                }
                SqlDataAdapter da = new SqlDataAdapter(sql, dbcnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            else
            {
                return null;
            }
        }

        #endregion 执行查询语句，返回一个表（DataTable）

        #region 执行增删改的方法返回受影响的行数

        /// <summary>
        /// 执行增删改的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ps">参数数组</param>
        /// <returns>返回影响的行数</returns>
        public static int ExcuteNoQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(dbcnn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                if (ps != null)
                {
                    command.Parameters.AddRange(ps);
                }
                return command.ExecuteNonQuery();
            }
        }

        #endregion 执行增删改的方法返回受影响的行数

        #region 执行存储过程

        /// <summary>
        /// 执行存储过程的方法
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="ps">参数数组</param>
        /// <returns></returns>
        public static int ExcuteProc(string procName, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(dbcnn))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
                if (ps != null)
                {
                    command.Parameters.AddRange(ps);
                }
                return command.ExecuteNonQuery();
            }
        }

        #endregion 执行存储过程

        #region 查询符合条件的行数

        /// <summary>
        /// 查询符合条件的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public static int SearchNum(string tableName, string where)
        {
            using (SqlConnection conn = new SqlConnection(dbcnn))
            {
                string SearchSQL = "";
                if (where != null)
                {
                    SearchSQL = "SELECT COUNT(1) FROM " + tableName + " WHERE " + where;
                }
                else
                {
                    SearchSQL = "SELECT COUNT(1) FROM";
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(SearchSQL, conn) { CommandType = CommandType.Text };
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
        }

        public static int SearchNum(string _dbconn, string tableName, string where)
        {
            using (SqlConnection conn = new SqlConnection(_dbconn))
            {
                string SearchSQL = "";
                if (where != null)
                {
                    SearchSQL = "SELECT COUNT(1) FROM " + tableName + " WHERE " + where;
                }
                else
                {
                    SearchSQL = "SELECT COUNT(1) FROM";
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(SearchSQL, conn) { CommandType = CommandType.Text };
                return int.Parse(cmd.ExecuteScalar().ToString());
            }

            #endregion 查询符合条件的行数
        }
    }
}