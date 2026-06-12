using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.Models
{
    internal class CompanyInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string Notes { get; set; }

        // الحقول الجديدة
        public string VATNumber { get; set; }
        public string CRNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DefaultCurrency { get; set; }

        public string EnglishName { get; set; }

    }
}
