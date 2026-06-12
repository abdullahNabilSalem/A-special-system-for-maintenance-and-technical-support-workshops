using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace technicalSupport.Report.ReportTemplates
{
    internal class StandardFooterComponent : IComponent
    {
        public string Address { get; set; } = "توقيع الباحث";
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string BorderColor { get; set; } = Colors.Grey.Lighten3;


        public void Compose(IContainer container)
        {
            container
                .PaddingTop(10) // مسافة بسيطة لفصل التذييل عن محتوى الصفحة
                .CornerRadius(10) // إضافة الحواف الدائرية
                .Background(BorderColor) // رمادي أغمق قليلاً (تدرج 3 أو 2 حسب رغبتك)
                .PaddingVertical(5) // مسافة داخلية للنصوص من الأعلى والأسفل
                .Column(col =>
                {
                    col.Item().AlignCenter().Text($"{Address} - {Name} - تلفون : {Phone}").FontSize(9);

                    col.Item().PaddingTop(5).AlignCenter().Text(x =>
                    {
                        x.Span("صفحة ");
                        x.CurrentPageNumber();
                        x.Span(" من ");
                        x.TotalPages();
                    });

                    col.Item().PaddingTop(5).AlignCenter().Text(t =>
                    {
                        t.Span("أصدرت من نظام ").FontSize(9).FontColor(Colors.Grey.Medium);
                        t.Span("Technical Support").FontSize(9).Bold().FontColor(Colors.Blue.Medium);
                    });
                });
        }
    }
}
