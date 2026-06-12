using Maintenance_program_template.Models;
using System.IO;
using Maintenance_program_template.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class ReceiptReportService
    {
        public void PrintReceipt(InvoiceModel model)
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"Receipt_{model.ReceiptId}.pdf"
            );

            var document = new PDFTemplate(model);
            document.GeneratePdf(path);
        }
    }
}
