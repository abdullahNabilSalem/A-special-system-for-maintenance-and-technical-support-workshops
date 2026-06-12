using Maintenance_program_template.BLL_Business_Logic_Layer_;
using Maintenance_program_template.Models;
using Maintenance_program_template.Reporting;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using QuestPDF.Fluent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;

namespace Maintenance_program_template.UserControls
{
    public partial class inventoryOfParts : UserControl
    {
        SupplierBLL supplierBLL = new SupplierBLL();
        SparePartsBLL partsBLL = new SparePartsBLL();

        int currentPage = 1;
        int pageSize = 50;
        public inventoryOfParts()
        {
            InitializeComponent();
        }

        private void inventoryOfParts_Load(object sender, EventArgs e)
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
            LoadParts();
        }
        private void LoadSuppliers()
        {
            filterComSup.DataSource = supplierBLL.GetSuppliers();
            filterComSup.DisplayMember = "SupplierName";
            filterComSup.ValueMember = "SupplierID";
            filterComSup.SelectedIndex = -1;
        }
        private void LoadParts()
        {
            DataTable dt = partsBLL.GetPartsPaged(currentPage, pageSize);
            dataGridView1.DataSource = dt;

            lblPageInfo.Text = $"Page {currentPage}";
        }

        private void filterComSup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void filterOfQuntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ApplyFilters();

            ApplyQuantityFilter();
        }
        private void ApplyQuantityFilter()
        {
            if (dataGridView1.DataSource == null) return;

            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt == null) return;

            string filter = "";

            switch (filterOfQuntity.SelectedIndex)
            {
                case 0: // عرض كل القطع
                    filter = "";
                    break;
                case 1: // أقل من 5
                    filter = "Quantity < 5";
                    break;
                case 2: // نفدت
                    filter = "Quantity = 0";
                    break;
            }

            dt.DefaultView.RowFilter = filter;

            // إعادة تلوين الصفوف بعد الفلترة
            HighlightLowStock();
        }
        private void ApplyFilters()
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt == null) return;

            string supplier = filterComSup.SelectedIndex != -1 ? filterComSup.Text : null;
            int quantityIndex = filterComSup.SelectedIndex;

            string filter = partsBLL.BuildFilter(supplier, quantityIndex);

            dt.DefaultView.RowFilter = filter;

            HighlightLowStock();
        }
        private void HighlightLowStock()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Quantity"].Value == null) continue;

                int qty = Convert.ToInt32(row.Cells["Quantity"].Value);

                if (qty == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.DarkRed;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (qty < 5)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadParts();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadParts();
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            LoadParts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1);
        }
        private void ExportToExcel(DataGridView dgv)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Name = "فاتورة";

                // العناوين
                for (int i = 1; i <= dgv.Columns.Count; i++)
                {
                    worksheet.Cells[1, i] = dgv.Columns[i - 1].HeaderText;
                }

                // البيانات
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        object value = dgv.Rows[i].Cells[j].Value;
                        worksheet.Cells[i + 2, j + 1] = value?.ToString() ?? "";
                    }
                }

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Excel Files|*.xlsx";
                save.Title = "احفظ الملف باسم";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(save.FileName);
                    MessageBox.Show("تم التصدير بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                workbook.Close(false);
                excelApp.Quit();

                // تنظيف
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء التصدير: " + ex.Message);
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            var list = new List<SparePart>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                int partId = 0;
                int qty = 0;
                decimal price = 0;

                int.TryParse(row.Cells["PartID"].Value?.ToString(), out partId);
                int.TryParse(row.Cells["Quantity"].Value?.ToString(), out qty);
                decimal.TryParse(row.Cells["Price"].Value?.ToString(), out price);

                list.Add(new SparePart
                {
                    PartID = partId,
                    PartName = row.Cells["PartName"].Value?.ToString() ?? "",
                    Supplier = row.Cells["Supplier"].Value?.ToString() ?? "",
                    Quantity = qty,
                    Price = price
                });
            }

            var document = new SparePartsReport(list);

            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "SparePartsReport.pdf"
            );

            document.GeneratePdf(path);

            MessageBox.Show("تم إنشاء التقرير على سطح المكتب");
        }
    }
}
