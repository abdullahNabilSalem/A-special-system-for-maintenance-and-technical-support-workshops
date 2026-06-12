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
    public partial class FinancialReportForm : Form
    {
        ReportBLL bll = new ReportBLL();
        public FinancialReportForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FinancialReport_Load(object sender, EventArgs e)
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
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadReports();
        }
        private void LoadReports()
        {
            try
            {
                var data = bll.GetFinancialReports(
                    dateTimePicker1.Value,
                    dateTimePicker2.Value
                );

                dataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل التقارير: " + ex.Message);
            }
        }
        private void LoadCreditReports()
        {
            try
            {
                DataTable dt = bll.GetCreditReports(
                    dateTimePicker1.Value.Date,
                    dateTimePicker2.Value.Date
                );

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LoadCreditReports();
        }
    }
}
