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
    public partial class ReceivedDevices : UserControl
    {
        BLL_Business_Logic_Layer_.DeviceReceiptService reportsBLL = new BLL_Business_Logic_Layer_.DeviceReceiptService();

        int currentPage = 1;
        int pageSize = 50;
        int totalPages;
        public ReceivedDevices()
        {
            InitializeComponent();
        }

        private void ReceivedDevices_Load(object sender, EventArgs e)
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

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }
        private DateTime? GetFromDate()
        {
            return dateTimePicker1.Value.Date;
        }

        private DateTime? GetToDate()
        {
            return dateTimePicker2.Value.Date;
        }
        private void LoadReceipts()
        {
            DateTime? from = GetFromDate();
            DateTime? to = GetToDate();

            DataTable dt = reportsBLL.GetReceipts(from, to, currentPage, pageSize);
            dataGridView1.DataSource = dt;

            int totalRows = reportsBLL.GetReceiptsCount(from, to);
            totalPages = (int)Math.Ceiling((double)totalRows / pageSize);

            if (totalRows == 0)
            {
                lblPageInfo.Text = "لا توجد بيانات";
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                return;
            }

            lblPageInfo.Text = $"Page {currentPage} of {totalPages}";

            btnNext.Enabled = currentPage < totalPages;
            btnPrevious.Enabled = currentPage > 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadReceipts();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadReceipts();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadReceipts();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadReceipts();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DateTime? from = GetFromDate();
            DateTime? to = GetToDate();

            // تحقق من صحة التاريخ
            if (from.HasValue && to.HasValue && from > to)
            {
                MessageBox.Show("تاريخ البداية أكبر من تاريخ النهاية");
                return;
            }

            currentPage = 1; // مهم جدًا
            LoadReceipts();
        }
    }
}
