using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPrint
{
    /// <summary>
    /// 列特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int SortId { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnAttribute(string name, bool readOnly = false, int sortId = 99)
        {
            this.Name = name;
            this.ReadOnly = readOnly;
            this.SortId = sortId;
        }
    }

    /// <summary>
    /// 表特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
