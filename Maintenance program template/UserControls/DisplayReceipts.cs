using Maintenance_program_template.BLL_Business_Logic_Layer_;
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
    public partial class DisplayReceipts : UserControl
    {
        private DeviceReceiptService _service = new DeviceReceiptService();

        private int currentPage = 1;
        private int pageSize = 50;
        public DisplayReceipts()
        {
            InitializeComponent();
        }

        private void DisplayReceipts_Load(object sender, EventArgs e)
        {
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

            LoadReceipts();
        }
        private void LoadReceipts()
        {
            try
            {
                dataGridView1.DataSource = _service.GetReceipts(currentPage, pageSize);

                lblPageInfo.Text = $"الصفحة: {currentPage}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadReceipts();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadReceipts();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = SearchingForPiece.Text.Trim();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    MessageBox.Show("أدخل كلمة البحث");
                    return;
                }

                dataGridView1.DataSource = _service.SearchReceipts(keyword);

                lblPageInfo.Text = "نتائج البحث";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء البحث: " + ex.Message);
            }
        }
    }
}
