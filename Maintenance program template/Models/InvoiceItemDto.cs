using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class InvoiceItemDto
    {
        public string ItemCode { get; set; }

        public string ProductName { get; set; }
        public string ServiceName { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }
        public string PaymentStatus { get; set; }

        public string currency { get; set; }
    }
}
