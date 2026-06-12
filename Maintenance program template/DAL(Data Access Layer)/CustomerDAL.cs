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
    public class CustomerDAL
    {
        SqlConnection con = DatabaseHelper.GetConnection();
        public List<Customer> GetAllCustomers()
        {
            List<Customer> list = new List<Customer>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT CustomerID, CustomerName FROM Customers";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer
                            {
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                CustomerName = reader["CustomerName"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public List<Customer> GetCustomers(int page, int pageSize)
        {
            List<Customer> list = new List<Customer>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    CustomerID,
                    CustomerName,
                    Phone,
                    Address,
                    Notes
                FROM Customers
                ORDER BY CustomerID DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Offset", SqlDbType.Int)
                        .Value = (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int)
                        .Value = pageSize;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer
                            {
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                CustomerName = reader["CustomerName"].ToString(),
                                Phone = reader["Phone"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Notes = reader["Notes"]?.ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetCustomersCount()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Customers";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public void AddCustomer(Customer customer)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
            INSERT INTO Customers
            (
                CustomerName,
                Phone,
                Address,
                Notes
            )
            VALUES
            (
                @CustomerName,
                @Phone,
                @Address,
                @Notes
            )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar)
                        .Value = customer.CustomerName;

                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar)
                        .Value = customer.Phone;

                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar)
                        .Value = customer.Address;

                    cmd.Parameters.Add("@Notes", SqlDbType.NVarChar)
                        .Value = string.IsNullOrWhiteSpace(customer.Notes)
                        ? DBNull.Value
                        : (object)customer.Notes;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable SearchCustomers(string customerName)
        {
            DataTable dt = new DataTable();

            string query = @"
            SELECT
                CustomerID,
                CustomerName,
                Phone,
                Address,
                Notes
            FROM Customers
            WHERE CustomerName LIKE @CustomerName";

            SqlDataAdapter da = new SqlDataAdapter(query, DatabaseHelper.GetConnection());

            da.SelectCommand.Parameters.AddWithValue(
                "@CustomerName",
                "%" + customerName + "%");

            da.Fill(dt);

            return dt;
        }
        public Customer GetCustomerByName(string name)
        {
            Customer customer = null;

            SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 *
                FROM Customers
                WHERE CustomerName LIKE @Name", con);

            cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                customer = new Customer
                {
                    CustomerID = Convert.ToInt32(dr["CustomerID"]),
                    CustomerName = dr["CustomerName"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    Address = dr["Address"].ToString(),
                    Notes = dr["Notes"].ToString()
                };
            }

            con.Close();
            return customer;
        }

        // Update
        public void Update(Customer c)
        {
            SqlCommand cmd = new SqlCommand(@"
                UPDATE Customers
                SET CustomerName=@Name,
                    Phone=@Phone,
                    Address=@Address,
                    Notes=@Notes
                WHERE CustomerID=@ID", con);

            cmd.Parameters.AddWithValue("@ID", c.CustomerID);
            cmd.Parameters.AddWithValue("@Name", c.CustomerName);
            cmd.Parameters.AddWithValue("@Phone", c.Phone);
            cmd.Parameters.AddWithValue("@Address", c.Address);
            cmd.Parameters.AddWithValue("@Notes", c.Notes);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Delete
        public void Delete(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Customers WHERE CustomerID=@ID", con);

            cmd.Parameters.AddWithValue("@ID", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public List<Customer> GetCustomersPaged(
        int pageNumber,
        int pageSize,
        string searchText)
        {
            List<Customer> list = new List<Customer>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT CustomerID, CustomerName, Phone, Address, Notes
                FROM Customers
                WHERE (@Search='' OR CustomerName LIKE '%' + @Search + '%')
                ORDER BY CustomerID
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchText);

                    cmd.Parameters.AddWithValue(
                        "@Offset",
                        (pageNumber - 1) * pageSize);

                    cmd.Parameters.AddWithValue(
                        "@PageSize",
                        pageSize);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer
                            {
                                CustomerID =
                                    Convert.ToInt32(reader["CustomerID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                Phone =
                                    reader["Phone"].ToString(),

                                Address =
                                    reader["Address"].ToString(),

                                Notes =
                                    reader["Notes"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetCustomersCount(string searchText)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT COUNT(*)
        FROM Customers
        WHERE (@Search='' OR CustomerName LIKE '%' + @Search + '%')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchText);

                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
