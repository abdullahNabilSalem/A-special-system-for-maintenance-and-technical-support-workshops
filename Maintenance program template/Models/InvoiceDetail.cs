using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class InvoiceDetail
    {
        public string ProductName { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalLine { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }
    }
}
