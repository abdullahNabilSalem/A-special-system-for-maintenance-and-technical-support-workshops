using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class RepairWarranty
    {
        public int RepairID { get; set; }

        public int ReceiptID { get; set; }

        public string CustomerName { get; set; }

        public string ProblemDescription { get; set; }

        public string ReceiptNumber { get; set; }

        public string SerialNumber { get; set; }

        public string WorkDescription { get; set; }

        public string ClientNotes { get; set; }

        public decimal RepairCost { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string WarrantyPeriod { get; set; }
    }
}
