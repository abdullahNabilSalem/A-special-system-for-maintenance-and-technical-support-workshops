using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.DAL_Data_Access_Layer_
{
    public class InvoiceDAL
    {
        public int InsertInvoice(SqlConnection con, SqlTransaction trans, Invoice invoice)
        {
            string query = @"
        INSERT INTO Invoices (InvoiceDate, CustomerID, IsCredit, Total, Paid, Remain)
        VALUES (@date, @customer, @credit, @total, @paid, @remain);
        SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = invoice.InvoiceDate;
                cmd.Parameters.Add("@customer", SqlDbType.Int).Value =
                    (object)invoice.CustomerID ?? DBNull.Value;
                cmd.Parameters.Add("@credit", SqlDbType.Bit).Value = invoice.IsCredit;
                cmd.Parameters.Add("@total", SqlDbType.Decimal).Value = invoice.Total;
                cmd.Parameters.Add("@paid", SqlDbType.Decimal).Value = invoice.Paid;
                cmd.Parameters.Add("@remain", SqlDbType.Decimal).Value = invoice.Remain;

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void InsertInvoiceDetails(SqlConnection con, SqlTransaction trans, int invoiceID, List<InvoiceDetail> details)
        {
            string query = @"
        INSERT INTO InvoiceDetails 
        (InvoiceID, ProductName, ServiceName, Quantity, Price, TotalLine, Currency)
        VALUES 
        (@id, @product, @service, @qty, @price, @totalLine, @currency)";

            foreach (var item in details)
            {
                using (SqlCommand cmd = new SqlCommand(query, con, trans))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = invoiceID;
                    cmd.Parameters.Add("@product", SqlDbType.NVarChar).Value = (object)item.ProductName ?? DBNull.Value;
                    cmd.Parameters.Add("@service", SqlDbType.NVarChar).Value = (object)item.ServiceName ?? DBNull.Value;
                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = item.Quantity;
                    cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = item.Price;
                    cmd.Parameters.Add("@totalLine", SqlDbType.Decimal).Value = item.TotalLine;
                    cmd.Parameters.Add("@currency", SqlDbType.NVarChar).Value = item.Currency;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
