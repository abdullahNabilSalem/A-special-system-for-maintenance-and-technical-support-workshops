using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport;

namespace Maintenance_program_template.Models
{
    public class CustomerDto
    {
        public int CustomerID { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
        public string Notes { get; set; }

        public static async Task<CustomerDto> GetCustomerByIdAsync(int? customerId)
        {

            CustomerDto customer = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("GetCustomerByIDForReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)
                    {
                        Value = customerId
                    });

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customer = new CustomerDto();


                            customer.CustomerID = reader["CustomerID"] != DBNull.Value
                                ? Convert.ToInt32(reader["CustomerID"])
                                : 0;

                            customer.Name = reader["CustomerName"] != DBNull.Value
                                ? reader["CustomerName"].ToString()
                                : null;

                            customer.Phone = reader["Phone"] != DBNull.Value
                                ? reader["Phone"].ToString()
                                : null;

                            customer.Address = reader["Address"] != DBNull.Value
                                ? reader["Address"].ToString()
                                : null;

                            customer.Notes = null;
                        }
                    }
                }
            }

            return customer;
        }
    }
}
