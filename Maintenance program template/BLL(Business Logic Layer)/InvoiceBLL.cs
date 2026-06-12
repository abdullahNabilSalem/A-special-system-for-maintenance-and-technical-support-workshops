using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class InvoiceBLL
    {
        private InvoiceDAL dal = new InvoiceDAL();

        public void SaveInvoice(Invoice invoice)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();

                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        int invoiceID = dal.InsertInvoice(con, trans, invoice);
                        dal.InsertInvoiceDetails(con, trans, invoiceID, invoice.Details);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        public decimal CalculateLineTotal(decimal price, int quantity)
        {
            return price * quantity;
        }

        public InvoiceDetail CreateInvoiceDetail(string product, string service, int qty, decimal price, string currency, bool isCredit)
        {
            return new InvoiceDetail
            {
                ProductName = product,
                ServiceName = service,
                Quantity = qty,
                Price = price,
                TotalLine = CalculateLineTotal(price, qty),
                Currency = currency,
                PaymentStatus = isCredit ? "دين" : "مدفوع"
            };
        }
    }
}
