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
    public partial class PaymentRecord : UserControl
    {
        private int currentPage = 1;

        private int pageSize = 10;

        private int totalPages = 0;

        PaymentBLL bll = new PaymentBLL();
        public PaymentRecord()
        {
            InitializeComponent();
        }

        private void PaymentRecord_Load(object sender, EventArgs e)
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

            LoadPayments();

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }
        private void LoadPayments()
        {
            try
            {
                var data = bll.GetPayments(
                    dateTimePicker1.Value,
                    dateTimePicker2.Value.Date.AddDays(1),
                    currentPage,
                    pageSize);

                dataGridView1.DataSource = data;

                // تنسيق الجدول
                dataGridView1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.Columns["PaymentID"].Visible = false;

                dataGridView1.Columns["CustomerName"]
                    .HeaderText = "اسم العميل";

                dataGridView1.Columns["PaymentAmount"]
                    .HeaderText = "المبلغ";

                dataGridView1.Columns["PaymentDate"]
                    .HeaderText = "تاريخ الدفع";

                // حساب الصفحات
                totalPages = bll.GetTotalPages(
                    dateTimePicker1.Value,
                    dateTimePicker2.Value,
                    pageSize);

                lblPageInfo.Text =
                    $"الصفحة {currentPage} من {totalPages}";

                // تعطيل الأزرار
                btnPrevious.Enabled = currentPage > 1;

                btnNext.Enabled = currentPage < totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            currentPage = 1;

            LoadPayments();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;

                LoadPayments();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                LoadPayments();
            }
        }
    }
}
