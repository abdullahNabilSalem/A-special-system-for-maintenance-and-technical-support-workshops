using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class DeviceReceiptSearchResult
    {
        public int ReceiptID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProblemDescription { get; set; }
        public string SerialNumber { get; set; }
        public string ReceiptNumber { get; set; }
    }
}
