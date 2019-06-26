using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace NGZB.Models.Class
{
    public class NpoiHelp : System.Web.HttpResponseBase
    {
        private IWorkbook _workbook;
        private string _filePath;

        private List<string> SheetNames { get; set; }

        public NpoiHelp(string filePath)
        {
            LoadFile(filePath);
        }

        #region Excel信息

        /// <summary>
        /// 获取Excel信息
        /// </summary>
        /// <param name="filePath"></param>
        private List<string> LoadFile(string filePath)
        {
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            _filePath = filePath;
            SheetNames = new List<string>();
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _workbook = WorkbookFactory.Create(fs);
            }

            stopwatch.Stop();
            return GetSheetNames();
        }

        /// <summary>
        /// 获取SHeet名称
        /// </summary>
        /// <returns></returns>
        private List<string> GetSheetNames()
        {
            var count = _workbook.NumberOfSheets;
            for (int i = 0; i < count; i++)
            {
                SheetNames.Add(_workbook.GetSheetName(i));
            }
            return SheetNames;
        }

        #endregion Excel信息

        #region 获取数据源

        /// <summary>
        /// 获取所有数据，所有sheet的数据转化为datatable。
        /// </summary>
        /// <param name="isFirstRowCoumn">是否将第一行作为列标题</param>
        /// <returns></returns>
        public DataSet GetAllTables(bool isFirstRowCoumn)
        {
            var stopTime = new System.Diagnostics.Stopwatch();
            stopTime.Start();
            var ds = new DataSet();

            foreach (var sheetName in SheetNames)
            {
                ds.Tables.Add(ExcelToDataTable(sheetName, isFirstRowCoumn));
            }
            stopTime.Stop();
            return ds;
        }

        /// <summary>
        /// 获取第<paramref name="idx"/>的sheet的数据
        /// </summary>
        /// <param name="idx">Excel文件的第几个sheet表</param>
        /// <param name="isFirstRowCoumn">是否将第一行作为列标题</param>
        /// <returns></returns>
        public DataTable GetTable(int idx, bool isFirstRowCoumn)
        {
            if (idx >= SheetNames.Count || idx < 0)
                throw new Exception("找不到指定的工作薄......");
            return ExcelToDataTable(SheetNames[idx], isFirstRowCoumn);
        }

        /// <summary>
        /// 获取sheet名称为<paramref name="sheetName"/>的数据
        /// </summary>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="isFirstRowColumn">是否将第一行作为列标题</param>
        /// <returns></returns>
        public DataTable GetTable(string sheetName, bool isFirstRowColumn)
        {
            return ExcelToDataTable(sheetName, isFirstRowColumn);
        }

        #endregion 获取数据源

        #region 方法

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        private DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            var data = new DataTable { TableName = sheetName };
            try
            {
                var sheet = sheetName != null ? _workbook.GetSheet(sheetName) : _workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var firstRow = sheet.GetRow(0);
                    if (firstRow == null)
                        return data;
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    int startRow = isFirstRowColumn ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        var column = new DataColumn(Convert.ToChar('A' + i).ToString());
                        if (isFirstRowColumn)
                        {
                            var columnName = firstRow.GetCell(i).StringCellValue;
                            column = new DataColumn(columnName);
                        }
                        data.Columns.Add(column);
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                else throw new Exception("找不到指定的工作薄......");

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        #endregion 方法

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        /// <param name="sheetName">工作薄名称</param>
        public static void DataTableToFile(DataTable dtSource, string strHeaderText, string strFileName, string sheetName)
        {
            using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = DataTableToExcel(dtSource, strHeaderText, sheetName);
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dt">源DataTable</param>
        /// <param name="sheetTitle">表头文本</param>
        /// <param name="sheetName">工作薄名称</param>
        public static byte[] DataTableToExcel(DataTable dt, string sheetTitle, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetName);

            #region 右击文件 属性信息

            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = Sys.GetInfo("company");//填加xls文件作者信息
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "由开拓企业系统数据导出创建"; //填加xls文件作者信息
                si.ApplicationName = "由开拓企业系统创建"; //填加xls文件创建程序信息
                si.LastAuthor = "由开拓企业系统数据导出创建"; //填加xls文件最后保存者信息
                si.Comments = "QQ:34760393"; //填加xls文件备注信息
                si.Title = "系统定制请联系:15896348711"; //填加xls文件标题信息
                si.Subject = "Email:34760393@qq.com";//填加文件主题信息
                si.RevNumber = "0";
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
                si.RemoveApplicationName();
            }

            #endregion 右击文件 属性信息

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            //取得列宽
            int[] arrColWidth = new int[dt.Columns.Count];
            foreach (DataColumn item in dt.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName).Length;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dt.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 表头及样式

                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(sheetTitle);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        headStyle.WrapText = false;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                    }

                    #endregion 表头及样式

                    #region 列头及样式

                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(1);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dt.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }

                    #endregion 列头及样式

                    rowIndex = 2;
                }

                #endregion 新建表，填充表头，填充列头，样式

                #region 填充内容

                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;

                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;

                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;

                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;

                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;

                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion 填充内容

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms.ToArray();
            }
        }
    }
}