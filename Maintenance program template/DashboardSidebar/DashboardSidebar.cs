using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

public class DashboardSidebar
{
    private Panel container;

    public DashboardSidebar(Panel statisticsPanel)
    {
        container = statisticsPanel;
        container.Controls.Clear();

        container.BackColor = Color.FromArgb(245, 245, 245);
        container.AutoScroll = true;
        container.Padding = new Padding(5);
    }

    public void Load()
    {
        AddCard("طلبات اليوم", "25", Color.DodgerBlue);
        AddCard("قيد العمل", "12", Color.Orange);
        AddCard("مكتملة", "40", Color.SeaGreen);

        AddMiniChart();
        AddProgressCircle("نسبة الإنجاز", 75);
    }

    // 📦 Card بسيط
    private void AddCard(string title, string value, Color color)
    {
        Panel card = new Panel();
        card.Size = new Size(180, 60);
        card.Margin = new Padding(25);
        card.BackColor = Color.White;

        Label lblValue = new Label();
        lblValue.Text = value;
        lblValue.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        lblValue.ForeColor = color;
        lblValue.Location = new Point(10, 10);

        Label lblTitle = new Label();
        lblTitle.Text = title;
        lblTitle.Font = new Font("Segoe UI", 8);
        lblTitle.ForeColor = Color.Gray;
        lblTitle.Location = new Point(10, 40);

        card.Controls.Add(lblValue);
        card.Controls.Add(lblTitle);

        container.Controls.Add(card);
    }

    private void AddMiniChart()
    {
        Chart chart = new Chart();
        chart.Size = new Size(180, 600); // 👈 أصغر شوي عشان يناسب sidebar
        chart.Margin = new Padding(5);
        chart.BackColor = Color.White;

        ChartArea area = new ChartArea();
        area.BackColor = Color.White;

        // 🔥 تنظيف الشكل (مهم جدًا للـ sidebar)
        area.AxisX.MajorGrid.Enabled = false;
        area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
        area.AxisX.LineColor = Color.Transparent;
        area.AxisY.LineColor = Color.Transparent;

        area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7);
        area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7);

        chart.ChartAreas.Add(area);

        Series series = new Series();
        series.ChartType = SeriesChartType.Column;

        // 🔥 لون أنظف واحترافي
        series.Color = Color.FromArgb(0, 120, 215);

        series.IsValueShownAsLabel = false; // 👈 مهم: نشيل الأرقام عشان ما يزحم

        series.Points.AddXY("S", 5);
        series.Points.AddXY("S", 8);
        series.Points.AddXY("M", 3);
        series.Points.AddXY("T", 7);
        series.Points.AddXY("W", 6);

        chart.Series.Add(series);

        container.Controls.Add(chart);
    }

    // 🔵 دائرة بسيطة بدون مكتبات
    private void AddProgressCircle(string title, int percent)
    {
        Panel circle = new Panel();
        circle.Size = new Size(180, 180);
        circle.Margin = new Padding(5);
        circle.BackColor = Color.White;

        circle.Paint += (s, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(20, 20, 140, 140);

            using (Pen bg = new Pen(Color.LightGray, 10))
                g.DrawArc(bg, rect, 0, 360);

            using (Pen p = new Pen(Color.DodgerBlue, 10))
                g.DrawArc(p, rect, -90, (percent / 100f) * 360);

            using (Font f = new Font("Segoe UI", 14, FontStyle.Bold))
            using (Brush b = new SolidBrush(Color.Black))
            {
                string text = percent + "%";
                SizeF size = g.MeasureString(text, f);
                g.DrawString(text, f, b,
                    (circle.Width - size.Width) / 2,
                    (circle.Height - size.Height) / 2);
            }

            using (Font f2 = new Font("Segoe UI", 9))
            using (Brush b2 = new SolidBrush(Color.Gray))
            {
                SizeF size2 = g.MeasureString(title, f2);
                g.DrawString(title, f2, b2,
                    (circle.Width - size2.Width) / 2,
                    140);
            }
        };

        container.Controls.Add(circle);
    }
}