using Maintenance_program_template.Models;
using Maintenance_program_template.Properties;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

public class ComplaintPdfTemplate : IDocument
{
    private readonly ComplaintPdfModel Model;

    public ComplaintPdfTemplate(ComplaintPdfModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(20);

            page.Content().Border(1).Padding(15).Column(col =>
            {
                col.Spacing(12);

                // ===== HEADER (نفس سند التسليم) =====
                col.Item().PaddingBottom(10).Row(row =>
                {
                    // LEFT (EN)
                    row.RelativeItem().AlignLeft().Column(c =>
                    {
                        c.Item().Text("Center TECHNO CIRCUIT").Bold().FontSize(11);
                        c.Item().Text("Electronic Engineering Services").FontSize(9);
                        c.Item().Text("Maintenance: 735745845 / 778055160").FontSize(8);
                        c.Item().Text("Support: 780080836").FontSize(8);
                    });

                    // LOGO
                    row.ConstantItem(90).Height(90).AlignCenter().AlignMiddle().Element(e =>
                    {
                        try
                        {
                            using (var ms = new MemoryStream())
                            {
                                Resources.profile_techno.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                e.Image(ms.ToArray()).FitArea();
                            }
                        }
                        catch
                        {
                            e.Text("LOGO");
                        }
                    });

                    // RIGHT (AR)
                    row.RelativeItem().AlignRight().Column(c =>
                    {
                        c.Item().Text("مركز تكنوسركت").Bold().FontSize(13);
                        c.Item().Text("خدمات الهندسة الإلكترونية").FontSize(10);
                        c.Item().Text("قسم الصيانة: 735745845").FontSize(9);
                        c.Item().Text("الدعم الفني: 780080836").FontSize(9);
                    });
                });

                col.Item().LineHorizontal(1);

                // ===== رقم الشكوى =====
                col.Item().AlignRight().Text($"رقم الشكوى: {Model.ComplaintID}")
                    .Bold().FontSize(12);

                // ===== العنوان =====
                col.Item().AlignCenter().Text("نموذج شكوى")
                .Bold()
                .FontSize(18)
                .FontFamily("Arial"); // أو أي خط تريده

                col.Item().LineHorizontal(1);

                // ===== TABLE (نفس أسلوب سند التسليم) =====
                col.Item().Border(1).Padding(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    IContainer Cell(IContainer c)
                        => c.Border(1).Padding(5).AlignRight();

                    void Row(string label, string value)
                    {
                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span($"{label}: ").Bold().FontSize(9);
                            t.Span(string.IsNullOrEmpty(value) ? "-" : value).FontSize(9);
                        });
                    }

                    Row("اسم العميل", Model.CustomerName);
                    Row("رقم الهاتف", Model.PhoneNumber);
                    Row("المنطقة", Model.Region);
                    Row("نوع المنتج", Model.ProductName);
                    Row("الرقم التسلسلي", Model.SerialNumber);
                    Row("نوع المشكلة", Model.ProblemType);
                    Row("حالة الشكوى", Model.ComplaintStatus);
                    Row("الموظف", Model.EmployeeName);

                    // وصف المشكلة (Full Width)
                    table.Cell().ColumnSpan(2)
                        .Element(Cell)
                        .Text(t =>
                        {
                            t.Span("وصف المشكلة: ").Bold().FontSize(9);
                            t.Span(string.IsNullOrWhiteSpace(Model.ProblemDescription)
                                ? "لا يوجد"
                                : Model.ProblemDescription).FontSize(9);
                        });
                });

                // ===== DATE =====
                col.Item().AlignRight().PaddingTop(5)
                    .Text($"التاريخ: {Model.ComplaintDate:yyyy/MM/dd HH:mm}")
                    .Bold();

                col.Item().LineHorizontal(1);

                // ===== SIGNATURES =====
                col.Item().PaddingTop(15).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().Text("توقيع العميل").Bold().FontSize(10);

                        c.Item()
                            .Border(2)
                            .Height(70)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("................................");
                    });

                    row.ConstantItem(30);

                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().Text("توقيع الموظف").Bold().FontSize(10);

                        c.Item()
                            .Border(2)
                            .Height(70)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("................................");
                    });
                });

                col.Item().PaddingTop(12).LineHorizontal(1.5f);

                // ===== NOTICE (احترافي إضافي) =====
                col.Item()
                    .Border(1)
                    .Padding(8)
                    .Background(Colors.Grey.Lighten4)
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("ملاحظة: ").Bold().FontSize(9).FontColor(Colors.Red.Medium);
                        text.Span("سيتم مراجعة الشكوى والرد خلال أقرب وقت ممكن.\n");
                        text.Span("يرجى الاحتفاظ برقم الشكوى للمتابعة.");
                    });

                col.Item().PaddingTop(8).LineHorizontal(1);

                // ===== FOOTER =====
                col.Item()
                    .AlignCenter()
                    .Text("شكراً لاختياركم مركز تكنوسركت 🌹")
                    .Bold()
                    .FontSize(11);
            });
        });
    }
}