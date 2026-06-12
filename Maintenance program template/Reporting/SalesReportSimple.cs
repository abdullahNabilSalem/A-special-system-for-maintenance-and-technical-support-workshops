using Maintenance_program_template.Models;
using Maintenance_program_template.Reporting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport.Report.ReportTemplates;

namespace Maintenance_program_template.Reporting
{
    internal class SalesReportSimple : IDocument
    {
        //private bool isSale = false;
        private string title = "فاتورة مبيعات 💰";
        private readonly InvoiceDto _model;
        private decimal discountAmount = 0;
        private bool paymentType = false;
        private int InvoiceID = -1;
        private DateTime InvoiceDate;
        private InvoiceSummaryDto invoiceSummaryDto;
        private List<InvoiceItemDto> Items;
        private decimal YERCurrency = 0;
        private decimal KSACurrency = 0;
        private decimal USDCurrency = 0;
        private decimal[] curr;


        public SalesReportSimple(string title, InvoiceDto model, InvoiceSummaryDto invoiceSummaryDto, int InvoiceID, DateTime InvoiceDate, List<InvoiceItemDto> Items, decimal discountAmount = 0, bool paymentType = false)
        {
            this.title = title;
            this._model = model;
            this.discountAmount = discountAmount;
            this.paymentType = paymentType;
            this.invoiceSummaryDto = invoiceSummaryDto;
            this.InvoiceID = InvoiceID;
            this.InvoiceDate = InvoiceDate;
            this.Items = Items;

            curr = new decimal[3];

            curr[0] = invoiceSummaryDto.TotalExcludingVat;
            curr[1] = invoiceSummaryDto.ksa;
            curr[2] = invoiceSummaryDto.usd;
        }


        public DocumentMetadata GetMetadata() => new DocumentMetadata
        {
            Title = "Receipt Voucher",
            Author = "technicalSupport System",
            Subject = "Financial Documents",
            Keywords = "Sales, Invoice, BusinessMaster"
        };

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.ContentFromRightToLeft();
                // تعيين خط يدعم العربية، تأكد من تثبيته في نظامك أو إرفاقه
                page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                //page.Header().Element(ComposeHeader);
                // Replace this line:
                page.Header().Component(new StandardHeaderComponent
                {
                    Header = _model.Header,
                    TitleFontSize = 12,
                    DetailsFontSize = 9,
                    CenterBoxBackgroundColor = Colors.Brown.Lighten1,
                    HeaderBackgroundColor = Colors.Grey.Lighten4
                });
                page.Content().Element(ComposeContent);

                page.Footer().Component(new StandardFooterComponent
                {
                    Address = _model.Header.Address, // نمرر بيانات الفوتر من الموديل الرئيسي
                    Name = _model.Header.Name,
                    Phone = _model.Header.Phone
                });
            });
        }


        void ComposeContent(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().PaddingBottom(10).Text(title).FontSize(14).Bold().AlignCenter().FontColor(Colors.Green.Darken2);

                // جدول معلومات الفاتورة العلوي
                col.Item().PaddingBottom(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        // 4 أعمدة متساوية
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    // ================= الصف الأول =================
                    table.Cell().Element(HeaderBlock).Text("رقم الفاتورة").Bold().AlignCenter();
                    table.Cell().Element(ValueBlock).Text($"{InvoiceID}").Bold().AlignCenter().FontColor(Colors.Red.Darken2);
                    table.Cell().Element(HeaderBlock).Text("Invoice Number").Bold().AlignCenter();
                    // تمت إزالة ColumnSpan لتتناسب الخلية مع العمود الرابع مباشرة
                    table.Cell().Element(HeaderBlock).Text("فاتورة ضريبية مبسطة").Bold().AlignCenter();

                    // ================= الصف الثاني =================
                    table.Cell().Element(HeaderBlock).Text("تاريخ الفاتورة").Bold().AlignCenter();
                    table.Cell().Element(ValueBlock).Text($"{InvoiceDate:dd-MM-yyyy}").Bold().AlignCenter().FontColor(Colors.Red.Darken2);
                    table.Cell().Element(HeaderBlock).Text("Invoice Issue Date").Bold().AlignCenter();
                    // تمت إزالة ColumnSpan هنا أيضاً
                    table.Cell().Element(HeaderBlock).Text($"فاتورة مبيعات {(paymentType == true ? "اجلة" : "")}").Bold().AlignCenter();

                    // ================= الصف الثالث (بيانات العميل) =================
                    table.Cell().Element(HeaderBlock).Text("إسم العميل").Bold().AlignCenter();
                    table.Cell().Element(ValueBlock).Text($"{_model.Customer.Name}").Bold().AlignCenter().FontColor(Colors.Red.Darken2);
                    table.Cell().Element(HeaderBlock).Text("رقم العميل").Bold().AlignCenter();
                    table.Cell().Element(ValueBlock).Text($"{_model.Customer.Phone ?? "-"}").Bold().AlignCenter().FontColor(Colors.Red.Darken2);
                });

                // جدول الأصناف بتنسيق RTL
                col.Item().PaddingBottom(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // المنتج
                        columns.RelativeColumn(3); // الخدمة
                        columns.RelativeColumn(1); // الكمية
                        columns.RelativeColumn(1); // السعر
                        columns.RelativeColumn(1); // الإجمالي
                        columns.RelativeColumn(1); // حالة الدفع
                        columns.RelativeColumn(1); // العملة

                    });

                    // رأس الجدول (Header) - سيظهر في كل صفحة تلقائياً
                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderBlock).Text("اسم المنتج").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("اسم الخدمة").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("الكمية").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("السعر").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("الإجمالي").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("حالة الدفع").AlignCenter();
                        header.Cell().Element(HeaderBlock).Text("العملة").AlignCenter();


                    });

                    // محتويات الجدول (Items)
                    foreach (var item in Items)
                    {
                        table.Cell().Element(ValueBlock).Text(item.ProductName).AlignRight();

                        table.Cell().Element(ValueBlock).Text(item.ServiceName).AlignRight();

                        table.Cell().Element(ValueBlock)
                            .Text(item.Quantity.ToString("0.##"))
                            .AlignCenter();

                        table.Cell().Element(ValueBlock)
                            .Text(item.UnitPrice.ToString("N2"))
                            .AlignCenter();

                        table.Cell().Element(ValueBlock)
                            .Text(item.Total.ToString("N2"))
                            .AlignCenter();

                        table.Cell().Element(ValueBlock)
                            .Text(item.PaymentStatus)
                            .AlignCenter();

                        table.Cell().Element(ValueBlock)
                            .Text(item.currency)
                            .AlignCenter();
                    }
                });

                col.Item().Row(row =>
                {
                    row.RelativeItem().PaddingLeft(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1); // Value
                            columns.RelativeColumn(2); // Arabic text
                            columns.RelativeColumn(2); // English text
                        });

                        void DrawTotalRow(string ar, decimal value, string eng, bool isBold = false)
                        {
                            // العمود الأول: القيمة (منتصف)
                            var valCell = table.Cell().Element(ValueBlock).Text(value.ToString("G")).AlignCenter().FontColor(Colors.Red.Darken2);
                            if (isBold) valCell.Bold();

                            // العمود الأولالثاني النص العربي (يمين)
                            table.Cell().Element(HeaderBlock).Text(ar).AlignRight().FontSize(10);
                            // العمود الثالث: النص الإنجليزي (يسار)
                            table.Cell().Element(HeaderBlock).Text(eng).FontSize(10).AlignLeft();
                        }

                        table.Cell().ColumnSpan(3).Element(HeaderBlock).Text(t =>
                        {
                            t.Span("اجمالي المبالغ").Bold();
                            t.Span(" / Total Amount").FontSize(9).FontColor(Colors.Grey.Medium);
                        });

                        DrawTotalRow("الإجمالي (محلي)", curr[0], "Total (Local)", true);
                        DrawTotalRow("الإجمالي (ريال سعودي)", curr[1], "Total (SAR)", true);
                        DrawTotalRow("الإجمالي (دولار)", curr[2], "Total (USD)", true);
                        DrawTotalRow("المدفوغ", invoiceSummaryDto.PaidAmount, "PaidAmount", true);
                        DrawTotalRow("المتبقي", invoiceSummaryDto.RemainingAmount, "RemainingAmount", true);


                    });

                });
            });
        }


        // ======================================================
        // تصميم الخلايا والحدود (Helpers)
        // ======================================================
        static IContainer HeaderBlock(IContainer container)
        {
            return container
                .Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten4)
                .Padding(4)
                .AlignMiddle();
        }

        static IContainer DarkHeaderBlock(IContainer container)
        {
            return container
                .Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Darken2)
                .Padding(4)
                .AlignMiddle();
        }

        static IContainer ValueBlock(IContainer container)
        {
            return container
                .Border(1).BorderColor(Colors.Black)
                .Padding(4)
                .AlignMiddle();
        }

        public DocumentSettings GetSettings()
        {
            return new DocumentSettings
            {
                ImageCompressionQuality = ImageCompressionQuality.High
            };
        }
    }
}
