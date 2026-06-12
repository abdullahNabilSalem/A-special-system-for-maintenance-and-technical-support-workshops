using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport;

namespace Maintenance_program_template.DAL_Data_Access_Layer_
{
    internal class ProductRepository
    {
        public DataTable GetProducts()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT ProductID, ProductName FROM Products";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }
}
