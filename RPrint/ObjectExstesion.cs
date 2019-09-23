using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPrint
{
    /// <summary>
    /// 对象扩展
    /// </summary>
    public static class ObjectExstesion
    {
        /// <summary>
        /// 对象转Table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ObjectToTable(this object data)
        {
            DataTable dt = new DataTable();
            Type dType = data.GetType();

            IEnumerable<object> dataList = new List<object>();
            if (dType.IsGenericType)
            {
                dType = dType.GetGenericArguments()[0];
                dataList = data as IEnumerable<object>;
            }
            else
            {
                dataList.ToList().Add(data);
            }
            TableAttribute tableAttribute = dType.GetCustomAttribute<TableAttribute>();
            dt.TableName = tableAttribute != null ? tableAttribute.Name : dType.Name;
            PropertyInfo[] properties = dType.GetProperties();
            dt.Columns.AddRange(properties.Where(x => x.GetCustomAttribute<ColumnAttribute>() != null).Select(x => new
            {
                ColumnAttribute = x.GetCustomAttribute<ColumnAttribute>(),
                Type = x.PropertyType
            }).OrderBy(x => x.ColumnAttribute.SortId).Select(x =>
                {
                    DataColumn dataColumn = new DataColumn(x.ColumnAttribute.Name, x.Type);
                    dataColumn.ReadOnly = x.ColumnAttribute.ReadOnly;
                    return dataColumn;
                }).ToArray());
            DataRow newRow = null;
            foreach (var item in dataList)
            {
                newRow = dt.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    ColumnAttribute columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute != null)
                    {
                        newRow[columnAttribute.Name] = property.GetValue(item, null);
                    }
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        /// <summary>
        ///获取列名字段对应表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, PropertyInfo> GetPropertyColumns(this Type type)
        {
            return type.GetProperties().Where(x => x.GetCustomAttribute<ColumnAttribute>() != null).ToDictionary(t => t.GetCustomAttribute<ColumnAttribute>().Name, t => t);
        }
    }
}
