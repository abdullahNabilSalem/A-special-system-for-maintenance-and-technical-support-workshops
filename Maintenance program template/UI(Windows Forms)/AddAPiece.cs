using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maintenance_program_template.Forms
{
    public partial class AddAPiece : Form
    {
        private SparePartService _service = new SparePartService();
        public AddAPiece()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddSparePart();
        }
        private void AddSparePart()
        {
            try
            {
                decimal price;

                if (!decimal.TryParse(textBox3.Text, out price))
                {
                    MessageBox.Show("سعر غير صحيح");
                    return;
                }

                _service.AddSparePart(
                    textBox2.Text.Trim(),
                    (int)numericUpDown1.Value,
                    comboBox3.Text.Trim(),
                    price
                );

                MessageBox.Show("تم إضافة القطعة بنجاح");

                DialogResult result = MessageBox.Show(
                    "هل تريد إضافة قطعة أخرى؟",
                    "تأكيد",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    ClearFields();
                    GetNextPartId();
                }
                else
                {
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetNextPartId()
        {
            int nextId = _service.GetNextPartId();
            textBox1.Text = nextId.ToString();
        }

        private void AddAPiece_Load(object sender, EventArgs e)
        {
            textBox1.Text = _service.GetNextPartId().ToString();

            LoadSuppliers();
        }
        private void ClearFields()
        {
            textBox2.Clear(); // اسم القطعة
            numericUpDown1.Value = 0;
            textBox3.Clear(); // السعر
            comboBox3.SelectedIndex = -1; // الشركة
        }
        private void LoadSuppliers()
        {
            try
            {
                comboBox3.DataSource = _service.GetSuppliers();
                comboBox3.DisplayMember = "SupplierName";
                comboBox3.ValueMember = "SupplierID";
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الموردين: " + ex.Message);
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                var result = _service.SearchPiece(SearchingForPiece.Text.Trim());

                if (result == null)
                {
                    MessageBox.Show("لم يتم العثور على القطعة");
                    return;
                }

                // تعبئة البيانات
                textBox1.Text = result.PartID.ToString();
                textBox2.Text = result.PartName;
                numericUpDown1.Value = result.Quantity;
                comboBox3.Text = result.Supplier;
                textBox3.Text = result.Price.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء البحث: " + ex.Message);
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SparePart part = new SparePart
                {
                    PartID = int.Parse(textBox1.Text),
                    PartName = textBox2.Text,
                    Quantity = (int)numericUpDown1.Value,
                    Supplier = comboBox3.Text,
                    Price = decimal.Parse(textBox3.Text)
                };

                _service.UpdatePart(part);

                MessageBox.Show("تم التعديل بنجاح");

            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء التعديل: " + ex.Message);
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox1.Text);

                // تأكيد من المستخدم
                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد من حذف هذه القطعة؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                // التحقق من الارتباط
                //if (_service.IsPartUsed(id))
                //{
                //    MessageBox.Show(
                //        "لا يمكن حذف هذه القطعة لأنها مرتبطة بسجلات أخرى",
                //        "رفض الحذف",
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);

                //    return;
                //}

                // تنفيذ الحذف
                _service.DeletePart(id);

                MessageBox.Show("تم الحذف بنجاح");

                ClearFields();
            }
            catch (SqlException ex)
            {
                // رقم الخطأ 547 = Foreign Key
                if (ex.Number == 547)
                {
                    MessageBox.Show(
                        "لا يمكن حذف هذه القطعة لأنها مرتبطة بسجلات أخرى",
                        "رفض الحذف",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("خطأ في قاعدة البيانات: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء الحذف: " + ex.Message);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
