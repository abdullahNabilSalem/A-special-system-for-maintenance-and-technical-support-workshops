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
    public partial class DeferredPayments : Form
    {
        PaymentBLL bll = new PaymentBLL();
        public DeferredPayments()
        {
            InitializeComponent();
        }

        private void DeferredPayments_Load(object sender, EventArgs e)
        {
            LoadCustommersToComboBox();

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
        private void LoadCustommersToComboBox()
        {
            try
            {
                DataTable dt = bll.GetCustomers();

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "CustomerName";
                comboBox1.ValueMember = "CustomerID";
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (!ValidatePayment(out int customerId, out decimal payAmount))
                return;

            try
            {
                bll.PayCredit(customerId, payAmount);

                if (payAmount <= 0)
                {
                    MessageBox.Show("أدخل مبلغ صحيح");
                    return;
                }

                MessageBox.Show("✅ تم تسديد المبلغ بنجاح");

                LoadCustomerInvoices();
                textBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
        private bool ValidatePayment(out int customerId, out decimal payAmount)
        {
            customerId = 0;
            payAmount = 0;

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار عميل أولاً.");
                return false;
            }

            if (!decimal.TryParse(textBox2.Text, out payAmount) || payAmount <= 0)
            {
                MessageBox.Show("يرجى إدخال مبلغ صحيح.");
                return false;
            }

            customerId = Convert.ToInt32(comboBox1.SelectedValue);
            return true;
        }
        private void LoadCustomerInvoices()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار عميل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(comboBox1.SelectedValue);

            try
            {
                var result = bll.GetCustomerInvoices(customerId);

                dataGridView1.DataSource = result.table;
                label3.Text = $"الرصيد المتبقي: {result.totalRemaining:N0} ريال";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار عميل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(comboBox1.SelectedValue);

            try
            {
                var result = bll.GetCustomerCreditReport(customerId);

                dataGridView1.DataSource = result.table;

                // ✅ تنسيق (يبقى في UI فقط)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
                dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 10);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);

                label3.Text = $"💰 الرصيد المتبقي: {result.totalRemaining:N0} ريال";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار عميل أولاً.");
                return;
            }

            int customerId = Convert.ToInt32(comboBox1.SelectedValue);

            DialogResult confirm = MessageBox.Show(
                "هل أنت متأكد أنك تريد تسديد جميع ديون هذا العميل بالكامل؟",
                "تأكيد",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bll.PayAllCustomerDebts(customerId);

                MessageBox.Show("✅ تم تسديد جميع ديون العميل بنجاح");

                LoadCustomerInvoices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى اختيار فاتورة أولاً.");
                return;
            }

            int invoiceId;

            try
            {
                invoiceId = Convert.ToInt32(
                    dataGridView1.SelectedRows[0]
                    .Cells["رقم الفاتورة"].Value);
            }
            catch
            {
                MessageBox.Show("خطأ أثناء قراءة رقم الفاتورة");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "هل تريد تسديد هذه الفاتورة بالكامل؟",
                "تأكيد",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bll.PaySingleInvoice(invoiceId);

                MessageBox.Show("✅ تم تسديد الفاتورة بنجاح");

                LoadCustomerInvoices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
    }
}
