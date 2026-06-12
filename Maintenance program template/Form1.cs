using Maintenance_program_template.UI_Windows_Forms_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maintenance_program_template
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void الخروجمنالنظامToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "هل تريد فعلاً الخروج من النظام؟",
            "تأكيد الخروج",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // يخرج من التطبيق
            }
            else
            {
                // لا شيء، المستخدم اختار "لا"
                MessageBox.Show("تم إلغاء عملية الخروج.", "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void أضافةقطعةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenPage(new UseControls.AddPiece(), "إضافة قطعة");

            Forms.AddAPiece addAPieceForm = new Forms.AddAPiece();
            addAPieceForm.ShowDialog();
        }
        private void OpenPage(UserControl control, string title)
        {
            // إذا الصفحة موجودة مسبقًا
            foreach (Control c in panelMain.Controls)
            {
                if (c.GetType() == control.GetType())
                {
                    foreach (Control cc in panelMain.Controls)
                        cc.Visible = false;

                    c.Visible = true;
                    return;
                }
            }

            // إخفاء الكل
            foreach (Control c in panelMain.Controls)
                c.Visible = false;

            control.Dock = DockStyle.Fill;
            panelMain.Controls.Add(control);
            control.Visible = true;

            AddTab(title, control);
        }
        private void AddTab(string title, UserControl control)
        {
            // إنشاء التاب
            Panel tab = new Panel();
            tab.Width = 160;
            tab.Height = 32;
            tab.Tag = control;

            // 🔥 تحسين اللون (أوضح من السابق)
            tab.BackColor = Color.FromArgb(225, 225, 225);

            tab.Padding = new Padding(5, 0, 0, 0);
            tab.Margin = new Padding(2);

            // عنوان التاب
            Label lbl = new Label();
            lbl.Text = title;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.ForeColor = Color.Black;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lbl.Cursor = Cursors.Hand;

            // زر الإغلاق ❌
            Button btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.Width = 30;
            btnClose.Dock = DockStyle.Right;

            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;

            // 🔥 تحسين وضوح الزر بدل الشفافية (WinForms يعاني منها)
            btnClose.BackColor = Color.FromArgb(225, 225, 225);
            btnClose.ForeColor = Color.Gray;
            btnClose.Cursor = Cursors.Hand;

            // Hover Effect 🔥
            btnClose.MouseEnter += (s, e) =>
            {
                btnClose.BackColor = Color.Red;
                btnClose.ForeColor = Color.White;
            };

            btnClose.MouseLeave += (s, e) =>
            {
                btnClose.BackColor = Color.FromArgb(225, 225, 225);
                btnClose.ForeColor = Color.Gray;
            };

            // التنقل بين التابات
            tab.Click += (s, e) => ActivateTab(tab);
            lbl.Click += (s, e) => ActivateTab(tab);

            // إغلاق التاب
            btnClose.Click += (s, e) =>
            {
                TabBarFlowLPanel.Controls.Remove(tab);
                panelMain.Controls.Remove(control);
                control.Dispose();

                // عرض آخر صفحة مفتوحة
                if (panelMain.Controls.Count > 0)
                    panelMain.Controls[panelMain.Controls.Count - 1].Visible = true;
            };

            // ترتيب العناصر
            tab.Controls.Add(lbl);
            tab.Controls.Add(btnClose);

            // إضافة التاب
            TabBarFlowLPanel.Controls.Add(tab);
        }
        private void ActivateTab(Panel tab)
        {
            UserControl control = (UserControl)tab.Tag;

            // إخفاء الكل
            foreach (Control c in panelMain.Controls)
                c.Visible = false;

            // عرض المطلوب
            control.Visible = true;

            // تحسين اللون (بدون تعقيد)
            foreach (Panel t in TabBarFlowLPanel.Controls)
                t.BackColor = Color.FromArgb(225, 225, 225);

            tab.BackColor = Color.White;
        }

        private void عرضالقطعالمضافةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.DisplayOfAddedItems(), "عرض القطع المضافة");
        }
        DashboardSidebar dashboard;
        private void Form1_Load(object sender, EventArgs e)
        {
            TabBarFlowLPanel.FlowDirection = FlowDirection.LeftToRight;
            TabBarFlowLPanel.WrapContents = false;
            TabBarFlowLPanel.AutoScroll = true;


            menuStrip1.BackColor = Color.FromArgb(245, 245, 245);
            menuStrip1.ForeColor = Color.Black;
            menuStrip1.RenderMode = ToolStripRenderMode.System;
            menuStrip1.GripStyle = ToolStripGripStyle.Hidden;
            menuStrip1.Padding = new Padding(6, 3, 6, 3);

            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CustomMenuColorTable());

            statisticsPanel.BackColor = Color.FromArgb(250, 250, 250);
            statisticsPanel.Padding = new Padding(10);
            statisticsPanel.AutoScroll = true;

            dashboard = new DashboardSidebar(statisticsPanel);
            dashboard.Load();
        }
        class CustomMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
                => Color.FromArgb(220, 220, 220);

            public override Color MenuItemBorder
                => Color.Transparent;

            public override Color MenuItemSelectedGradientBegin
                => Color.FromArgb(230, 230, 230);

            public override Color MenuItemSelectedGradientEnd
                => Color.FromArgb(230, 230, 230);

            public override Color ToolStripDropDownBackground
                => Color.White;

            public override Color ImageMarginGradientBegin
                => Color.White;

            public override Color ImageMarginGradientMiddle
                => Color.White;

            public override Color ImageMarginGradientEnd
                => Color.White;
        }

        private void شToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.Suppliers(), "عرض الشركات الموردة");
        }

        private void قطعسندأستلامToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Receipt receipt = new Receipt();
            receipt.ShowDialog();
        }

        private void عرضسنداتالأستلامToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.DisplayReceipts(), "عرض سندات الأستلام");
        }

        private void قطعسندتسليمToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.DeliveryNoteUserControl(), "قطع سند أستلام");
        }

        private void نظامالأخطاءToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void عرضسنداتالتسليمToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.deliveryReceipts(), "عرض سندات التسليم");
        }

        private void رسالةواتسأبلأستلامالجهازToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WhatsAppMessage message = new WhatsAppMessage();
            message.ShowDialog();
        }

        private void الجردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.inventoryOfParts(), "جرد القطع");
        }

        private void الأجهزةالمستلمةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.ReceivedDevices(), "الأجهزة المستلمة");
        }

        private void الأجهزةالمسلمةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.DevicesThatWereDelivered(), "الأجهزة المسلمة");
        }

        private void تقريرالمخزونToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.Inventory(), "تقرير المخزون");
        }

        private void تقريرالموظفينToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.StaffReport(), "تقرير الموظفين");
        }

        private void تقريرالشكاوىToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.ComplaintReports(), "شكاوي العملاء");
        }

        private void تقريرعميلمفصلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("قيد التجهيز بعد قسم النظام المحاسبي سيتم العمل على هذا النقطة");
            //OpenPage(new UserControls.CustomerReport (), "تقرير عميل");
        }

        private void عرضالشكاويToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.ComplaintsDisplay(), "عرض الشكاوي");
        }

        private void أضافةشكؤىToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAComplaint add = new AddAComplaint();
            add.ShowDialog();
        }

        private void صفحةالمبيعاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.FinancialAccounts(), "صفحة المبيعات");
        }

        private void التقاريرالماليةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinancialReportForm financialReport = new FinancialReportForm();
            financialReport.ShowDialog();
        }

        private void الحساباتالاجلةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeferredPayments deferredPayments = new DeferredPayments();
            deferredPayments.ShowDialog();
        }

        private void أضافةعميلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.AddClient(), "صفحة العملاء");
        }

        private void صفحةالفواتيرToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void سجلالمدفوعاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.PaymentRecord(), "سجل المدفوعات");
        }

        private void بانتظارردالعميلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.CustomerResponse(), "أنتظار رد العميل");
        }

        private void جاريالأجهزةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.OngoingDevices(), "جاري العمل");
        }

        private void الأجهزةالجاهزةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.ReadyMadeDevices(), "الأجهزة الجاهزة");
        }

        private void صفحةالضماناتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPage(new UserControls.GuaranteesPage(), "صفحة الضمانات");
        }
    }
}
