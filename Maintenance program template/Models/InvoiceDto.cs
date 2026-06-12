using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    internal class InvoiceDto
    {
        public CompanyInfo Header { get; set; }

        public InvoiceInfoDto Invoice { get; set; }

        public CustomerDto Customer { get; set; }
        //public SupplierDto Supplier { get; set; }

        public List<InvoiceItemDto> Items { get; set; }

        public InvoiceSummaryDto Summary { get; set; }

        public bool paymentType { get; set; } = false; // false = cash, true = credit

    }
}
