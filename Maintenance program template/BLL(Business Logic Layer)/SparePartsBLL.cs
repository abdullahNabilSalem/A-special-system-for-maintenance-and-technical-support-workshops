using Maintenance_program_template.DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class SparePartsBLL
    {
        private SparePartsDAL dal = new SparePartsDAL();

        public DataTable GetPartsPaged(int pageNumber, int pageSize)
        {
            return dal.GetPartsPaged(pageNumber, pageSize);
        }

        public string BuildFilter(string supplier, int quantityFilterIndex)
        {
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(supplier))
                filters.Add($"Supplier = '{supplier}'");

            switch (quantityFilterIndex)
            {
                case 1:
                    filters.Add("Quantity < 5");
                    break;
                case 2:
                    filters.Add("Quantity = 0");
                    break;
            }

            return string.Join(" AND ", filters);
        }
        public DataTable GetParts(int page, int size)
        {
            return dal.GetParts(page, size);
        }
    }
}
