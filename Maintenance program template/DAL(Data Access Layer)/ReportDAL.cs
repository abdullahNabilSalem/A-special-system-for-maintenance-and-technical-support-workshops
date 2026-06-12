using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance_program_template;
using Maintenance_program_template.Models;
using technicalSupport;

namespace Maintenance_program_template.DAL_Data_Access_Layer_
{
    public class ReportDAL
    {
        public List<FinancialReport> GetReports(DateTime from, DateTime to)
        {
            List<FinancialReport> list = new List<FinancialReport>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    i.InvoiceID AS [رقم الفاتورة],
                    i.InvoiceDate AS [التاريخ],
                    c.CustomerName AS [اسم العميل],
                    i.Total AS [الإجمالي],
                    i.Paid AS [المدفوع],
                    i.Remain AS [المتبقي],
                    (SELECT TOP 1 Currency FROM InvoiceDetails d WHERE d.InvoiceID = i.InvoiceID) AS [العملة],
                    CASE 
                        WHEN i.IsCredit = 1 THEN N'آجل' 
                        ELSE N'نقد' 
                    END AS [نوع الدفع]
                FROM Invoices i
                LEFT JOIN Customers c ON i.CustomerID = c.CustomerID
                WHERE i.InvoiceDate BETWEEN @From AND @To
                ORDER BY i.InvoiceID DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@From", SqlDbType.Date).Value = from.Date;
                    cmd.Parameters.Add("@To", SqlDbType.Date).Value = to.Date;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new FinancialReport
                            {
                                InvoiceID = Convert.ToInt32(reader["رقم الفاتورة"]),
                                InvoiceDate = Convert.ToDateTime(reader["التاريخ"]),
                                CustomerName = reader["اسم العميل"]?.ToString(),
                                Total = Convert.ToDecimal(reader["الإجمالي"]),
                                Paid = Convert.ToDecimal(reader["المدفوع"]),
                                Remain = Convert.ToDecimal(reader["المتبقي"]),
                                Currency = reader["العملة"]?.ToString(),
                                PaymentType = reader["نوع الدفع"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public DataTable GetCreditReports(DateTime from, DateTime to)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    i.InvoiceID AS [رقم الفاتورة],
                    i.InvoiceDate AS [التاريخ],
                    c.CustomerName AS [اسم العميل],
                    i.Total AS [الإجمالي],
                    i.Paid AS [المدفوع],
                    i.Remain AS [المتبقي],
                    N'آجل' AS [نوع الدفع],

                    (
                        SELECT TOP 1 d.Currency 
                        FROM InvoiceDetails d 
                        WHERE d.InvoiceID = i.InvoiceID
                    ) AS [العملة]

                FROM Invoices i
                INNER JOIN Customers c ON i.CustomerID = c.CustomerID
                WHERE 
                    i.IsCredit = 1
                    AND i.InvoiceDate BETWEEN @From AND @To
                ORDER BY i.InvoiceID DESC;";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@From", from);
                cmd.Parameters.AddWithValue("@To", to);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }

}
