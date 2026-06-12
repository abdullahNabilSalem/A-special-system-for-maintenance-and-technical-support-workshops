using Maintenance_program_template.Models;
using System.IO;
using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Maintenance_program_template.Reporting
{
    public class DeliveryPDFTemplate : IDocument
    {
        private readonly DeliveryInvoiceModel Model;

        public DeliveryPDFTemplate(DeliveryInvoiceModel model)
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

                    // ===== HEADER =====
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
                                    Properties.Resources.profile_techno.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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

                    // رقم السند
                    col.Item().AlignRight().Text($"رقم السند: {Model.ReceiptId}")
                        .Bold().FontSize(12);

                    // العنوان
                    col.Item().AlignCenter().Text("سند تسليم جهاز")
                        .FontSize(18).Bold();

                    col.Item().LineHorizontal(1);

                    // ===== DATA TABLE =====
                    col.Item().Border(1).Padding(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        IContainer Cell(IContainer c)
                        {
                            return c.Border(1).Padding(5).AlignRight();
                        }

                        // صف 1
                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("اسم العميل: ").Bold().FontSize(9);
                            t.Span(Model.CustomerName ?? "-").FontSize(9);
                        });

                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("الموظف: ").Bold().FontSize(9);
                            t.Span(Model.EmployeeName ?? "-").FontSize(9);
                        });

                        // صف 2
                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("التكلفة: ").Bold().FontSize(9);
                            t.Span($"{Model.RepairCost}").FontSize(9);
                        });

                        table.Cell().Element(Cell).Text(t =>
                        {
                            t.Span("طريقة الدفع: ").Bold().FontSize(9);
                            t.Span(Model.PaymentMethod ?? "-").FontSize(9);
                        });

                        // وصف العمل
                        table.Cell().ColumnSpan(2)
                            .Element(Cell)
                            .Text(t =>
                            {
                                t.Span("وصف العمل: ").Bold().FontSize(9);
                                t.Span(Model.WorkDescription ?? "لا يوجد").FontSize(9);
                            });

                        // ملاحظات العميل
                        table.Cell().ColumnSpan(2)
                            .Element(Cell)
                            .Text(t =>
                            {
                                t.Span("ملاحظات العميل: ").Bold().FontSize(9);
                                t.Span(Model.ClientNotes ?? "لا يوجد").FontSize(9);
                            });
                    });

                    // ===== DATE =====
                    col.Item().AlignRight().PaddingTop(5)
                        .Text($"تاريخ التسليم: {Model.DeliveryDate:yyyy/MM/dd HH:mm}")
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

                    // ===== NOTICE =====
                    col.Item()
                        .Border(1)
                        .Padding(8)
                        .Background(Colors.Grey.Lighten4)
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("إشعار: ").Bold().FontSize(9).FontColor(Colors.Red.Medium);
                            text.Span("تم تسليم الجهاز بحالة سليمة بعد الفحص.\n");
                            text.Span("لا يتحمل المركز أي مسؤولية بعد خروج الجهاز من المحل.");
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
}