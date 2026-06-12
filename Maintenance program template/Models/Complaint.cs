using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class Complaint
    {
        public int ComplaintID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string SerialNumber { get; set; }
        public int EmployeeID { get; set; }
        public string ComplaintStatus { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime ComplaintDate { get; set; }
        public int ProductID { get; set; }
        public string ProblemType { get; set; }
    }
}
