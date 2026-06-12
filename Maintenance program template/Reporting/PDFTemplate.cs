using Maintenance_program_template.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Maintenance_program_template.Reporting
{
    public class PDFTemplate : IDocument
    {
        private readonly InvoiceModel Model;

        public PDFTemplate(InvoiceModel model)
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

                    col.Item().PaddingBottom(10).Row(row =>
                    {
                        // ===== LEFT (EN) =====
                        row.RelativeItem().AlignLeft().Column(c =>
                        {
                            c.Spacing(2);

                            c.Item().Text("Center TECHNO CIRCUIT")
                                .Bold().FontSize(11);

                            c.Item().Text("Electronic Engineering Services")
                                .FontSize(9);

                            c.Item().Text("Maintenance: 735745845 / 778055160")
                                .FontSize(8);

                            c.Item().Text("Support: 780080836")
                                .FontSize(8);
                        });

                        // ===== LOGO (CENTER) =====
                        row.ConstantItem(90).Height(90).AlignCenter().AlignMiddle().Element(e =>
                        {
                            try
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Properties.Resources.profile_techno.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    e.Image(ms.ToArray()).FitArea();
                                }
                            }
                            catch
                            {
                                e.Text("LOGO");
                            }
                        });

                        // ===== RIGHT (AR) =====
                        row.RelativeItem().AlignRight().Column(c =>
                        {
                            c.Spacing(2);

                            c.Item().Text("مركز تكنوسركت")
                                .Bold().FontSize(13);

                            c.Item().Text("خدمات الهندسة الإلكترونية")
                                .FontSize(10);

                            c.Item().Text("قسم الصيانة: 735745845")
                                .FontSize(9);

                            c.Item().Text("الدعم الفني: 780080836")
                                .FontSize(9);
                        });
                    });

                    col.Item().LineHorizontal(1);

                    // رقم السند
                    col.Item().AlignRight().Text($"رقم السند: {Model.ReceiptId}")
                        .Bold().FontSize(12);

                    // العنوان
                    col.Item().AlignCenter().Text("سند استلام جهاز")
                        .FontSize(18).Bold();

                    col.Item().LineHorizontal(1);

                    // ===== DATA BOX =====
                    col.Item().Border(1).Padding(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        IContainer Cell(IContainer cellContainer)
                        {
                            return cellContainer
                                .Border(1)
                                .Padding(5)
                                .AlignRight();
                        }

                        // ===== صف 1 =====
                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("اسم العميل: ").Bold().FontSize(9);
                            t.Span(Model.CustomerName).FontSize(9);
                        });

                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("رقم السيريال: ").Bold().FontSize(9);
                            t.Span(Model.SerialNumber).FontSize(9);
                        });

                        // ===== صف 2 =====
                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("نوع المنتج: ").Bold().FontSize(9);
                            t.Span(Model.ProductType).FontSize(9);
                        });

                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("رقم الهاتف: ").Bold().FontSize(9);
                            t.Span(Model.Phone).FontSize(9);
                        });

                        // ===== المشكلة =====
                        table.Cell().ColumnSpan(2)
                            .Element(Cell)
                            .Text(t =>
                            {
                                t.Span("وصف المشكلة: ").Bold().FontSize(9);
                                t.Span(string.IsNullOrWhiteSpace(Model.Problem) ? "لا يوجد" : Model.Problem)
                                    .FontSize(9);
                            });

                        // ===== الموظف =====
                        table.Cell().ColumnSpan(2)
                            .Element(Cell)
                            .Text(t =>
                            {
                                t.Span("الموظف المستلم: ").Bold().FontSize(9);
                                t.Span(Model.ReceivedBy).FontSize(9);
                            });
                    });

                    // ===== DATE =====
                    col.Item().AlignRight().PaddingTop(5).Text(
                        $"تاريخ الاستلام: {Model.ReceiveDate:yyyy/MM/dd HH:mm}"
                    ).Bold();

                    col.Item().LineHorizontal(1);

                    col.Item().PaddingTop(15).Row(row =>
                    {
                        // ===== CUSTOMER SIGNATURE =====
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignCenter().Text("توقيع العميل").Bold().FontSize(10);

                            c.Item()
                                .Border(2) // حدود أقوى
                                .Height(70)
                                .Padding(5)
                                .AlignCenter()
                                .AlignMiddle()
                                .Text("................................")
                                .FontSize(12);
                        });

                        row.ConstantItem(30);

                        // ===== EMPLOYEE SIGNATURE =====
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignCenter().Text("توقيع الموظف").Bold().FontSize(10);

                            c.Item()
                                .Border(2)
                                .Height(70)
                                .Padding(5)
                                .AlignCenter()
                                .AlignMiddle()
                                .Text("................................")
                                .FontSize(12);
                        });
                    });

                    col.Item().PaddingTop(12).LineHorizontal(1.5f);

                    // ===== NOTICE BOX =====
                    col.Item()
                        .Border(1)
                        .Padding(8)
                        .Background(Colors.Grey.Lighten4)
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("إشعار: ").Bold().FontSize(9).FontColor(Colors.Red.Medium);
                            text.Span("المركز غير مسؤول عن الأعطال الناتجة عن سوء الاستخدام بعد الاستلام.\n");
                            text.Span("يرجى مراجعة الجهاز خلال 24 ساعة من الاستلام.");
                        });
                    //.FontSize(9);

                    col.Item().PaddingTop(8).LineHorizontal(1);

                    // ===== FOOTER =====
                    col.Item()
                        .PaddingTop(5)
                        .AlignCenter()
                        .Text("شكراً لاختياركم مركز تكنوسركت 🌹")
                        .Bold()
                        .FontSize(11)
                        .FontColor(Colors.Black);
                });
            });
        }
    }
}
