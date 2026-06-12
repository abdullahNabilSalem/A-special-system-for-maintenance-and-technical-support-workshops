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
    public partial class CustomerResponse : UserControl
    {
        DeviceReceiptBLL bll = new DeviceReceiptBLL();

        private int currentPage = 1;

        private int pageSize = 10;

        private int totalPages = 0;

        public CustomerResponse()
        {
            InitializeComponent();
        }

        private void CustomerResponse_Load(object sender, EventArgs e)
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

            LoadWaitingClients();
        }
        private void LoadWaitingClients()
        {
            try
            {
                var data =
                    bll.GetWaitingClients(
                        currentPage,
                        pageSize);

                dataGridView1.DataSource = data;

                // تنسيق الأعمدة
                dataGridView1.Columns["ReceiptID"]
                    .Visible = false;

                dataGridView1.Columns["CustomerName"]
                    .HeaderText = "اسم العميل";

                dataGridView1.Columns["PhoneNumber"]
                    .HeaderText = "رقم الهاتف";

                dataGridView1.Columns["ProductType"]
                    .HeaderText = "نوع الجهاز";

                dataGridView1.Columns["ProblemDescription"]
                    .HeaderText = "وصف المشكلة";

                dataGridView1.Columns["DeviceStatus"]
                    .HeaderText = "حالة الجهاز";

                dataGridView1.Columns["ReceiptNumber"]
                    .HeaderText = "رقم السند";

                dataGridView1.Columns["SerialNumber"]
                    .HeaderText = "الرقم التسلسلي";

                dataGridView1.Columns["ReceiveDate"]
                    .HeaderText = "تاريخ الاستلام";

                dataGridView1.Columns["ReceivedBy"]
                    .HeaderText = "المستلم";

                dataGridView1.Columns["ReceiveDate"]
                    .DefaultCellStyle.Format = "yyyy/MM/dd";

                // الصفحات
                totalPages =
                    bll.GetTotalPages(pageSize);

                lblPageInfo.Text =
                    $"الصفحة {currentPage} من {totalPages}";

                btnPrevious.Enabled =
                    currentPage > 1;

                btnNext.Enabled =
                    currentPage < totalPages;

                AddActionButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "❌ " + ex.Message);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name
                == "StartWorkButton")
            {
                int receiptID = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex]
                    .Cells["ReceiptID"].Value);

                try
                {
                    bll.MoveToInProgress(receiptID);

                    MessageBox.Show(
                        "✔ تم نقل الجهاز إلى (جاري العمل)");

                    LoadWaitingClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ " + ex.Message);
                }
            }
        }
        private void AddActionButton()
        {
            if (!dataGridView1.Columns.Contains("StartWorkButton"))
            {
                DataGridViewButtonColumn btn =
                    new DataGridViewButtonColumn();

                btn.Name = "StartWorkButton";

                btn.HeaderText = "الإجراء";

                btn.Text = "🛠 بدء العمل";

                btn.UseColumnTextForButtonValue = true;

                btn.Width = 140;

                btn.FlatStyle = FlatStyle.Flat;

                dataGridView1.Columns.Add(btn);
            }

            // تنسيق الزر
            dataGridView1.Columns["StartWorkButton"]
                .DefaultCellStyle.BackColor =
                Color.FromArgb(0, 120, 215);

            dataGridView1.Columns["StartWorkButton"]
                .DefaultCellStyle.ForeColor =
                Color.White;

            dataGridView1.Columns["StartWorkButton"]
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(0, 90, 180);

            dataGridView1.Columns["StartWorkButton"]
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            dataGridView1.Columns["StartWorkButton"]
                .DefaultCellStyle.Font =
                new Font("Tahoma", 9, FontStyle.Bold);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;

                LoadWaitingClients();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                LoadWaitingClients();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword =
                    SearchingForPiece.Text.Trim();

                var data =
                    bll.SearchWaitingClients(keyword);

                dataGridView1.DataSource = data;

                // إعادة تنسيق الأعمدة
                dataGridView1.Columns["ReceiptID"]
                    .Visible = false;

                dataGridView1.Columns["CustomerName"]
                    .HeaderText = "اسم العميل";

                dataGridView1.Columns["PhoneNumber"]
                    .HeaderText = "رقم الهاتف";

                dataGridView1.Columns["ProductType"]
                    .HeaderText = "نوع الجهاز";

                dataGridView1.Columns["ProblemDescription"]
                    .HeaderText = "وصف المشكلة";

                dataGridView1.Columns["DeviceStatus"]
                    .HeaderText = "حالة الجهاز";

                dataGridView1.Columns["ReceiptNumber"]
                    .HeaderText = "رقم السند";

                dataGridView1.Columns["SerialNumber"]
                    .HeaderText = "الرقم التسلسلي";

                dataGridView1.Columns["ReceiveDate"]
                    .HeaderText = "تاريخ الاستلام";

                dataGridView1.Columns["ReceivedBy"]
                    .HeaderText = "المستلم";

                AddActionButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
    }
}
