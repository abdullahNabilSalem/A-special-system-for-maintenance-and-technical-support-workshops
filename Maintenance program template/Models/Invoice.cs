using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? CustomerID { get; set; }
        public bool IsCredit { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        public decimal Remain { get; set; }

        public List<InvoiceDetail> Details { get; set; }
    }
}
