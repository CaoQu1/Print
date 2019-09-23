using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPrint
{
    /// <summary>
    /// 列表页
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        /// 操作的数据集
        /// </summary>
        List<ProductModel> list = new List<ProductModel>();
        /// <summary>
        /// 最大净重
        /// </summary>
        double MaxNetWeight = double.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxNetWeight"]);

        /// <summary>
        /// ctor
        /// </summary>
        public Main()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CellClick += DataGridView1_CellClick;
            //禁止用户更改行高
            dataGridView1.AllowUserToResizeRows = false;
            // 禁止用户改变列头的高度  
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dataGridView1.CellStateChanged += DataGridView1_CellStateChanged;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
            //dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;
        }

        /// <summary>
        /// 单元格编辑完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];
            if (dataGridViewRow != null)
            {
                DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[e.ColumnIndex];
                if (dataGridViewCell != null && list != null && list.Any())
                {
                    ProductModel model = list.FirstOrDefault(x => x.Id == Convert.ToInt32(dataGridViewRow.Cells[0].Value));
                    var columnName = dataGridViewCell.OwningColumn.Name;
                    var cellValue = dataGridViewCell.Value;
                    Type type = model.GetType();
                    Dictionary<string, PropertyInfo> columnProperties = type.GetPropertyColumns();
                    PropertyInfo propertyInfo = columnProperties[columnName];
                    if (propertyInfo != null && cellValue != DBNull.Value && cellValue != null)
                    {
                        propertyInfo.SetValue(model, cellValue);
                    }
                    Task.Run(() =>
                    {
                        dataGridView1.BeginInvoke(new Action(() =>
                        {
                            dataGridView1.DataSource = list.ObjectToTable();
                        }));
                    });
                }
            }
        }

        /// <summary>
        /// 单元格状态变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ReadOnly == true)
            {
                dataGridView1.Rows[e.Cell.RowIndex].Selected = true;
                RowPrint(dataGridView1.Rows[e.Cell.RowIndex]);
            }
        }

        /// <summary>
        /// 单元格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ReadOnly == true)
            {
                RowPrint(dataGridView1.Rows[e.RowIndex]);
            }
            else
            {
                dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView1.BeginEdit(true);
            }
        }

        /// <summary>
        /// 单条打印
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        private void RowPrint(DataGridViewRow gridViewRow)
        {
            if (gridViewRow == null)
            {
                throw new ArgumentNullException("请选择要打印的行数!");
            }
            ProductModel model = new ProductModel();
            model.SubProductName = "";
            Dictionary<string, PropertyInfo> columnDictionries = model.GetType().GetPropertyColumns();
            foreach (DataGridViewCell item in gridViewRow.Cells)
            {
                if (columnDictionries.ContainsKey(item.OwningColumn.Name) && item.Value != null && item.Value != DBNull.Value)
                {
                    columnDictionries[item.OwningColumn.Name].SetValue(model, item.Value);
                }
            }
            Print print = new Print(new List<ProductModel> { model });
            print.Show();
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print print = new Print(list);
            print.Show();
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string weight = txtWeight.Text.Trim();
            double netWight = 0;
            if (!string.IsNullOrEmpty(weight) && double.TryParse(weight, out netWight))
            {
                if (netWight > MaxNetWeight)
                {
                    MessageBox.Show("超过最大净重,请重新输入!");
                    txtWeight.Text = "";
                    return;
                }
                Task.Run(() =>
                {
                    GenerateData generateData = new GenerateData();
                    var generateTable = generateData.Generate(netWight, out list);
                    this.BeginInvoke(new Action(() =>
                    {
                        dataGridView1.DataSource = generateTable;
                        WriteToLabel(this, new MyEventArgs($"加载完成,一共{generateTable.Rows.Count}行数据！"));
                    }));
                });
            }
            else
            {
                MessageBox.Show("请输入净重!");
            }
        }

        /// <summary>
        /// 写描述到界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WriteToLabel(object sender, MyEventArgs e)
        {
            this.statusLbl.ForeColor = Color.Red;
            this.statusLbl.Text = e.Message;
        }
    }

    /// <summary>
    /// 自定义事件消息
    /// </summary>
    public class MyEventArgs : EventArgs
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public MyEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
