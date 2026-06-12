using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class FinancialReport
    {
        [DisplayName("رقم الفاتورة")]
        public int InvoiceID { get; set; }

        [DisplayName("التاريخ")]
        public DateTime InvoiceDate { get; set; }

        [DisplayName("اسم العميل")]
        public string CustomerName { get; set; }

        [DisplayName("الإجمالي")]
        public decimal Total { get; set; }

        [DisplayName("المدفوع")]
        public decimal Paid { get; set; }

        [DisplayName("المتبقي")]
        public decimal Remain { get; set; }

        [DisplayName("العملة")]
        public string Currency { get; set; }

        [DisplayName("نوع الدفع")]
        public string PaymentType { get; set; }
    }
}
