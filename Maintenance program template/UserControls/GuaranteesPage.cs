using Maintenance_program_template.BLL_Business_Logic_Layer_;
using Maintenance_program_template.Models;
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
    public partial class GuaranteesPage : UserControl
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 0;

        DeviceReceiptBLL bll = new DeviceReceiptBLL();
        public GuaranteesPage()
        {
            InitializeComponent();
        }

        private void GuaranteesPage_Load(object sender, EventArgs e)
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
                var data = bll.GetRepairs(currentPage, pageSize);

                dataGridView1.DataSource = data;

                dataGridView1.Columns["RepairID"].HeaderText = "رقم الإصلاح";
                dataGridView1.Columns["CustomerName"].HeaderText = "اسم العميل";
                dataGridView1.Columns["ProblemDescription"].HeaderText = "المشكلة";
                dataGridView1.Columns["ReceiptNumber"].HeaderText = "رقم السند";
                dataGridView1.Columns["SerialNumber"].HeaderText = "السيريال";
                dataGridView1.Columns["WorkDescription"].HeaderText = "وصف العمل";
                dataGridView1.Columns["ClientNotes"].HeaderText = "ملاحظات العميل";
                dataGridView1.Columns["RepairCost"].HeaderText = "التكلفة";
                dataGridView1.Columns["PaymentMethod"].HeaderText = "طريقة الدفع";
                dataGridView1.Columns["DeliveryDate"].HeaderText = "تاريخ التسليم";
                dataGridView1.Columns["WarrantyPeriod"].HeaderText = "مدة الضمان";

                dataGridView1.Columns["RepairID"].Width = 80;

                totalPages = bll.GetRepairsPages(pageSize);

                lblPageInfo.Text = $"الصفحة {currentPage} من {totalPages}";

                btnPrevious.Enabled = currentPage > 1;
                btnNext.Enabled = currentPage < totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            string keyword = SearchingForPiece.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadRepairs();
                return;
            }

            dataGridView1.DataSource = bll.SearchRepairs(keyword);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadRepairs();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadRepairs();
            }
        }
    }
}
