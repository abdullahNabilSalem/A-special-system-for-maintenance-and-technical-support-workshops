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
    public partial class OngoingDevices : UserControl
    {
        private int currentPage = 1;

        private int pageSize = 10;

        private int totalPages = 0;

        DeviceReceiptBLL bll =
            new DeviceReceiptBLL();
        public OngoingDevices()
        {
            InitializeComponent();
        }

        private void OngoingDevices_Load(object sender, EventArgs e)
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

            LoadInProgressDevices();
        }
        private void LoadInProgressDevices()
        {
            try
            {
                var data =
                    bll.GetInProgressDevices(
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
                    .DefaultCellStyle.Format =
                    "yyyy/MM/dd";

                // Pagination
                totalPages =
                    bll.GetInProgressTotalPages(
                        pageSize);

                lblPageInfo.Text =
                    $"الصفحة {currentPage} من {totalPages}";

                btnPrevious.Enabled =
                    currentPage > 1;

                btnNext.Enabled =
                    currentPage < totalPages;

                AddFinishButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "❌ " + ex.Message);
            }
        }
        private void AddFinishButton()
        {
            if (!dataGridView1.Columns
                .Contains("FinishWorkButton"))
            {
                DataGridViewButtonColumn btn =
                    new DataGridViewButtonColumn();

                btn.Name = "FinishWorkButton";

                btn.HeaderText = "الإجراء";

                btn.Text = "✅ إنهاء العمل";

                btn.UseColumnTextForButtonValue = true;

                btn.Width = 140;

                btn.FlatStyle = FlatStyle.Flat;

                dataGridView1.Columns.Add(btn);
            }

            dataGridView1.Columns["FinishWorkButton"]
                .DefaultCellStyle.BackColor =
                Color.FromArgb(40, 167, 69);

            dataGridView1.Columns["FinishWorkButton"]
                .DefaultCellStyle.ForeColor =
                Color.White;

            dataGridView1.Columns["FinishWorkButton"]
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(30, 130, 50);

            dataGridView1.Columns["FinishWorkButton"]
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            dataGridView1.Columns["FinishWorkButton"]
                .DefaultCellStyle.Font =
                new Font("Tahoma", 9, FontStyle.Bold);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
        e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex]
                .Name == "FinishWorkButton")
            {
                int receiptID =
                    Convert.ToInt32(
                        dataGridView1.Rows[e.RowIndex]
                        .Cells["ReceiptID"].Value);

                try
                {
                    bll.MoveToCompleted(
                        receiptID);

                    MessageBox.Show(
                        "✔ تم نقل الجهاز إلى الأجهزة الجاهزة");

                    LoadInProgressDevices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "❌ " + ex.Message);
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;

                LoadInProgressDevices();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                LoadInProgressDevices();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword =
                    SearchingForPiece.Text.Trim();

                var data =
                    bll.SearchInProgressDevices(
                        keyword);

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

                AddFinishButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "❌ " + ex.Message);
            }
        }
    }
}
