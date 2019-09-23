using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPrint
{
    /// <summary>
    /// 打印页面
    /// </summary>
    public partial class Print : Form
    {
        /// <summary>
        /// report坐标
        /// </summary>
        private static int X = 0;
        private static int Y = 0;

        /// <summary>
        /// ctor
        /// </summary>
        public Print()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="list">数据集</param>
        public Print(List<ProductModel> list) : this()
        {
            Task.Run(() =>
            {
                InitReport(list);
            });
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="list"></param>
        public void InitReport(List<ProductModel> list)
        {
            try
            {
                //foreach (var item in list)
                //{
                while (!this.IsHandleCreated)
                { }
                this.BeginInvoke(new Action(() =>
                    {
                        var reportViewer1 = InitReport();
                        ReportDataSource rds1 = new ReportDataSource("DataSet1", list);//DataSet_JSHZB对应报表文件中的数据集
                        reportViewer1.LocalReport.DataSources.Add(rds1);
                        reportViewer1.RefreshReport();
                        this.Controls.Add(reportViewer1);
                    }));

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化ReportViewer
        /// </summary>
        /// <returns></returns>
        public ReportViewer InitReport()
        {
            var reportViewer1 = new ReportViewer();
            reportViewer1.Name = "货物过磅单";
            reportViewer1.LocalReport.ReportPath = "Report2.rdlc";
            reportViewer1.Dock = System.Windows.Forms.DockStyle.None;
            reportViewer1.Location = new System.Drawing.Point(X, Y);
            reportViewer1.Size = new System.Drawing.Size(985, 500);
            reportViewer1.TabIndex = 0;
            reportViewer1.Margin = new Padding(20);
            return reportViewer1;
        }
    }
}
