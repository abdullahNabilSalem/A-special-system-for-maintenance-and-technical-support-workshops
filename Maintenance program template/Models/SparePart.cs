using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    public class SparePart
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
    }
}
