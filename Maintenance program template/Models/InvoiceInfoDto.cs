using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class InvoiceInfoDto
    {
        public int InvoiceID { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }


        public string Currency { get; set; }
        public string CurrencyCode { get; set; }


        public string Notes { get; set; }

        public bool IsCredit { get; set; }

        public string PaymentType { get; set; }
    }
}
