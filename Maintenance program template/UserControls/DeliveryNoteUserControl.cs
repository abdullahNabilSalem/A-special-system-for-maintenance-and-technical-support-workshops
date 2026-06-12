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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Maintenance_program_template.UserControls
{
    public partial class DeliveryNoteUserControl : UserControl
    {
        private int selectedReceiptID = 0;
        private DeviceReceiptService _service = new DeviceReceiptService();
        private SparePartService _sparePartService = new SparePartService();
        private ReportService _reportService = new ReportService();
        DataTable usedPartsTable = new DataTable();
        public DeliveryNoteUserControl()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DeliveryNoteUserControl_Load(object sender, EventArgs e)
        {
            LoadSpareParts();
            LoadEmployees();

            usedPartsTable.Columns.Add("PartID", typeof(int));
            usedPartsTable.Columns.Add("PartName", typeof(string));
            usedPartsTable.Columns.Add("Quantity", typeof(int));
            usedPartsTable.Columns.Add("Price", typeof(decimal));

            dataGridView2.DataSource = usedPartsTable;
        }
        private void LoadEmployees()
        {
            comboBox2.DataSource = _service.GetEmployees();
            comboBox2.DisplayMember = "EmployeeName";
            comboBox2.ValueMember = "EmployeeID";
            comboBox2.SelectedIndex = -1;
        }
        private void LoadSpareParts()
        {
            try
            {
                DisplayOfPiecesCombox.DataSource = _sparePartService.GetSpareParts();
                DisplayOfPiecesCombox.DisplayMember = "PartName";
                DisplayOfPiecesCombox.ValueMember = "PartID";
                DisplayOfPiecesCombox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل قطع الغيار: " + ex.Message);
            }
        }
        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = textPhone.Text.Trim();

                var result = _service.Search(keyword);

                if (result == null)
                {
                    MessageBox.Show("🚫 لا توجد نتائج");
                    //ClearFields();
                    return;
                }

                selectedReceiptID = result.ReceiptID;

                textPhone.Text = result.PhoneNumber;
                DocumentNumber.Text = result.ReceiptID.ToString();
                textBox4.Text = result.CustomerName;
                textBox3.Text = result.ProblemDescription;
                SerachSerialtxt.Text = result.SerialNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ✔ لا توجد قطع
                if (radioButton1.Checked)
                {
                    usedPartsTable.Clear();

                    usedPartsTable.Rows.Add(DBNull.Value, "لا توجد قطع مستخدمة", 0, 0);
                    return;
                }

                // ✔ تحقق من الاختيار
                if (DisplayOfPiecesCombox.SelectedValue == null)
                {
                    MessageBox.Show("⚠️ الرجاء اختيار القطعة.");
                    return;
                }

                int qty = (int)numericUpDown1.Value;

                if (qty <= 0)
                {
                    MessageBox.Show("⚠️ الرجاء إدخال كمية صحيحة.");
                    return;
                }

                int partID = (int)DisplayOfPiecesCombox.SelectedValue;
                string partName = DisplayOfPiecesCombox.Text;

                decimal price = _sparePartService.GetPartPrice(partID); // 👈 من BLL

                // ✔ منع التكرار (تحسين احترافي)
                foreach (DataRow row in usedPartsTable.Rows)
                {
                    if (row["PartID"] != DBNull.Value && (int)row["PartID"] == partID)
                    {
                        row["Quantity"] = (int)row["Quantity"] + qty;
                        return;
                    }
                }

                usedPartsTable.Rows.Add(partID, partName, qty, price);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var repair = new Repair
                {
                    ReceiptID = selectedReceiptID,
                    EmployeeID = (int)comboBox2.SelectedValue,
                    WorkDescription = textBox6.Text.Trim(),
                    ClientNotes = textBox7.Text.Trim(),
                    RepairCost = decimal.TryParse(textBox8.Text, out var cost) ? cost : 0,
                    PaymentMethod = string.IsNullOrWhiteSpace(comboBox3.Text) ? "نقداً" : comboBox3.Text,
                    WarrantyPeriod = comboBox4.Text,
                    Currency = comboBox5.Text
                };

                // ✔ إضافة القطع
                if (radioButton2.Checked)
                {
                    foreach (DataRow row in usedPartsTable.Rows)
                    {
                        repair.Parts.Add(new RepairPart
                        {
                            PartID = Convert.ToInt32(row["PartID"]),
                            Quantity = Convert.ToInt32(row["Quantity"]),
                            Price = Convert.ToDecimal(row["Price"])
                        });
                    }
                }

                _service.SaveRepair(repair);

                MessageBox.Show("✅ تم الحفظ بنجاح");
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message);
            }
        }
        private void ResetForm()
        {
            // إعادة القيم الأساسية
            selectedReceiptID = 0;

            // TextBoxes
            textPhone.Clear(); // Phone
            textBox2.Clear(); // ReceiptID
            textBox3.Clear(); // Problem
            textBox4.Clear(); // CustomerName
            textBox6.Clear(); // WorkDescription
            textBox7.Clear(); // ClientNotes
            textBox8.Clear(); // RepairCost
            SerachSerialtxt.Clear(); // Serial
            DocumentNumber.Clear(); // DocumentNumber

            // ComboBoxes
            DisplayOfPiecesCombox.SelectedIndex = -1; // Spare Parts
            comboBox2.SelectedIndex = -1; // Employee
            comboBox3.SelectedIndex = -1; // Payment
            comboBox4.SelectedIndex = -1; // Warranty
            comboBox5.SelectedIndex = -1; // Currency

            // RadioButtons
            radioButton1.Checked = true; // لا توجد قطع
            radioButton2.Checked = false;

            // Numeric
            numericUpDown1.Value = 0;

            // جدول القطع
            usedPartsTable.Clear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new DeliveryInvoiceModel
                {
                    ReceiptId = selectedReceiptID,
                    CustomerName = textBox4.Text.Trim(),
                    EmployeeName = comboBox2.Text,
                    RepairCost = decimal.TryParse(textBox8.Text, out var cost) ? cost : 0,
                    PaymentMethod = comboBox3.Text,
                    WorkDescription = textBox6.Text,
                    ClientNotes = textBox7.Text,
                    DeliveryDate = DateTime.Now
                };

                string path = _reportService.GenerateDeliveryPdf(model);

                MessageBox.Show($"✅ تم إنشاء سند التسليم:\n{path}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ خطأ: " + ex.Message);
            }
        }
    }
}
