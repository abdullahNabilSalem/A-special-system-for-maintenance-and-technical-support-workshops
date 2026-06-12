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
    internal class PaymentDAL
    {
        public List<(int InvoiceID, decimal Total, decimal Paid)> GetPendingInvoices(SqlConnection con, SqlTransaction trans, int customerId)
        {
            string query = @"
            SELECT InvoiceID, Total, Paid
            FROM Invoices
            WHERE CustomerID = @CustomerID 
              AND IsCredit = 1 
              AND (Total - Paid) > 0
            ORDER BY InvoiceDate;";

            var list = new List<(int, decimal, decimal)>();

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add((
                            Convert.ToInt32(reader["InvoiceID"]),
                            Convert.ToDecimal(reader["Total"]),
                            Convert.ToDecimal(reader["Paid"])
                        ));
                    }
                }
            }

            return list;
        }
        public void UpdateInvoicePayment(SqlConnection con, SqlTransaction trans, int invoiceId, decimal amount)
        {
            string query = @"
            UPDATE Invoices 
            SET Paid = Paid + @Amount,
                Remain = Total - (Paid + @Amount)
            WHERE InvoiceID = @InvoiceID;";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;

                cmd.ExecuteNonQuery();
            }
        }
        public void InsertPaymentLog(SqlConnection con, SqlTransaction trans, int customerId, decimal amount)
        {
            string query = @"
            INSERT INTO PaymentsLog (CustomerID, PaymentAmount, PaymentDate)
            VALUES (@CustomerID, @Amount, GETDATE());";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;

                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetCustomerCreditInvoices(int customerId)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT 
            i.InvoiceID AS [رقم الفاتورة],
            i.InvoiceDate AS [تاريخ الفاتورة],
            i.Total AS [الإجمالي],
            i.Paid AS [المدفوع],
            (i.Total - i.Paid) AS [المتبقي]
        FROM Invoices i
        WHERE 
            i.IsCredit = 1 
            AND i.CustomerID = @CustomerID 
            AND (i.Total - i.Paid) > 0
        ORDER BY i.InvoiceDate;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
        }
        public DataTable GetCustomers()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT CustomerID, CustomerName FROM Customers";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable GetCustomerCreditReport(int customerId)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT 
                        i.InvoiceID AS [رقم الفاتورة],
                        i.InvoiceDate AS [تاريخ الفاتورة],
                        i.Total AS [المبلغ الإجمالي],
                        i.Paid AS [المدفوع],
                        i.Remain AS [المتبقي],
                        MAX(d.Currency) AS [العملة]
                    FROM Invoices i
                    LEFT JOIN InvoiceDetails d 
                        ON i.InvoiceID = d.InvoiceID
                    WHERE 
                        i.IsCredit = 1
                        AND i.CustomerID = @CustomerID
                        AND i.Remain > 0
                    GROUP BY 
                        i.InvoiceID,
                        i.InvoiceDate,
                        i.Total,
                        i.Paid,
                        i.Remain
                    ORDER BY i.InvoiceDate DESC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }
        public decimal GetTotalRemaining(SqlConnection con, SqlTransaction trans, int customerId)
        {
            string query = @"
            SELECT ISNULL(SUM(Total - Paid), 0)
            FROM Invoices
            WHERE CustomerID = @CustomerID 
            AND IsCredit = 1";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
        public List<(int invoiceId, decimal total, decimal paid)> GetAllPendingInvoices(
            SqlConnection con,
            SqlTransaction trans,
            int customerId)
        {
            string query = @"
                SELECT InvoiceID, Total, Paid
                FROM Invoices
                WHERE CustomerID = @CustomerID 
                  AND IsCredit = 1 
                  AND (Total - Paid) > 0;";

            List<(int, decimal, decimal)> invoices = new List<(int, decimal, decimal)>();

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invoices.Add((
                            Convert.ToInt32(reader["InvoiceID"]),
                            Convert.ToDecimal(reader["Total"]),
                            Convert.ToDecimal(reader["Paid"])
                        ));
                    }
                }
            }

            return invoices;
        }
        public void CloseInvoice(
            SqlConnection con,
            SqlTransaction trans,
            int invoiceId)
        {
            string query = @"
            UPDATE Invoices
            SET Paid = Total,
                Remain = 0
            WHERE InvoiceID = @InvoiceID;";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;

                cmd.ExecuteNonQuery();
            }
        }
        public (int customerId, decimal remain) GetInvoiceInfo(
    SqlConnection con,
    SqlTransaction trans,
    int invoiceId)
        {
            string query = @"
            SELECT CustomerID, Remain
            FROM Invoices
            WHERE InvoiceID = @InvoiceID;";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            Convert.ToInt32(reader["CustomerID"]),
                            Convert.ToDecimal(reader["Remain"])
                        );
                    }
                }
            }

            throw new Exception("الفاتورة غير موجودة");
        }
        public List<PaymentLog> GetPayments(
    DateTime from,
    DateTime to,
    int page,
    int pageSize)
        {
            List<PaymentLog> list = new List<PaymentLog>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT
                    p.PaymentID,
                    c.CustomerName,
                    p.PaymentAmount,
                    p.PaymentDate
                FROM PaymentsLog p
                INNER JOIN Customers c
                    ON p.CustomerID = c.CustomerID
                WHERE p.PaymentDate BETWEEN @From AND @To
                ORDER BY p.PaymentDate DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@From", SqlDbType.Date)
                        .Value = from.Date;

                    cmd.Parameters.Add("@To", SqlDbType.Date)
                        .Value = to.Date;

                    cmd.Parameters.Add("@Offset", SqlDbType.Int)
                        .Value = (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int)
                        .Value = pageSize;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PaymentLog
                            {
                                PaymentID = Convert.ToInt32(reader["PaymentID"]),

                                CustomerName = reader["CustomerName"].ToString(),

                                PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]),

                                PaymentDate = Convert.ToDateTime(reader["PaymentDate"])
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetPaymentsCount(DateTime from, DateTime to)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT COUNT(*)
                FROM PaymentsLog
                WHERE PaymentDate BETWEEN @From AND @To";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@From", SqlDbType.Date)
                        .Value = from.Date;

                    cmd.Parameters.Add("@To", SqlDbType.Date)
                        .Value = to.Date;

                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
