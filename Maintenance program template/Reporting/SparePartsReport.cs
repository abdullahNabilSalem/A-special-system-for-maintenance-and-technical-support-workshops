using Maintenance_program_template.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace Maintenance_program_template.Reporting
{
    public class SparePartsReport : IDocument
    {
        private readonly List<SparePart> Parts;

        public SparePartsReport(List<SparePart> parts)
        {
            Parts = parts;
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

                    // ================= HEADER (نفس السند) =================
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

                    // ================= TITLE =================
                    col.Item().AlignCenter().Text("تقرير قطع الغيار")
                        .FontSize(18).Bold();

                    col.Item().AlignRight().Text($"التاريخ: {DateTime.Now:yyyy/MM/dd}")
                        .Bold().FontSize(10);

                    col.Item().LineHorizontal(1);

                    // ================= TABLE =================
                    col.Item().Border(1).Padding(8).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(70);
                        });

                        // ===== HEADER STYLE =====
                        IContainer Header(IContainer c)
                        {
                            return c
                                .Border(1)
                                .Background(Colors.Grey.Lighten3)
                                .Padding(5)
                                .AlignCenter()
                                .DefaultTextStyle(x => x.Bold().FontSize(9));
                        }

                        // ===== BODY STYLE =====
                        IContainer Body(IContainer c)
                        {
                            return c
                                .Border(1)
                                .Padding(4)
                                .AlignCenter()
                                .DefaultTextStyle(x => x.FontSize(9));
                        }

                        // ===== HEADER ROW =====
                        table.Cell().Element(Header).Text("ID");
                        table.Cell().Element(Header).Text("اسم القطعة");
                        table.Cell().Element(Header).Text("المورد");
                        table.Cell().Element(Header).Text("الكمية");
                        table.Cell().Element(Header).Text("السعر");

                        // ===== DATA =====
                        foreach (var p in Parts)
                        {
                            table.Cell().Element(Body).Text(p.PartID.ToString());
                            table.Cell().Element(Body).AlignRight().Text(p.PartName);
                            table.Cell().Element(Body).AlignRight().Text(p.Supplier);
                            table.Cell().Element(Body).Text(p.Quantity.ToString());
                            table.Cell().Element(Body).Text(p.Price.ToString("0.00"));
                        }
                    });

                    // ================= SUMMARY BOX =================
                    col.Item().PaddingTop(10).Border(1).Padding(8).Background(Colors.Grey.Lighten4)
                        .AlignRight().Text(text =>
                        {
                            text.Span("إجمالي القطع: ").Bold().FontSize(10);
                            text.Span(Parts.Count.ToString()).FontSize(10);
                        });

                    col.Item().LineHorizontal(1);

                    // ================= FOOTER =================
                    col.Item().AlignCenter().Text(text =>
                    {
                        text.Span("تم إنشاء التقرير بواسطة ").FontSize(8);
                        text.Span("نظام إدارة الصيانة - تكنوسركت").Bold().FontSize(9);
                    });

                    col.Item().AlignCenter().Text("شكراً لاستخدامكم النظام 🌹")
                        .FontSize(10).Bold();
                });
            });
        }
    }
}