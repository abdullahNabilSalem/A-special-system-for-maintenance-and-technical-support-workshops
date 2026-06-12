using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class Repair
    {
        public int RepairID { get; set; }
        public int ReceiptID { get; set; }
        public int EmployeeID { get; set; }

        public string WorkDescription { get; set; }
        public string ClientNotes { get; set; }

        public decimal RepairCost { get; set; }
        public string PaymentMethod { get; set; }

        public string WarrantyPeriod { get; set; }
        public string Currency { get; set; }

        public List<RepairPart> Parts { get; set; } = new List<RepairPart>();
    }
}
