using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Tools
{
    public static class DataTableHelper
    {
        public static List<T> DataTableToList<T>(DataTable dataTable) where T : new()
        {
            List<T> list = new();

            if (dataTable is null)
            {
                return list;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            Dictionary<string, PropertyInfo> propertyDict = properties.ToDictionary(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return (columnAttribute != null ? columnAttribute.Name : p.Name)!;
            });

            foreach (DataRow row in dataTable.Rows)
            {
                T item = new();
                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    if (propertyDict.TryGetValue(columnName, out PropertyInfo? property))
                    {
                        object value = row[column];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(item, value);
                        }
                        else
                        {
                            property.SetValue(item, null);
                        }
                    }
                }
                list.Add(item);
            }

            return list;
        }


        public static DataTable ListToDataTable<T>(List<T> data)
        {
            DataTable dataTable = new();

            // 获取目标类型的属性信息
            PropertyInfo[] properties = typeof(T).GetProperties();

            // 创建列映射字典，将列名与属性信息关联
            Dictionary<string, PropertyInfo> columnMappings = new();
            foreach (PropertyInfo property in properties)
            {
                string columnName = property.Name; // 默认使用属性名作为列名
                ColumnAttribute? columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                if (columnAttribute != null && !string.IsNullOrWhiteSpace(columnAttribute.Name))
                {
                    columnName = columnAttribute.Name; // 如果有 ColumnAttribute，则使用指定的列名
                }
                columnMappings[columnName] = property;

                // 创建DataTable列，列名从列映射中获取
                dataTable.Columns.Add(columnName, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // 将List<T>的数据填充到DataTable
            foreach (T item in data)
            {
                DataRow row = dataTable.NewRow();
                foreach (DataColumn column in dataTable.Columns)
                {
                    PropertyInfo property = columnMappings[column.ColumnName];
                    object value = property.GetValue(item);
                    row[column] = value ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
