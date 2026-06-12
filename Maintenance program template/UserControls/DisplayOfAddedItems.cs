using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maintenance_program_template.UserControls
{
    public partial class DisplayOfAddedItems : UserControl
    {
        private SparePartService _service = new SparePartService();
        private int currentPage = 1;
        private int pageSize = 50;
        public DisplayOfAddedItems()
        {
            InitializeComponent();
        }

        private void DisplayOfAddedItems_Load(object sender, EventArgs e)
        {
            LoadParts();


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
        private void LoadParts()
        {
            try
            {
                dataGridView1.DataSource = _service.GetParts(currentPage, pageSize);
                lblPageInfo.Text = $"الصفحة: {currentPage}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل قطع الغيار: " + ex.Message);
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
    }
}
