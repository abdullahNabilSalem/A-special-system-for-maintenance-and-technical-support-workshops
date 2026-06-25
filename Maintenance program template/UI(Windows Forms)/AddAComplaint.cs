using Maintenance_program_template.BLL_Business_Logic_Layer_;
using Maintenance_program_template.Models;
using QuestPDF.Fluent;
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
    public partial class AddAComplaint : Form
    {
        ComplaintBLL bll = new ComplaintBLL();
        private ComplaintPdfModel lastComplaint;
        int m = 0;
        public AddAComplaint()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();

        }

        private void btnAddComplaint_Click(object sender, EventArgs e)
        {
            try
            {
                Complaint complaint = new Complaint()
                {
                    CustomerName = txtCustomerName.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    Region = txtRegion.Text,
                    SerialNumber = txtSerialNumber.Text,
                    EmployeeID = cmbEmployee.SelectedValue != null ? (int)cmbEmployee.SelectedValue : 0,
                    ComplaintStatus = cmbComplaintStatus.Text,
                    ProblemDescription = txtProblemDescription.Text,
                    ComplaintDate = dtpComplaintDate.Value,
                    ProductID = cmbProduct.SelectedValue != null ? (int)cmbProduct.SelectedValue : 0,
                    ProblemType = cmbProblemType.Text
                };

                // 🔥 مهم: نحتاج ID بعد الحفظ
                int newId = bll.AddComplaint(complaint);

                // ✅ تجهيز بيانات PDF
                lastComplaint = new ComplaintPdfModel
                {
                    ComplaintID = newId,
                    CustomerName = complaint.CustomerName,
                    PhoneNumber = complaint.PhoneNumber,
                    Region = complaint.Region,
                    SerialNumber = complaint.SerialNumber,
                    ProductName = cmbProduct.Text,
                    ProblemType = complaint.ProblemType,
                    ProblemDescription = complaint.ProblemDescription,
                    EmployeeName = cmbEmployee.Text,
                    ComplaintStatus = complaint.ComplaintStatus,
                    ComplaintDate = complaint.ComplaintDate
                };

                MessageBox.Show("تم إضافة الشكوى بنجاح");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddAComplaint_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadProducts();
        }
        private void LoadEmployees()
        {
            cmbEmployee.DataSource = bll.GetEmployees();
            cmbEmployee.DisplayMember = "EmployeeName";
            cmbEmployee.ValueMember = "EmployeeID";
            cmbEmployee.SelectedIndex = -1;
        }

        private void LoadProducts()
        {
            cmbProduct.DataSource = bll.GetProducts();
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductID";
            cmbProduct.SelectedIndex = -1;
        }

        private void PrintBtnComplaint_Click(object sender, EventArgs e)
        {
            if (lastComplaint == null)
            {
                MessageBox.Show("لا توجد شكوى للطباعة");
                return;
            }

            var model = new ComplaintPdfModel
            {
                ComplaintID = lastComplaint.ComplaintID,
                CustomerName = lastComplaint.CustomerName,
                PhoneNumber = lastComplaint.PhoneNumber,
                Region = lastComplaint.Region,
                SerialNumber = lastComplaint.SerialNumber,
                ProductName = lastComplaint.ProductName,
                ProblemType = lastComplaint.ProblemType,
                ProblemDescription = lastComplaint.ProblemDescription,
                EmployeeName = lastComplaint.EmployeeName,
                ComplaintStatus = "جديدة", // أو من DB
                ComplaintDate = lastComplaint.ComplaintDate
            };

            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "PDF File|*.pdf",
                FileName = $"Complaint_{model.ComplaintID}.pdf"
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
                var document = new ComplaintPdfTemplate(model);
                document.GeneratePdf(save.FileName);

                MessageBox.Show("تم حفظ الملف بنجاح");
            }
        }
    }
}
