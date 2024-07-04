using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static EES.Infrastructure.Jwt.ResponseAuthenticationHandler;

namespace EES.Infrastructure.Tools
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 读取excel指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">当文件为NULL时触发该异常</exception>
        /// <exception cref="Exception">当文件类型不受支持时触发该异常</exception>
        public static DataTable ReadExcelRectangularArea(IFormFile? file, int sheetIndex, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileName = file.FileName.ToLower(System.Globalization.CultureInfo.CurrentCulture).Trim();

            if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
            {
                throw new Exception("不支持该文件类型");
            }

            using var fileStream = file.OpenReadStream();

            return ReadExcelRectangularArea(fileStream, sheetIndex, startRowIndex, endRowIndex, startColIndex, endColIndex, includeHeader);
        }

        /// <summary>
        /// 读取excel指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="dataTable">接收数据的DataTable</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        /// <exception cref="ArgumentNullException">当文件为NULL时触发该异常</exception>
        /// <exception cref="Exception">当文件类型不受支持时触发该异常</exception>
        public static void ReadExcelRectangularArea(IFormFile? file, ref DataTable dataTable, int sheetIndex, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileName = file.FileName.ToLower(System.Globalization.CultureInfo.CurrentCulture).Trim();

            if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
            {
                throw new Exception("不支持该文件类型");
            }

            using var fileStream = file.OpenReadStream();

            ReadExcelRectangularArea(fileStream, ref dataTable, sheetIndex, startRowIndex, endRowIndex, startColIndex, endColIndex, includeHeader);
        }



        /// <summary>
        /// 将excel数据读取到datatable
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="headerRowIndex">表头位置</param>
        /// <param name="dataStartRowIndex">数据起始位置</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">当文件为NULL时触发该异常</exception>
        /// <exception cref="Exception">当文件类型不受支持时触发该异常</exception>
        public static DataTable ReadExcelToDataTable(IFormFile? file, int sheetIndex = 0, int headerRowIndex = 0, int dataStartRowIndex = 1)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileName = file.FileName.ToLower(System.Globalization.CultureInfo.CurrentCulture).Trim();

            if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
            {
                throw new Exception("不支持该文件类型");
            }

            using var fileStream = file.OpenReadStream();

            return ReadExcelToDataTable(fileStream, sheetIndex, headerRowIndex, dataStartRowIndex);
        }

        /// <summary>
        /// 将excel数据读取到datatable
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="dataTable">数据表</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="dataStartRowIndex">数据起始位置</param>
        /// <exception cref="ArgumentNullException">当文件为NULL时触发该异常</exception>
        /// <exception cref="Exception">当文件类型不受支持时触发该异常</exception>
        public static void ReadExcelToDataTable(IFormFile? file, ref DataTable dataTable, int sheetIndex = 0, int dataStartRowIndex = 1)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var fileName = file.FileName.ToLower(System.Globalization.CultureInfo.CurrentCulture).Trim();

            if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
            {
                throw new Exception("不支持该文件类型");
            }

            using var fileStream = file.OpenReadStream();

            ReadExcelToDataTable(fileStream, ref dataTable, sheetIndex, dataStartRowIndex);
        }



        /// <summary>
        /// 将excel数据读取到datatable
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="headerRowIndex">表头位置</param>
        /// <param name="dataStartRowIndex">数据起始位置</param>
        /// <remarks>DataTable默认所有字段都是string类型，需要调用者二次处理</remarks>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(Stream fileStream, int sheetIndex = 0, int headerRowIndex = 0, int dataStartRowIndex = 1)
        {
            DataTable dataTable = new();
            using (var workbook = WorkbookFactory.Create(fileStream))
            {
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet != null)
                {
                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    if (headerRow != null)
                    {
                        // 创建列并添加到DataTable
                        for (int i = 0; i < headerRow.LastCellNum; i++)
                        {
                            ICell headerCell = headerRow.GetCell(i);
                            if (headerCell != null)
                            {
                                string columnName = headerCell.ToString()!;
                                if (columnName.StartsWith("*"))
                                {
                                    columnName = columnName.TrimStart('*');
                                }
                                if (!string.IsNullOrWhiteSpace(columnName))
                                {
                                    dataTable.Columns.Add(columnName, typeof(string)); // 可以根据需要指定数据类型
                                }
                            }
                        }

                        // 从数据起始行读取数据并添加到DataTable
                        for (int rowIndex = dataStartRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow dataRow = sheet.GetRow(rowIndex);
                            if (dataRow != null)
                            {
                                DataRow newRow = dataTable.NewRow();
                                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                                {
                                    ICell dataCell = dataRow.GetCell(columnIndex);
                                    if (dataCell != null)
                                    {

                                        newRow[columnIndex] = dataCell.ToString(); // 可以根据需要处理数据类型转换
                                    }
                                }
                                dataTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }

            return dataTable;
        }


        /// <summary>
        /// 读取excel数据填充到DataTable中
        /// </summary>
        /// <param name="fileStream">excel的文件流</param>
        /// <param name="dataTable">datatable</param>
        /// <param name="sheetIndex">sheet表索引</param>
        /// <param name="dataStartRowIndex">数据行的索引</param>
        /// <remarks>dataTable 需要指定Column的DataType属性，否则会默认将其设置为string</remarks>
        /// <example>
        ///  DataTableType dataTable = new();
        ///  dataTable.Columns.Add("column1", typeof(int));
        /// </example>
        public static void ReadExcelToDataTable(Stream fileStream, ref DataTable dataTable, int sheetIndex = 0, int dataStartRowIndex = 1)
        {
            using var workbook = WorkbookFactory.Create(fileStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            if (sheet != null)
            {
                // 从数据起始行读取数据并添加到DataTable
                for (int rowIndex = dataStartRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow dataRow = sheet.GetRow(rowIndex);
                    if (dataRow != null)
                    {
                        DataRow newRow = dataTable.NewRow();
                        for (int columnIndex = 0; columnIndex < dataRow.LastCellNum; columnIndex++)
                        {
                            ICell dataCell = dataRow.GetCell(columnIndex);
                            if (dataCell != null)
                            {
                                //SetColumnValue(newRow, columnIndex, column.DataType, dataCell.ToString());// 可以根据需要处理数据类型转换
                                newRow[columnIndex] = dataCell.ToString(); 
                            }
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }
            }

        }


        /// <summary>
        /// Excel数据导出
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="templateFullPath">模板位置(绝对路径)</param>
        /// <param name="data">数据</param>
        /// <param name="sheetIndex">sheet表索引</param>
        /// <param name="headerRowIndex">表头所在行索引</param>
        /// <param name="startDataRowIndex">数据从哪一行开始写</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">当指定路径未找到模板文件时抛出该异常</exception>
        public static byte[] ExportExcel<T>(string templateFullPath, List<T> data, int sheetIndex = 0, int headerRowIndex = 0, int startDataRowIndex = 1)
        {
            if (!File.Exists(templateFullPath))
            {
                throw new FileNotFoundException("指定位置未找到模板");
            }

            using FileStream templateStream = new(templateFullPath, FileMode.Open, FileAccess.Read);

            // 使用FillExcelTemplate方法生成Excel文件
            MemoryStream excelStream = FillExcelTemplate(templateStream, data, sheetIndex, headerRowIndex, startDataRowIndex);

            return excelStream.ToArray();

        }



        /// <summary>
        /// 将数据写入excel流
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="templateStream">excel的流</param>
        /// <param name="data">数据</param>
        /// <param name="sheetIndex">sheet表索引</param>
        /// <param name="headerRowIndex">表头所在行索引</param>
        /// <param name="startDataRowIndex">数据从哪一行开始写</param>
        /// <returns></returns>
        public static MemoryStream FillExcelTemplate<T>(Stream templateStream, List<T> data, int sheetIndex = 0, int headerRowIndex = 0, int startDataRowIndex = 1)
        {
            // 创建一个内存流，用于保存生成的Excel文件
            MemoryStream outputMemoryStream = new();

            using (var workbook = WorkbookFactory.Create(templateStream))
            {
                ISheet sheet = workbook.GetSheetAt(sheetIndex); // 要填充第工作表

                PropertyInfo[] properties = typeof(T).GetProperties();

                IRow headerRow = sheet.GetRow(headerRowIndex); // 模板的表头

                // 创建一个字典，将表头的文本与属性名关联
                Dictionary<string, PropertyInfo> headerToPropertyMap = new();
                foreach (var property in properties)
                {
                    string columnName = property.Name; // 默认使用属性名
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>(); // 如果属性有自定义列名属性，则使用自定义列名
                    if (columnAttribute != null && !string.IsNullOrWhiteSpace(columnAttribute.Name))
                    {
                        columnName = columnAttribute.Name;
                    }
                    headerToPropertyMap[columnName] = property;
                }

                // 在模板中查找要填充数据的单元格位置并替换为数据
                for (int rowIndex = startDataRowIndex, i = 0; i < data.Count; rowIndex++, i++)
                {
                    IRow targetRow = sheet.CreateRow(rowIndex);

                    for (int columnIndex = 0; columnIndex < headerRow.LastCellNum; columnIndex++)
                    {
                        ICell targetCell = targetRow.CreateCell(columnIndex);
                        string headerText = headerRow.GetCell(columnIndex).ToString()!;

                        if (headerText.StartsWith("*"))
                        {
                            headerText = headerText.TrimStart('*');
                        }

                        if (headerToPropertyMap.ContainsKey(headerText))
                        {
                            PropertyInfo property = headerToPropertyMap[headerText];
                            if (targetCell != null)
                            {
                                var value = DealValue(data[i], property);

                                targetCell.SetCellValue(value?.ToString());
                            }
                        }
                    }
                }

                // 写入生成的Excel到内存流
                workbook.Write(outputMemoryStream);
            }

            return outputMemoryStream;
        }


        /// <summary>
        /// 将数据写入Sheet
        /// </summary>
        /// <typeparam name="T">Data类型</typeparam>
        /// <param name="sheet">工作簿</param>
        /// <param name="dataList">数据集</param>
        /// <param name="startColumnIndex">开始列索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="startDataRowIndex">数据开始行索引</param>
        /// <param name="headerRowIndex">表头所在行索引</param>
        public static void FillDataIntoSheet<T>(ISheet sheet, List<T> dataList, int startColumnIndex,int endColumnIndex,int startDataRowIndex, int headerRowIndex)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            IRow headerRow = sheet.GetRow(headerRowIndex); // 模板的表头

            // 创建一个字典，将表头的文本与属性名关联
            Dictionary<string, PropertyInfo> headerToPropertyMap = new();
            foreach (var property in properties)
            {
                string columnName = property.Name; // 默认使用属性名
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>(); // 如果属性有自定义列名属性，则使用自定义列名
                if (columnAttribute != null && !string.IsNullOrWhiteSpace(columnAttribute.Name))
                {
                    columnName = columnAttribute.Name;
                }
                headerToPropertyMap[columnName] = property;
            }

            // 在模板中查找要填充数据的单元格位置并替换为数据
            for (int rowIndex = startDataRowIndex, i = 0; i < dataList.Count; rowIndex++, i++)
            {
                IRow targetRow = sheet.GetRow(rowIndex);

                targetRow ??= sheet.CreateRow(rowIndex);
                             
                for (int columnIndex = startColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                {
                    ICell targetCell = targetRow.CreateCell(columnIndex);
                    string headerText = headerRow.GetCell(columnIndex)?.ToString()!;

                    if (string.IsNullOrEmpty(headerText))
                    {
                        continue;
                    }

                    if (headerText.StartsWith("*"))
                    {
                        headerText = headerText.TrimStart('*');
                    }

                    if (headerToPropertyMap.ContainsKey(headerText))
                    {
                        PropertyInfo property = headerToPropertyMap[headerText];
                        if (targetCell != null)
                        {
                            var value = DealValue(dataList[i], property);

                            targetCell.SetCellValue(value?.ToString());
                        }
                    }
                }
            }

        }


        /// <summary>
        ///  读取excel指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ReadExcelRectangularArea(Stream fileStream, int sheetIndex, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            DataTable dataTable = new();

            using (var workbook = WorkbookFactory.Create(fileStream))
            {
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                dataTable = ReadSheetRectangularArea(sheet, startRowIndex, endRowIndex, startColIndex, endColIndex, includeHeader);
            }

            return dataTable;
        }

        /// <summary>
        ///  读取excel指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="dataTable">接收数据的DataTable</param>
        /// <param name="sheetIndex">数据所在的sheet</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        public static void ReadExcelRectangularArea(Stream fileStream, ref DataTable dataTable, int sheetIndex, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            using var workbook = WorkbookFactory.Create(fileStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            ReadSheetRectangularArea(sheet, ref dataTable, startRowIndex, endRowIndex, startColIndex, endColIndex, includeHeader);
        }


        /// <summary>
        /// 读取Sheet指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="sheet">工作簿</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ReadSheetRectangularArea(ISheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            DataTable dataTable = new();

            if (sheet != null)
            {
                int dataRowIndex = includeHeader ? startRowIndex + 1 : startRowIndex; //拿到真实的数据行索引

                IRow headerRow = sheet.GetRow(startRowIndex);

                if (headerRow != null)
                {
                    for (int i = startColIndex; i <= endColIndex; i++)
                    {
                        if (includeHeader)
                        {
                            ICell headerCell = headerRow.GetCell(i);
                            if (headerCell != null)
                            {
                                string columnName = headerCell.ToString()!;

                                if (columnName.StartsWith("*"))
                                {
                                    columnName = columnName.TrimStart('*');
                                }

                                if (!string.IsNullOrWhiteSpace(columnName))
                                {
                                    dataTable.Columns.Add(columnName, typeof(string)); // 可以根据需要指定数据类型
                                }
                                else
                                {
                                    dataTable.Columns.Add(i.ToString(), typeof(string)); // 可以根据需要指定数据类型
                                }
                            }
                            else
                            {
                                dataTable.Columns.Add("Column" + i.ToString(), typeof(string));
                            }
                        }
                        else
                        {
                            dataTable.Columns.Add("Column" + i.ToString(), typeof(string));
                        }
                    }
                }
                else
                {
                    throw new Exception("数据异常");
                }

                int lastRowIndex = endRowIndex;
                if (endRowIndex < 0)
                {
                    lastRowIndex = sheet.LastRowNum;
                }


                for (int rowIndex = dataRowIndex; rowIndex <= lastRowIndex; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null)
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int columnIndex = startColIndex, j = 0; columnIndex <= endColIndex; columnIndex++, j++)
                        {
                            ICell cell = row.GetCell(columnIndex);
                            if (cell!=null)
                            {
                                if (cell.CellType == CellType.Formula)
                                {
                                    cell = cell.Row.GetCell(cell.ColumnIndex);
                                    dataRow[j] = EvaluateFormula(cell);
                                }
                                else
                                {
                                    var value = GetCellValueToString(cell);
                                    dataRow[j] = value;
                                }
                            }        
                        }
                        dataTable.Rows.Add(dataRow);

                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 读取Sheet指定区域的数据填充到DataTable
        /// </summary>
        /// <param name="sheet">工作簿</param>
        /// <param name="dataTable">接收数据的DataTable</param>
        /// <param name="startRowIndex">数据起始行索引</param>
        /// <param name="endRowIndex">数据截至行索引,如果该值小于0，则视为动态行，自动检索到最后有数据的一行为止</param>
        /// <param name="startColIndex">数据起始列索引</param>
        /// <param name="endColIndex">数据截至列索引</param>
        /// <param name="includeHeader">是否包含表头(如果包含，默认指定区域的第一行就是表头行)</param>
        public static void ReadSheetRectangularArea(ISheet sheet, ref DataTable dataTable, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool includeHeader = true)
        {
            if (sheet != null)
            {
                int dataRowIndex = includeHeader ? startRowIndex + 1 : startRowIndex; //拿到真实的数据行索引

                int lastRowIndex = endRowIndex;
                if (endRowIndex < 0)
                {
                    lastRowIndex = sheet.LastRowNum;
                }

                for (int rowIndex = dataRowIndex; rowIndex <= lastRowIndex; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null)
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int columnIndex = startColIndex, j = 0; columnIndex <= endColIndex; columnIndex++, j++)
                        {
                            ICell cell = row.GetCell(columnIndex);
                            var value = GetCellValueToString(cell);
                            dataRow[j] = value;
                        }
                        dataTable.Rows.Add(dataRow);

                    }
                }
            }
        }


        /// <summary>
        /// 设置DataTable的单元格的值
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="ColumnDataType">列的数据类型</param>
        /// <param name="cellValue">单元格的值</param>
        /// <exception cref="Exception"></exception>
        private static void SetColumnValue(DataRow row, int columnIndex, Type ColumnDataType, string? cellValue)
        {
            if (ColumnDataType == null)
            {
                ColumnDataType = typeof(string); //防止未指定Column的DataType
            }

            switch (ColumnDataType.FullName)
            {
                case "System.Int32":
                    if (!int.TryParse(cellValue, out var intValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为int类型失败");
                    }
                    row[columnIndex] = intValue;
                    break;

                case "System.UInt32":
                    if (!uint.TryParse(cellValue, out var uintValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为uint类型失败");
                    }
                    row[columnIndex] = uintValue;
                    break;

                case "System.Int64":
                    if (!long.TryParse(cellValue, out var longValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为long类型失败");
                    }
                    row[columnIndex] = longValue;
                    break;

                case "System.UInt64":
                    if (!ulong.TryParse(cellValue, out var ulongValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为ulong类型失败");
                    }
                    row[columnIndex] = ulongValue;
                    break;
                case "System.Int16":
                    if (!short.TryParse(cellValue, out var shortValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为short类型失败");
                    }
                    row[columnIndex] = shortValue;
                    break;
                case "System.UInt16":
                    if (!ushort.TryParse(cellValue, out var ushortValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为ushort类型失败");
                    }
                    row[columnIndex] = ushortValue;
                    break;
                case "System.Single":
                    if (!float.TryParse(cellValue, out var floatValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为float类型失败");
                    }
                    row[columnIndex] = floatValue;
                    break;
                case "System.Double":
                    if (!double.TryParse(cellValue, out var doubleValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为double类型失败");
                    }
                    row[columnIndex] = doubleValue;
                    break;
                case "System.Decimal":
                    if (!decimal.TryParse(cellValue, out var decimalValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为decimal类型失败");
                    }
                    row[columnIndex] = decimalValue;
                    break;
                case "System.Char":
                    if (!char.TryParse(cellValue, out var charValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为char类型失败");
                    }
                    row[columnIndex] = charValue;
                    break;
                case "System.SByte":
                    if (!sbyte.TryParse(cellValue, out var sbyteValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为sbyte类型失败");
                    }
                    row[columnIndex] = sbyteValue;
                    break;
                case "System.Boolean":
                    if (!bool.TryParse(cellValue, out var boolValue))
                    {
                        throw new Exception($"将数据{cellValue}转换为bool类型失败");
                    }
                    row[columnIndex] = boolValue;
                    break;
                case "System.String":
                default:
                    row[columnIndex] = cellValue;
                    break;
            }

        }


        /// <summary>
        /// 值处理
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static object? DealValue(object? obj,PropertyInfo property)
        {
            // 根据数据类型设置单元格值，这里默认为字符串
            object? value = property.GetValue(obj);

            if (value is null)
            {
                return string.Empty;
            }
            else
            {
                if (property.PropertyType.IsEnum)
                {
                    // 如果属性是枚举类型，将其转换为字符串
                    return Enum.GetName(property.PropertyType, value);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    // 如果属性是布尔类型，将其转换为"是"或"否"
                    return (bool)value ? "是" : "否";
                }
                else
                {
                    return value;
                }
            }
        }

        /// <summary>
        /// 获取单元格的值，默认全部转string类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string GetCellValueToString(ICell cell)
        {
            if (cell is null)
            {
                return string.Empty;
            }

            if (cell.CellType== CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
            {            
                try
                {
                    var value = cell.DateCellValue;

                    return value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                   
                }
                return cell.ToString();
            }

            return cell.ToString();
        }
        /// <summary>
        /// 使用 EvaluateInCell() 计算公式
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        static string EvaluateFormula(ICell cell)
        {
            if (cell != null)
            {
                try
                {
                    var evaluator = cell.Sheet.Workbook.GetCreationHelper().CreateFormulaEvaluator();
                    evaluator.EvaluateInCell(cell);
                    return cell.ToString();
                }
                catch (Exception ex)
                {
                    return $"Error evaluating formula: {ex.Message}";
                }
            }

            return string.Empty;
        }
    }
}
