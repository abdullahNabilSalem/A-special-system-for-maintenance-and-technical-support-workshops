using Maintenance_program_template.BLL_Business_Logic_Layer_;
using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using Maintenance_program_template.Reporting;
using Maintenance_program_template.UI_Windows_Forms_;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using technicalSupport;

namespace Maintenance_program_template.UserControls
{
    public partial class FinancialAccounts : UserControl
    {
        string invoiceCurrency = "";
        public FinancialAccounts()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                checkBox1.Checked = false;

            CalculateTotal();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                checkBox2.Checked = false;

            CalculateTotal();
        }
        void CalculateTotal()
        {
            decimal total = 0m;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                if (TryGetDecimal(row.Cells[4]?.Value, out decimal value))
                {
                    total += value;
                }
            }

            bool isCredit = checkBox1.Checked;
            bool isCash = checkBox2.Checked;

            ApplyPaymentMode(total, isCredit, isCash);
        }
        private void ApplyPaymentMode(decimal total, bool isCredit, bool isCash)
        {
            textBox5.BackColor = Color.White;
            textBox6.BackColor = Color.White;

            if (isCredit)
            {
                SetValues(paid: 0m, remaining: total, paidColor: Color.White, remainingColor: Color.LightCoral);
            }
            else if (isCash)
            {
                SetValues(paid: total, remaining: 0m, paidColor: Color.LightGreen, remainingColor: Color.White);
            }
            else
            {
                SetValues(0m, 0m, Color.White, Color.White);
            }
        }
        private void SetValues(decimal paid, decimal remaining, Color paidColor, Color remainingColor)
        {
            textBox6.Text = paid.ToString("0.00");
            textBox5.Text = remaining.ToString("0.00");

            textBox6.BackColor = paidColor;
            textBox5.BackColor = remainingColor;
        }
        private bool TryGetDecimal(object value, out decimal result)
        {
            return decimal.TryParse(Convert.ToString(value), out result);
        }

        private void FinancialAccounts_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            LoadNextInvoiceId();


            //LoadCustommersToComboBox();
        }
        //private void LoadCustommersToComboBox()
        //{
        //    try
        //    {
        //        CustomerBLL bll = new CustomerBLL();

        //        var customers = bll.GetCustomers();

        //        comboBox1.DataSource = customers;
        //        comboBox1.DisplayMember = "CustomerName";
        //        comboBox1.ValueMember = "CustomerID";
        //        comboBox1.SelectedIndex = -1;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("خطأ في تحميل العملاء: " + ex.Message);
        //    }
        //}
        private void InitializeDataGridView()
        {
            if (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.Clear();

            dataGridView1.AutoGenerateColumns = false;

            AddColumn("ProductName", "اسم المنتج");
            AddColumn("ServiceName", "اسم الخدمة");
            AddColumn("Quantity", "الكمية");
            AddColumn("UnitPrice", "السعر الفردي");
            AddColumn("Total", "الإجمالي");
            AddColumn("PaymentStatus", "حالة الدفع");
            AddColumn("Currency", "العملة");
        }
        private void AddColumn(string name, string header)
        {
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name, // مهم جدًا لو ربطت DB لاحقًا
                ReadOnly = true
            });
        }
        private bool ValidateInvoiceBeforeSave()
        {
            if (dataGridView1.Rows.Count <= 1)
            {
                MessageBox.Show("لا يوجد أصناف لحفظ الفاتورة.");
                return false;
            }

            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("يرجى اختيار نوع الفاتورة (كاش أو أجل).");
                return false;
            }

            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("لا يمكن اختيار كاش وأجل معاً.");
                return false;
            }

            if (string.IsNullOrEmpty(invoiceCurrency))
            {
                MessageBox.Show("لم يتم تحديد عملة الفاتورة.");
                return false;
            }

            // ✅ الجديد: تحقق من العميل في حالة الأجل
            if (checkBox1.Checked && comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("يجب اختيار العميل عند الفاتورة الآجلة.");
                return false;
            }

            return true;
        }
        private void SaveTheInvoice_Click(object sender, EventArgs e)
        {
            if (!ValidateInvoiceBeforeSave())
                return;

            try
            {
                Invoice invoice = new Invoice
                {
                    InvoiceDate = dateTimePicker1.Value,
                    CustomerID = checkBox1.Checked && comboBox1.SelectedIndex != -1
                        ? (int?)comboBox1.SelectedValue
                        : null,
                    IsCredit = checkBox1.Checked,
                    Total = GetInvoiceTotalByCurrency(),
                    Paid = SafeDecimal(textBox6.Text),
                    Remain = SafeDecimal(textBox5.Text),
                    Details = GetDetailsFromGrid()
                };

                InvoiceBLL bll = new InvoiceBLL();
                bll.SaveInvoice(invoice);

                MessageBox.Show("✅ تم حفظ الفاتورة بنجاح!");
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }
        private decimal GetInvoiceTotalByCurrency()
        {
            switch (invoiceCurrency)
            {
                case "ريال يمني":
                    return SafeDecimal(textBox7.Text);

                case "ريال سعودي":
                    return SafeDecimal(textBox8.Text);

                case "دولار":
                    return SafeDecimal(textBox9.Text);

                default:
                    return 0m;
            }
        }
        private decimal SafeDecimal(string value)
        {
            return decimal.TryParse(value, out decimal result) ? result : 0m;
        }
        private void ResetForm()
        {
            dataGridView1.Rows.Clear();

            textBox7.Text = "0";
            textBox6.Text = "0";
            textBox5.Text = "0";

            textBox2.Clear();
            textBox3.Clear();

            comboBox2.SelectedIndex = -1;

            invoiceCurrency = ""; // مهم جدًا إعادة العملة

            LoadNextInvoiceId();

            textBox7.Text = "0.00";
            textBox8.Text = "0.00";
            textBox9.Text = "0.00";
        }
        private void LoadNextInvoiceId()
        {
            const string query = "SELECT ISNULL(MAX(InvoiceID), 0) + 1 FROM Invoices";

            try
            {
                using (SqlConnection con = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    object result = cmd.ExecuteScalar();

                    int nextInvoiceId = (result != null && result != DBNull.Value)
                        ? Convert.ToInt32(result)
                        : 1;

                    textBox1.Text = nextInvoiceId.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب رقم الفاتورة:\n" + ex.Message);
                textBox1.Text = "1";
            }
        }
        private List<InvoiceDetail> GetDetailsFromGrid()
        {
            List<InvoiceDetail> list = new List<InvoiceDetail>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                list.Add(new InvoiceDetail
                {
                    ProductName = row.Cells[0].Value?.ToString(),
                    ServiceName = row.Cells[1].Value?.ToString(),
                    Quantity = Convert.ToInt32(row.Cells[2].Value),
                    Price = Convert.ToDecimal(row.Cells[3].Value),
                    TotalLine = Convert.ToDecimal(row.Cells[4].Value),
                    Currency = invoiceCurrency
                });
            }

            return list;
        }
        private bool ValidatePaymentType()
        {
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                ShowWarning("يرجى اختيار نوع الفاتورة (كاش أو أجل).");
                return false;
            }

            if (checkBox1.Checked && checkBox2.Checked)
            {
                ShowWarning("لا يمكن اختيار كاش وأجل معاً.");
                return false;
            }

            return true;
        }
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private bool ValidateProductOrService(out string productName, out string serviceName)
        {
            productName = textBox2.Text.Trim();
            serviceName = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(productName) && string.IsNullOrWhiteSpace(serviceName))
            {
                ShowWarning("يرجى كتابة اسم المنتج أو الخدمة.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(productName) && !string.IsNullOrWhiteSpace(serviceName))
            {
                ShowWarning("يرجى اختيار منتج واحد فقط أو خدمة واحدة فقط.");
                return false;
            }

            return true;
        }
        private bool ValidateQuantity(out int quantity)
        {
            quantity = (int)numericUpDown1.Value;

            if (quantity <= 0)
            {
                ShowWarning("الكمية يجب أن تكون أكثر من 0.");
                return false;
            }

            return true;
        }
        private bool ValidateUnitPrice(out decimal unitPrice)
        {
            if (!decimal.TryParse(textBox4.Text, out unitPrice))
            {
                ShowWarning("يرجى إدخال سعر صحيح.");
                return false;
            }

            return true;
        }
        private bool ValidateCurrency(out string currency)
        {
            currency = comboBox2.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(currency))
            {
                ShowWarning("يرجى اختيار العملة.");
                return false;
            }

            return true;
        }
        private bool ValidateInvoiceCurrency(string currency)
        {
            if (string.IsNullOrEmpty(invoiceCurrency))
            {
                invoiceCurrency = currency;
                return true;
            }

            if (invoiceCurrency != currency)
            {
                ShowWarning("لا يمكن إضافة منتجات بعملات مختلفة داخل نفس الفاتورة.");
                return false;
            }

            return true;
        }
        private void UpdateCurrencyTotals(string currency, decimal amount)
        {
            switch (currency)
            {
                case "ريال يمني":
                    textBox7.Text = (SafeDecimal(textBox7.Text) + amount).ToString("0.00");
                    break;

                case "ريال سعودي":
                    textBox8.Text = (SafeDecimal(textBox8.Text) + amount).ToString("0.00");
                    break;

                case "دولار":
                    textBox9.Text = (SafeDecimal(textBox9.Text) + amount).ToString("0.00");
                    break;
            }
        }
        private void AddRowToGrid(InvoiceDetail detail)
        {
            dataGridView1.Rows.Add(
                string.IsNullOrWhiteSpace(detail.ProductName) ? null : detail.ProductName,
                string.IsNullOrWhiteSpace(detail.ServiceName) ? null : detail.ServiceName,
                detail.Quantity,
                detail.Price.ToString("0.00"),
                detail.TotalLine.ToString("0.00"),
                detail.PaymentStatus,
                detail.Currency
            );
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (!ValidatePaymentType()) return;
            if (!ValidateProductOrService(out string productName, out string serviceName)) return;
            if (!ValidateQuantity(out int quantity)) return;
            if (!ValidateUnitPrice(out decimal unitPrice)) return;
            if (!ValidateCurrency(out string currency)) return;
            if (!ValidateInvoiceCurrency(currency)) return;

            try
            {
                InvoiceBLL bll = new InvoiceBLL();

                var detail = bll.CreateInvoiceDetail(
                    productName,
                    serviceName,
                    quantity,
                    unitPrice,
                    currency,
                    checkBox1.Checked
                );

                UpdateCurrencyTotals(detail.Currency, detail.TotalLine);

                AddRowToGrid(detail);

                CalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
                textBox3.Enabled = false;
            else
                textBox3.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 0)
                textBox2.Enabled = false;
            else
                textBox2.Enabled = true;
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            await GenerateSanadPdf();
        }
        private async Task<string> GenerateSanadPdf(string customPath = null)
        {
            //--------------------------------------------------
            // 1️⃣ التحقق من وجود منتجات
            //--------------------------------------------------
            if (dataGridView1.Rows.Count == 0 ||
               (dataGridView1.Rows.Count == 1 && dataGridView1.Rows[0].IsNewRow))
            {
                MessageBox.Show("لا يمكن حفظ الفاتورة بدون إضافة أي منتجات.",
                    "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            //--------------------------------------------------
            // 2️⃣ تحديد مسار الحفظ
            //--------------------------------------------------
            string filePath = customPath;

            if (string.IsNullOrEmpty(filePath))
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "حفظ التقرير";
                    saveFileDialog.FileName = $"تقرير_مبيعات_{DateTime.Now:yyyyMMddhhmmss}.pdf";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return null;

                    filePath = saveFileDialog.FileName;
                }
            }

            try
            {
                //--------------------------------------------------
                // 3️⃣ الخصم
                //--------------------------------------------------
                decimal discountAmount = decimal.TryParse(textBox8.Text.Trim(), out decimal d) ? d : 0;

                //--------------------------------------------------
                // 4️⃣ استخراج بيانات العميل
                //--------------------------------------------------
                string manualName = textBox10.Text.Trim();

                int? customerId = comboBox1.SelectedValue != null
                    ? Convert.ToInt32(comboBox1.SelectedValue)
                    : (int?)null;

                //--------------------------------------------------
                // 5️⃣ تحديد العميل (منهج واحد فقط)
                //--------------------------------------------------
                CustomerDto customer;

                if (!string.IsNullOrWhiteSpace(manualName))
                {
                    customer = new CustomerDto
                    {
                        Name = manualName
                    };
                }
                else
                {
                    if (customerId.HasValue)
                        customer = await CustomerDto.GetCustomerByIdAsync(customerId);
                    else
                        customer = null;

                    if (customer == null)
                    {
                        customer = new CustomerDto
                        {
                            Name = "عميل نقدي"
                        };
                    }
                }

                //--------------------------------------------------
                // 6️⃣ ملخص الفاتورة
                //--------------------------------------------------
                InvoiceSummaryDto summary = new InvoiceSummaryDto
                {
                    TotalExcludingVat = SafeDecimal(textBox7.Text),
                    ksa = SafeDecimal(textBox8.Text),
                    usd = SafeDecimal(textBox9.Text),
                    PaidAmount = SafeDecimal(textBox6.Text),
                    RemainingAmount = SafeDecimal(textBox5.Text),
                };

                //--------------------------------------------------
                // 7️⃣ عناصر الفاتورة
                //--------------------------------------------------
                List<InvoiceItemDto> items = new List<InvoiceItemDto>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    items.Add(new InvoiceItemDto
                    {
                        //ItemCode = row.Cells["ItemNumber"].Value?.ToString() ?? "",
                        ProductName = row.Cells["ProductName"].Value?.ToString() ?? "-",
                        ServiceName = row.Cells["ServiceName"].Value?.ToString() ?? "-",
                        Quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int q) ? q : 0,
                        UnitPrice = decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal p) ? p : 0,
                        Total = decimal.TryParse(row.Cells["Total"].Value?.ToString(), out decimal t) ? t : 0,
                        PaymentStatus = row.Cells["PaymentStatus"].Value?.ToString() ?? "",
                        currency = row.Cells["currency"].Value?.ToString() ?? ""
                    });
                }

                //--------------------------------------------------
                // 8️⃣ بناء الموديل الكامل (إجباري)
                //--------------------------------------------------
                InvoiceDto reportData = new InvoiceDto
                {
                    Header = new CompanyInfo
                    {
                        Name = "مركز تكنوسركت\n" +
                            "مركز متخصص في تقديم\n" +
                            "خدمات الهندسة الإلكترونية\n" +
                            "قسم الصيانة:\n" +
                            "735745845 && 778055160\n" +
                            "الدعم الفني: 780080836",
                        Address = "",
                        Phone = "",
                        EnglishName = "Center TECHNO CIRCUIT\n" +
                                    "A center specializing in providing\n" +
                                    "electronic engineering services\n" +
                                    "Maintenance: 735745845 && 778055160\n" +
                                    "Technical Support: 780080836",
                        Logo = ImageToByteArray(Properties.Resources.profile_techno_removebg_preview)
                    },
                    Invoice = new InvoiceInfoDto(),
                    Customer = customer,
                    Items = items,
                    Summary = summary,
                    paymentType = checkBox1.Checked
                };

                //--------------------------------------------------
                // 9️⃣ إنشاء التقرير
                //--------------------------------------------------
                var document = new SalesReportSimple(
                    "فاتورة مبيعات && خدمات",
                    reportData,
                    summary,
                    Convert.ToInt32(textBox1.Text),
                    dateTimePicker1.Value,
                    items,
                    discountAmount,
                    checkBox1.Checked
                );

                document.GeneratePdf(filePath);

                //--------------------------------------------------
                // 🔟 رسالة النجاح
                //--------------------------------------------------
                if (customPath == null)
                {
                    MessageBox.Show("تم حفظ الفاتورة بنجاح ✅");
                }

                return filePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ:\n" + ex.Message);
                return null;
            }
        }
        private static byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FrmCustomerLookup frm = new FrmCustomerLookup();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // نضيف العميل المختار فقط إلى الـ ComboBox
                    var selectedCustomer = new List<Customer>
            {
                new Customer
                {
                    CustomerID = frm.SelectedCustomerID,
                    CustomerName = frm.SelectedCustomerName
                }
            };

                    comboBox1.DataSource = null;
                    comboBox1.DataSource = selectedCustomer;
                    comboBox1.DisplayMember = "CustomerName";
                    comboBox1.ValueMember = "CustomerID";
                    comboBox1.SelectedIndex = 0;
                }
            }
        }
    }
}
