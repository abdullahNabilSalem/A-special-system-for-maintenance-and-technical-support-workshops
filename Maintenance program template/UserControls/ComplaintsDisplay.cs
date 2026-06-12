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
    public partial class ComplaintsDisplay : UserControl
    {
        int currentPage = 1;
        int pageSize = 10;
        int totalPages = 0;

        ComplaintBLL bll = new ComplaintBLL();
        public ComplaintsDisplay()
        {
            InitializeComponent();
        }

        private void ComplaintsDisplay_Load(object sender, EventArgs e)
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

            LoadData();
        }
        private void LoadData()
        {
            dataGridView1.DataSource = bll.GetComplaints(currentPage, pageSize);

            totalPages = bll.GetTotalPages(pageSize);

            lblPageInfo.Text = $"Page {currentPage} of {totalPages}";

            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchingForPiece.Text))
            {
                // رجوع للوضع الطبيعي (Pagination)
                currentPage = 1;
                LoadData();
                return;
            }

            if (int.TryParse(SearchingForPiece.Text, out int complaintId))
            {
                var result = bll.SearchComplaintByID(complaintId);

                if (result.Rows.Count > 0)
                {
                    dataGridView1.DataSource = result;

                    lblPageInfo.Text = "نتيجة البحث";
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                }
                else
                {
                    MessageBox.Show("لا توجد شكوى بهذا الرقم");
                }
            }
            else
            {
                MessageBox.Show("الرجاء إدخال رقم صحيح");
            }
        }

        private void SearchingForPiece_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSerach.PerformClick();
            }
        }
    }
}
