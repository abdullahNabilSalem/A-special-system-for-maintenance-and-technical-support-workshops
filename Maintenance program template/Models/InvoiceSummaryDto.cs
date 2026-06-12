using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class InvoiceSummaryDto
    {
        public decimal TotalExcludingVat { get; set; }
        public decimal ksa { get; set; }
        public decimal usd { get; set; }

        public decimal PaidAmount { get; set; }      // المدفوع
        public decimal RemainingAmount { get; set; } // المتبقي

        //public decimal VatAmount { get; set; }

        //public decimal TotalIncludingVat { get; set; }

        //public decimal discountAmount { get; set; }
        //public decimal netDiscountAmount { get; set; }

    }
}
