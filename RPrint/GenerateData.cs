using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPrint
{
    /// <summary>
    /// 生成数据类
    /// </summary>
    public class GenerateData
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
        /// 生成数据
        /// </summary>
        /// <param name="netWeight"></param>
        /// <returns></returns>
        public DataTable Generate(double netWeight, out List<ProductModel> models)
        {
            models = new List<ProductModel>();
            int index = 1;
            while (true)
            {
                var grossWeight = GetRandomNumber(GrossWeightMin, GrossWeightMax, 2);
                var deadWeight = GetRandomNumber(DeadWeightMin, DeadWeightMax, 2);
                var _netWeight = grossWeight - deadWeight;
                netWeight = Math.Round(netWeight - Math.Round(_netWeight, 2));
                if (netWeight > 0 && _netWeight > 0)
                {
                    ProductModel model = new ProductModel();
                    model.GrossWeight = grossWeight;
                    model.DeadWeight = deadWeight;
                    model.NetWeight = _netWeight;
                    model.ProductNo = index.ToString().PadLeft(6, '0');
                    model.Id = index++;
                    models.Add(model);
                }
                else
                {
                    break;
                }
            }
            return models.ObjectToTable();
        }

        /// <summary>
        /// 生成范围随机数
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public double GetRandomNumber(double minimum, double maximum, int Len)   //Len小数点保留位数
        {
            Random random = new Random(GetRandomSeed());
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, Len);
        }

        /// <summary>
        /// 随机种子
        /// </summary>
        /// <returns></returns>
        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
