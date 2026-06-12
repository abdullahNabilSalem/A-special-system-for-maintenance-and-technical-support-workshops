using QuestPDF.Helpers;
using System;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Maintenance_program_template.Models;

namespace technicalSupport.Report.ReportTemplates
{
    internal class StandardHeaderComponent : IComponent
    {
        public CompanyInfo Header { get; set; }

        public float MainPadding { get; set; } = 10;
        public float CornerRadius { get; set; } = 10;
        public float BorderThickness { get; set; } = 0.5f;
        public float LogoBoxWidth { get; set; } = 120;
        public float LogoBoxHeight { get; set; } = 60;

        // ==========================================
        // إعدادات الخطوط (Typography)
        // ==========================================
        public float TitleFontSize { get; set; } = 12;
        public float DetailsFontSize { get; set; } = 9;
        public float CenterBoxFontSize { get; set; } = 18;

        // ==========================================
        // إعدادات الألوان (Colors)
        // ==========================================
        public string HeaderBackgroundColor { get; set; } = Colors.Grey.Lighten4;
        public string CenterBoxBackgroundColor { get; set; } = Colors.Blue.Lighten1;
        public string BorderColor { get; set; } = Colors.Grey.Lighten1;
        public string CenterBoxFontColor { get; set; } = Colors.White;



        // =========================
        // LOGO
        // =========================
        public static byte[] HexToBytes(string hex)
        {
            if (hex.StartsWith("0x"))
                hex = hex.Substring(2);

            int length = hex.Length / 2;
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
        public void Compose(IContainer container)
        {
            if (Header == null) return;

            container.PaddingBottom(5)
                .CornerRadius(10)
                .Background(HeaderBackgroundColor)
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Row(row =>
                {
                    // القسم الأيمن (عربي)
                    row.RelativeItem().Padding(10).AlignRight().Element(ComposeArabicSide);

                    // القسم الأوسط (المربع الملون)
                    row.ConstantItem(120).AlignCenter().AlignMiddle().Element(ComposeCenterBox);

                    // القسم الأيسر (إنجليزي)
                    row.RelativeItem().Padding(10).AlignLeft().Element(ComposeEnglishSide);
                });
        }

        // --- المكونات الفرعية (Sub-Elements) ---

        // 1️⃣ مكون البيانات العربية
        private void ComposeArabicSide(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text(Header.Name).Bold().FontSize(12).AlignRight();
                // col.Item().Text($"الرقم الضريبي : {Header.VATNumber}").FontSize(9).AlignRight();
                //col.Item().Text($"س . ت : {Header.CRNumber}").FontSize(9).AlignRight();
                //col.Item().Text($"تلفون : {Header.Phone}").FontSize(9).AlignRight();
                col.Item().Text(Header.Email).FontSize(9).AlignRight();
                col.Item().Text(Header.Address).FontSize(9).AlignRight();
            });
        }

        // 2️⃣ مكون المربع الأوسط (اللوجو أو اسم المطعم)
        private void ComposeCenterBox(IContainer container)
        {
            container.Column(col =>
            {
                if (Header.Logo != null && Header.Logo.Length > 0)
                {
                    col.Item().Padding(5).Image(Header.Logo);
                }
                else
                {
                    // تقسيم الاسم لنصين فوق بعض كما في تصميمك
                    var nameParts = Header.Name.Split(' ');
                    string firstPart = nameParts[0];
                    string secondPart = nameParts.Length > 1 ? nameParts[1] : "";

                    col.Item().Text(firstPart).Bold().FontSize(20).FontColor(Colors.White).AlignCenter();
                    if (!string.IsNullOrEmpty(secondPart))
                        col.Item().Text(secondPart).Bold().FontSize(20).FontColor(Colors.White).AlignCenter();
                }
            });
        }

        // 3️⃣ مكون البيانات الإنجليزية
        private void ComposeEnglishSide(IContainer container)
        {
            container.ContentFromLeftToRight().Column(engCol =>
            {
                engCol.Item().Text(Header.EnglishName).Bold().FontSize(12).AlignLeft();
                // engCol.Item().Text($"Tax ID : {Header.VATNumber}").FontSize(9).AlignLeft();
                // engCol.Item().Text($"Reg. No : {Header.CRNumber}").FontSize(9).AlignLeft();
                //engCol.Item().Text($"Phone : {Header.Phone}").FontSize(9).AlignLeft();
                engCol.Item().Text(Header.Email).FontSize(9).AlignLeft();
                engCol.Item().Text(Header.Address).FontSize(9).AlignLeft();
            });
        }
    }
}
