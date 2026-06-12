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
    public partial class SaveCustomer : Form
    {
        CustomerBLL bll = new CustomerBLL();
        int CustomerID = 0;

        public delegate void CustomerAddedHandler();

        public event CustomerAddedHandler CustomerAdded;
        public SaveCustomer()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SaveCustomer_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Customer customer = new Customer
                {
                    CustomerName = textBox1.Text.Trim(),
                    Phone = textBox2.Text.Trim(),
                    Address = textBox3.Text.Trim(),
                    Notes = textBox4.Text.Trim()
                };

                bll.AddCustomer(customer);

                MessageBox.Show(
                    "✅ تم إضافة العميل بنجاح",
                    "نجاح",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // 🔥 تحديث الجدول تلقائيًا
                CustomerAdded?.Invoke();

                // 🔥 إغلاق النافذة
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "❌ " + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            var customer = bll.SearchByName(SearchingForPiece.Text);

            if (customer != null)
            {
                CustomerID = customer.CustomerID;

                textBox1.Text = customer.CustomerName;
                textBox2.Text = customer.Phone;
                textBox3.Text = customer.Address;
                textBox4.Text = customer.Notes;
            }
            else
            {
                MessageBox.Show("لم يتم العثور على العميل");
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            var customer = new Models.Customer
            {
                CustomerID = CustomerID,
                CustomerName = textBox1.Text,
                Phone = textBox2.Text,
                Address = textBox3.Text,
                Notes = textBox4.Text
            };

            bll.Update(customer);

            MessageBox.Show("تم التعديل بنجاح");
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            bll.Delete(CustomerID);

            MessageBox.Show("تم الحذف");

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            CustomerID = 0;
        }
    }
}
