using Maintenance_program_template.Models;
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
    internal class SupplierRepository
    {
        public DataTable GetSuppliers(int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT 
                            SupplierID, 
                            SupplierName, 
                            SupplierPhone, 
                            Notes
                         FROM Suppliers
                         ORDER BY SupplierID DESC
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
        public void AddSupplier(Supplier supplier)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO Suppliers
                         (SupplierName, SupplierPhone, Notes)
                         VALUES
                         (@Name, @Phone, @Notes)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", supplier.SupplierName);
                cmd.Parameters.AddWithValue("@Phone", supplier.SupplierPhone);
                cmd.Parameters.AddWithValue("@Notes", supplier.Notes ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Supplier GetSupplierByName(string name)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT TOP 1 
                            SupplierID,
                            SupplierName,
                            SupplierPhone,
                            Notes
                         FROM Suppliers
                         WHERE SupplierName LIKE @Name";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new Supplier
                    {
                        SupplierID = Convert.ToInt32(dr["SupplierID"]),
                        SupplierName = dr["SupplierName"].ToString(),
                        SupplierPhone = dr["SupplierPhone"].ToString(),
                        Notes = dr["Notes"].ToString()
                    };
                }

                return null;
            }
        }
        public void UpdateSupplier(Supplier supplier)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE Suppliers
                         SET SupplierName = @Name,
                             SupplierPhone = @Phone,
                             Notes = @Notes
                         WHERE SupplierID = @ID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ID", supplier.SupplierID);
                cmd.Parameters.AddWithValue("@Name", supplier.SupplierName);
                cmd.Parameters.AddWithValue("@Phone", supplier.SupplierPhone);
                cmd.Parameters.AddWithValue("@Notes", supplier.Notes ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteSupplier(int id)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM Suppliers WHERE SupplierID = @ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
