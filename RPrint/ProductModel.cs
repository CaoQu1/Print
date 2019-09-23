using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPrint
{
    /// <summary>
    /// 产品类
    /// </summary>
    [Table("打印模型")]
    public class ProductModel
    {
        /// <summary>
        /// 最小毛重
        /// </summary>
        public static double GrossWeightMin = double.Parse(System.Configuration.ConfigurationManager.AppSettings["GrossWeightMin"].ToString());
        /// <summary>
        /// 最大毛重
        /// </summary>
        public static double GrossWeightMax = double.Parse(System.Configuration.ConfigurationManager.AppSettings["GrossWeightMax"].ToString());
        /// <summary>
        /// 最小自重
        /// </summary>
        public static double DeadWeightMin = double.Parse(System.Configuration.ConfigurationManager.AppSettings["DeadWeightMin"].ToString());
        /// <summary>
        /// 最大自重
        /// </summary>
        public static double DeadWeightMax = double.Parse(System.Configuration.ConfigurationManager.AppSettings["DeadWeightMax"].ToString());


        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        [Column("序号", true, 1)]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }


        /// <summary>
        /// 毛重
        /// </summary>
        private double grossWeight;
        [Column("毛重", true)]
        public double GrossWeight
        {
            get
            {
                return grossWeight;
            }
            set
            {
                if (GrossWeightMax >= value && GrossWeightMin <= value)
                {
                    grossWeight = value;
                }
                else
                {
                    throw new Exception("参数不符合范围(" + GrossWeightMin + "~" + GrossWeightMax + ")!" + value);
                }
            }
        }

        /// <summary>
        /// 自重
        /// </summary>
        private double deadWeight;
        [Column("自重", true)]
        public double DeadWeight
        {
            get
            {
                return deadWeight;
            }
            set
            {
                if (DeadWeightMax >= value && DeadWeightMin <= value)
                {
                    deadWeight = value;
                }
                else
                {
                    throw new Exception("参数不符合范围(" + DeadWeightMin + "~" + DeadWeightMax + ")!" + value);
                }
            }
        }

        /// <summary>
        /// 净重
        /// </summary>
        private double netWeight;
        [Column("净重", true)]
        public double NetWeight
        {
            get
            {
                return netWeight;
            }
            set
            {
                netWeight = Math.Round(value, 2);
            }
        }

        /// <summary>
        ///产品编号
        /// </summary>

        private string productNo;
        [Column("产品编号", true)]
        public string ProductNo
        {
            get
            {

                return productNo;
            }
            set
            {
                productNo = value;
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        private string productName = "货物过磅单";
        [Column("产品名称", true)]
        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
            }
        }

        /// <summary>
        /// 子产品名称
        /// </summary>
        public string SubProductName { get; set; } = "货物过磅单";

        /// <summary>
        /// 物资名称
        /// </summary>
        private string materialName;
        [Column("物资名称")]
        public string MaterialName
        {
            get
            {
                return materialName;
            }
            set
            {
                materialName = value;
            }
        }

        /// <summary>
        /// 车号
        /// </summary>
        private string licenseNumber;
        [Column("车号")]
        public string LicenseNumber
        {
            get
            {
                return licenseNumber;
            }
            set
            {
                licenseNumber = value;
            }
        }

        /// <summary>
        /// 驾驶员
        /// </summary>
        private string driver;
        [Column("驾驶员")]
        public string Driver
        {
            get
            {
                return driver;
            }
            set
            {
                driver = value;
            }
        }

        /// <summary>
        /// 供货单位
        /// </summary>
        private string supplier;
        [Column("供货单位")]
        public string Supplier
        {
            get
            {
                return supplier;
            }
            set
            {
                supplier = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string remark;
        [Column("备注")]
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
            }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        private string customerName;
        [Column("客户名称", false, 2)]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
            }
        }

        /// <summary>
        /// 过磅员
        /// </summary>
        private string weigher;
        [Column("过磅员")]
        public string Weigher
        {
            get
            {
                return weigher;
            }
            set
            {
                weigher = value;
            }
        }

    }
}
