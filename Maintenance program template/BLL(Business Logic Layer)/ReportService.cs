using Maintenance_program_template.Models;
using Maintenance_program_template.Reporting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class ReportService
    {
        public string GenerateDeliveryPdf(DeliveryInvoiceModel model)
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"Delivery_{model.ReceiptId}.pdf"
            );

            var document = new DeliveryPDFTemplate(model);
            document.GeneratePdf(path);

            return path;
        }
    }
}
