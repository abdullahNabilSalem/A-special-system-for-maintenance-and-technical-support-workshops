using Maintenance_program_template.BLL_Business_Logic_Layer_;
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

namespace Maintenance_program_template.UI_Windows_Forms_
{
    public partial class AddedByCompany : Form
    {
        //إنشاء Delegate داخل فورم الإضافة
        public delegate void SupplierAddedHandler();
        public event SupplierAddedHandler OnSupplierAdded;

        private SupplierService _service = new SupplierService();

        private int currentSupplierId = 0;
        public AddedByCompany()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void AddedByCompany_Load(object sender, EventArgs e)
        {

        }

        private void addedBtnCompany_Click(object sender, EventArgs e)
        {
            try
            {
                Supplier supplier = new Supplier
                {
                    SupplierName = textBox2.Text,
                    SupplierPhone = textBox3.Text,
                    Notes = textBox1.Text
                };

                _service.AddSupplier(supplier);

                MessageBox.Show("تم إضافة المورد بنجاح");

                // 🔥 هنا السحر
                OnSupplierAdded?.Invoke();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                var result = _service.SearchSupplier(SearchingForPiece.Text.Trim());

                if (result == null)
                {
                    MessageBox.Show("لم يتم العثور على المورد");
                    return;
                }

                currentSupplierId = result.SupplierID;

                textBox2.Text = result.SupplierName;
                textBox3.Text = result.SupplierPhone;
                textBox1.Text = result.Notes;
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
                Supplier supplier = new Supplier
                {
                    SupplierID = currentSupplierId,
                    SupplierName = textBox2.Text.Trim(),
                    SupplierPhone = textBox3.Text.Trim(),
                    Notes = textBox1.Text.Trim()
                };

                _service.UpdateSupplier(supplier);

                MessageBox.Show("تم التعديل بنجاح");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSupplierId == 0)
                {
                    MessageBox.Show("قم بالبحث عن المورد أولاً");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد من حذف هذا المورد؟",
                    "تأكيد",
                    MessageBoxButtons.YesNo);

                if (confirm != DialogResult.Yes)
                    return;

                _service.DeleteSupplier(currentSupplierId);

                MessageBox.Show("تم الحذف بنجاح");

            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("لا يمكن حذف المورد لأنه مرتبط ببيانات أخرى");
                }
                else
                {
                    MessageBox.Show("خطأ في قاعدة البيانات: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
        }
    }
}
