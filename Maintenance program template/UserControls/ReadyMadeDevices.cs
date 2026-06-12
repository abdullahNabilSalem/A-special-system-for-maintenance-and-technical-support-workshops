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
    public partial class ReadyMadeDevices : UserControl
    {
        private int currentPage = 1;

        private int pageSize = 10;

        private int totalPages = 0;

        DeviceReceiptBLL bll =
            new DeviceReceiptBLL();
        public ReadyMadeDevices()
        {
            InitializeComponent();
        }

        private void ReadyMadeDevices_Load(object sender, EventArgs e)
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

            LoadCompletedDevices();
        }
        private void LoadCompletedDevices()
        {
            try
            {
                var data =
                    bll.GetCompletedDevices(
                        currentPage,
                        pageSize);

                dataGridView1.DataSource = data;

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

                totalPages =
                    bll.GetCompletedPages(
                        pageSize);

                lblPageInfo.Text =
                    $"الصفحة {currentPage} من {totalPages}";

                btnPrevious.Enabled =
                    currentPage > 1;

                btnNext.Enabled =
                    currentPage < totalPages;

                AddDeliverButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
        private void AddDeliverButton()
        {
            if (!dataGridView1.Columns
                .Contains("DeliverButton"))
            {
                DataGridViewButtonColumn btn =
                    new DataGridViewButtonColumn();

                btn.Name = "DeliverButton";

                btn.HeaderText = "الإجراء";

                btn.Text = "📦 تسليم الجهاز";

                btn.UseColumnTextForButtonValue = true;

                btn.Width = 150;

                btn.FlatStyle = FlatStyle.Flat;

                dataGridView1.Columns.Add(btn);
            }

            // تنسيق الزر
            dataGridView1.Columns["DeliverButton"]
                .DefaultCellStyle.BackColor =
                Color.FromArgb(255, 140, 0);

            dataGridView1.Columns["DeliverButton"]
                .DefaultCellStyle.ForeColor =
                Color.White;

            dataGridView1.Columns["DeliverButton"]
                .DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(230, 120, 0);

            dataGridView1.Columns["DeliverButton"]
                .DefaultCellStyle.SelectionForeColor =
                Color.White;

            dataGridView1.Columns["DeliverButton"]
                .DefaultCellStyle.Font =
                new Font("Tahoma", 9,
                FontStyle.Bold);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
       e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex]
                .Name == "DeliverButton")
            {
                int receiptID =
                    Convert.ToInt32(
                        dataGridView1.Rows[e.RowIndex]
                        .Cells["ReceiptID"].Value);

                try
                {
                    bll.DeliverDevice(receiptID);

                    MessageBox.Show(
                        "✔ تم تسليم الجهاز");

                    LoadCompletedDevices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ " + ex.Message);
                }
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            string keyword =
        SearchingForPiece.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadCompletedDevices();
                return;
            }

            dataGridView1.DataSource =
                bll.SearchCompletedDevices(keyword);

            AddDeliverButton();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;

                LoadCompletedDevices();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                LoadCompletedDevices();
            }
        }
    }
}
