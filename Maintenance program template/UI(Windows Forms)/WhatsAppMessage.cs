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
    public partial class WhatsAppMessage : Form
    {
        private readonly BLL_Business_Logic_Layer_.DeviceReceiptService _service = new BLL_Business_Logic_Layer_.DeviceReceiptService();
        public WhatsAppMessage()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReceiptID.Text, out int id))
                {
                    MessageBox.Show("أدخل رقم سند صحيح");
                    return;
                }

                DataTable dt = _service.GetReceiptForWhatsApp(id);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("لا يوجد بيانات");
                    return;
                }

                txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                txtPhone.Text = dt.Rows[0]["PhoneNumber"].ToString();
                txtProblem.Text = dt.Rows[0]["ProblemDescription"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string message = $@"مرحباً {txtCustomerName.Text} 👋

            تم الانتهاء من صيانة جهازك بنجاح ✅

            🔧 المشكلة:
            {txtProblem.Text}

            📦 الجهاز جاهز للاستلام من المركز

            نشكركم على ثقتكم بنا 🙏";

            txtMessage.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string phone = txtPhone.Text.Trim();
                string message = txtMessage.Text.Trim();

                if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(message))
                {
                    MessageBox.Show("تحقق من البيانات");
                    return;
                }

                // تنظيف الرقم (إزالة 0 أو +)
                phone = phone.Replace("+", "").Replace(" ", "");

                // تشفير النص
                string encodedMessage = Uri.EscapeDataString(message);

                string url = $"https://wa.me/{phone}?text={encodedMessage}";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }
    }
}
