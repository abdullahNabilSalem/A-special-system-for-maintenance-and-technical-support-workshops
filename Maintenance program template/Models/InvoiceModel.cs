using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class InvoiceModel
    {
        public int ReceiptId { get; set; }
        public string SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string ProductType { get; set; }
        public string Problem { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
