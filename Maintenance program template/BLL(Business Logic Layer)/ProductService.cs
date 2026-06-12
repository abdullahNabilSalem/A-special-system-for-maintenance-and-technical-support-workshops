using Maintenance_program_template.DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class ProductService
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public DataTable GetProducts()
        {
            return _repo.GetProducts();
        }
    }
}
