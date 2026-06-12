using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    internal class PaymentBLL
    {
        private PaymentDAL dal = new PaymentDAL();

        public void PayCredit(int customerId, decimal amount)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();

                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        // ✅ احسب إجمالي الدين
                        decimal totalRemaining = dal.GetTotalRemaining(con, trans, customerId);

                        if (totalRemaining <= 0)
                            throw new Exception("لا يوجد دين على هذا العميل");

                        // ❌ منع الدفع الزائد
                        if (amount > totalRemaining)
                            throw new Exception($"المبلغ المدخل أكبر من الدين المتبقي ({totalRemaining:N0})");

                        var invoices = dal.GetPendingInvoices(con, trans, customerId);

                        decimal originalAmount = amount;

                        foreach (var inv in invoices)
                        {
                            if (amount <= 0) break;

                            decimal remaining = inv.Total - inv.Paid;
                            decimal toPay = Math.Min(remaining, amount);

                            dal.UpdateInvoicePayment(con, trans, inv.InvoiceID, toPay);

                            amount -= toPay;
                        }

                        dal.InsertPaymentLog(con, trans, customerId, originalAmount);

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
        public (DataTable table, decimal totalRemaining) GetCustomerInvoices(int customerId)
        {
            var dt = dal.GetCustomerCreditInvoices(customerId);

            decimal totalRemaining = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalRemaining += Convert.ToDecimal(row["المتبقي"]);
            }

            return (dt, totalRemaining);
        }
        public DataTable GetCustomers()
        {
            return dal.GetCustomers();
        }
        public (DataTable table, decimal totalRemaining) GetCustomerCreditReport(int customerId)
        {
            var dt = dal.GetCustomerCreditReport(customerId);

            decimal totalRemaining = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalRemaining += Convert.ToDecimal(row["المتبقي"]);
            }

            return (dt, totalRemaining);
        }
        public void PayAllCustomerDebts(int customerId)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();

                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        var invoices = dal.GetAllPendingInvoices(con, trans, customerId);

                        if (invoices.Count == 0)
                            throw new Exception("لا توجد فواتير آجلة لهذا العميل");

                        decimal totalPaid = 0;

                        foreach (var inv in invoices)
                        {
                            decimal remaining = inv.total - inv.paid;

                            totalPaid += remaining;

                            dal.CloseInvoice(con, trans, inv.invoiceId);
                        }

                        dal.InsertPaymentLog(con, trans, customerId, totalPaid);

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
        public void PaySingleInvoice(int invoiceId)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();

                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        // ✅ جلب بيانات الفاتورة قبل التحديث
                        var invoice = dal.GetInvoiceInfo(con, trans, invoiceId);

                        if (invoice.remain <= 0)
                            throw new Exception("هذه الفاتورة مسددة بالفعل");

                        // ✅ تحديث الفاتورة
                        dal.CloseInvoice(con, trans, invoiceId);

                        // ✅ تسجيل الدفع الحقيقي
                        dal.InsertPaymentLog(
                            con,
                            trans,
                            invoice.customerId,
                            invoice.remain);

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
        public List<PaymentLog> GetPayments(
        DateTime from,
        DateTime to,
        int page,
        int pageSize)
        {
            if (from > to)
                throw new Exception("تاريخ البداية أكبر من النهاية");

            return dal.GetPayments(from, to, page, pageSize);
        }

        public int GetTotalPages(
            DateTime from,
            DateTime to,
            int pageSize)
        {
            int totalRecords = dal.GetPaymentsCount(from, to);

            return (int)Math.Ceiling(
                (double)totalRecords / pageSize);
        }
    }
}
