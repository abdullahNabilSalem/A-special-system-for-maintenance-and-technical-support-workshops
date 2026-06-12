using Maintenance_program_template.BLL_Business_Logic_Layer_;
using Maintenance_program_template.UI_Windows_Forms_;
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
    public partial class AddClient : UserControl
    {
        BLL_Business_Logic_Layer_.CustomerBLL bll = new BLL_Business_Logic_Layer_.CustomerBLL();
        CustomerBLL customerBLL = new CustomerBLL();

        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 0;
        public AddClient()
        {
            InitializeComponent();
        }

        private void AddClient_Load(object sender, EventArgs e)
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

            LoadCustomers();
        }
        private void LoadCustomers()
        {
            try
            {
                var customers = bll.GetCustomersAll(currentPage, pageSize);

                dataGridView1.DataSource = customers;

                dataGridView1.Columns["CustomerID"].Visible = false;

                dataGridView1.Columns["CustomerName"].HeaderText = "اسم العميل";
                dataGridView1.Columns["Phone"].HeaderText = "رقم الهاتف";
                dataGridView1.Columns["Address"].HeaderText = "العنوان";
                dataGridView1.Columns["Notes"].HeaderText = "ملاحظات";

                totalPages = bll.GetTotalPages(pageSize);

                lblPageInfo.Text = $"الصفحة {currentPage} من {totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void btnAddComplaint_Click(object sender, EventArgs e)
        {
            SaveCustomer saveCustomer = new SaveCustomer();

            // 🔥 ربط الحدث
            saveCustomer.CustomerAdded += LoadCustomers;

            saveCustomer.ShowDialog();
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = customerBLL.SearchCustomers(SearchingForPiece.Text);
        }

        private void SearchingForPiece_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = customerBLL.SearchCustomers(SearchingForPiece.Text);
        }
    }
}
