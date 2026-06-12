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
    public partial class Suppliers : UserControl
    {
        private SupplierService _service = new SupplierService();

        private int currentPage = 1;
        private int pageSize = 50;
        public Suppliers()
        {
            InitializeComponent();
        }
        private void LoadSuppliers()
        {
            try
            {
                dataGridView1.DataSource = _service.GetSuppliers(currentPage, pageSize);

                dataGridView1.Columns["SupplierID"].HeaderText = "المعرف";
                dataGridView1.Columns["SupplierName"].HeaderText = "اسم الشركة";
                dataGridView1.Columns["SupplierPhone"].HeaderText = "رقم الهاتف";
                dataGridView1.Columns["Notes"].HeaderText = "ملاحظات";

                lblPageInfo.Text = $"الصفحة: {currentPage}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل الموردين: " + ex.Message);
            }
        }
        private void Suppliers_Load(object sender, EventArgs e)
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

            LoadSuppliers();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadSuppliers();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadSuppliers();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddedByCompany added = new AddedByCompany();
            added.OnSupplierAdded += RefreshSuppliers;

            added.ShowDialog();
        }
        private void RefreshSuppliers()
        {
            LoadSuppliers(); // نفس الدالة اللي عندك
        }
    }
}
