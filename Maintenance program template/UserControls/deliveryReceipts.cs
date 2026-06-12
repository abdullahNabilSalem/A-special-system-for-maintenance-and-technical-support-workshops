using Maintenance_program_template.DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maintenance_program_template.UserControls
{
    public partial class deliveryReceipts : UserControl
    {
        private DeviceReceiptRepository _service = new DeviceReceiptRepository();

        private int currentPage = 1;
        private int pageSize = 50;
        public deliveryReceipts()
        {
            InitializeComponent();
        }

        private void deliveryReceipts_Load(object sender, EventArgs e)
        {
            LoadRepairs();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);

            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 10);
            dataGridView1.RowTemplate.Height = 30;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
        }
        private void LoadRepairs()
        {
            try
            {
                dataGridView1.DataSource = _service.GetRepairs(currentPage, pageSize);
                lblPageInfo.Text = $"الصفحة: {currentPage}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadRepairs();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadRepairs();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                string search = SearchingForPiece.Text.Trim();

                if (string.IsNullOrWhiteSpace(search))
                {
                    MessageBox.Show("⚠️ أدخل رقم السند أو رقم الهاتف");
                    return;
                }

                DataTable dt = _service.SearchRepairs(search);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("🚫 لا توجد نتائج");
                    return;
                }

                dataGridView1.DataSource = dt;
                lblPageInfo.Text = $"عدد النتائج: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }
    }
}
