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

namespace Maintenance_program_template.UI_Windows_Forms_
{
    public partial class Receipt : Form
    {
        private DeviceReceiptService _service = new DeviceReceiptService();
        private ProductService _productService = new ProductService();
        private ReceiptReportService _report = new ReceiptReportService();
        public Receipt()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void addedBtnCompany_Click(object sender, EventArgs e)
        {
            SaveDeviceReceipt();
        }
        private void SaveDeviceReceipt()
        {
            try
            {
                DeviceReceipt receipt = new DeviceReceipt
                {
                    CustomerName = textBox2.Text.Trim(),
                    PhoneNumber = textBox5.Text.Trim(),
                    ProductType = comboBox3.Text.Trim(),
                    ProblemDescription = textBox3.Text.Trim(),
                    ReceiveDate = dateTimePicker1.Value,
                    ReceivedBy = comboBox1.Text.Trim(),
                    SerialNumber = textBox1.Text.Trim(),
                    ReceiptNumber = textBox4.Text.Trim(),
                    DeviceStatus = comboBox2.Text.Trim()
                };

                int id = _service.AddReceipt(receipt);

                MessageBox.Show($"تم تسجيل الجهاز بنجاح\nرقم السند: {id}");

                PrintReceipt(id); // UI Logic

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PrintReceipt(int id)
        {
            var model = new InvoiceModel
            {
                ReceiptId = id,
                SerialNumber = textBox1.Text.Trim(),
                CustomerName = textBox2.Text.Trim(),
                Phone = textBox5.Text.Trim(),
                ProductType = comboBox3.Text.Trim(),
                Problem = textBox3.Text.Trim(),
                ReceivedBy = comboBox1.Text.Trim(),
                ReceiveDate = dateTimePicker1.Value
            };

            try
            {
                _report.PrintReceipt(model);

                MessageBox.Show("تم إنشاء PDF بنجاح ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ:\n" + ex.Message);
            }
        }
        private void Receipt_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadProducts();
        }
        private void LoadEmployees()
        {
            comboBox1.DataSource = _service.GetEmployees();
            comboBox1.DisplayMember = "EmployeeName";
            comboBox1.ValueMember = "EmployeeID";
            comboBox1.SelectedIndex = -1;
        }
        private void LoadProducts()
        {
            try
            {
                comboBox3.DataSource = _productService.GetProducts();
                comboBox3.DisplayMember = "ProductName";
                comboBox3.ValueMember = "ProductID";
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل المنتجات: " + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
