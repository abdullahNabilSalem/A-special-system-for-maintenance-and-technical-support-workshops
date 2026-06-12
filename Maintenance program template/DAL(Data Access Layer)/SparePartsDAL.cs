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
    public class SparePartsDAL
    {
        public DataTable GetPartsPaged(int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT PartID, PartName, Quantity, Supplier, Price
        FROM SpareParts
        ORDER BY PartID
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable GetParts(int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    PartID AS [رقم القطعة],
                    PartName AS [اسم القطعة],
                    Quantity AS [الكمية المتاحة],
                    Supplier AS [المورّد],
                    Price AS [السعر]
                FROM SpareParts
                ORDER BY PartName ASC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }
}
