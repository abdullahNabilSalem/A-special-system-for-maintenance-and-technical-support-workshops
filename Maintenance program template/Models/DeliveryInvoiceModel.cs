using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class DeliveryInvoiceModel
    {
        public int ReceiptId { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }

        public decimal RepairCost { get; set; }
        public string PaymentMethod { get; set; }

        public string WorkDescription { get; set; }
        public string ClientNotes { get; set; }

        public DateTime DeliveryDate { get; set; }
    }
}
