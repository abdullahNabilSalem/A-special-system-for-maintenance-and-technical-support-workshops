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

namespace Maintenance_program_template.UI_Windows_Forms_
{
    public partial class FrmCustomerLookup : Form
    {
        public int SelectedCustomerID { get; set; }

        public string SelectedCustomerName { get; set; }

        CustomerBLL bll = new CustomerBLL();

        int currentPage = 1;

        int pageSize = 10;

        int totalPages = 0;

        string searchText = "";
        public FrmCustomerLookup()
        {
            InitializeComponent();
        }

        private void FrmCustomerLookup_Load(object sender, EventArgs e)
        {
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.None;

            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);

            dgvCustomers.DefaultCellStyle.Font = new Font("Tahoma", 10);
            dgvCustomers.RowTemplate.Height = 30;

            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            LoadCustomers();
        }
        private void LoadCustomers()
        {
            var customers =
                bll.GetCustomersPaged(
                    currentPage,
                    pageSize,
                    searchText);

            dgvCustomers.DataSource = customers;

            int totalRecords =
                bll.GetCustomersCount(searchText);

            totalPages =
                (int)Math.Ceiling(
                    (double)totalRecords / pageSize);

            lblPageInfo.Text =
                $"Page {currentPage} / {totalPages}";

            btnPrevious.Enabled =
                currentPage > 1;

            btnNext.Enabled =
                currentPage < totalPages;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;

                LoadCustomers();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                LoadCustomers();
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            searchText =
        txtSearch.Text.Trim();

            currentPage = 1;

            LoadCustomers();
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedCustomerID = Convert.ToInt32(
                    dgvCustomers.Rows[e.RowIndex]
                    .Cells["CustomerID"].Value);

                SelectedCustomerName =
                    dgvCustomers.Rows[e.RowIndex]
                    .Cells["CustomerName"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
