using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class ReportBLL
    {
        private ReportDAL dal = new ReportDAL();

        public List<FinancialReport> GetFinancialReports(DateTime from, DateTime to)
        {
            // تقدر تضيف هنا فلترة / صلاحيات / منطق إضافي
            return dal.GetReports(from, to);
        }
        public DataTable GetCreditReports(DateTime from, DateTime to)
        {
            // ممكن تضيف تحقق هنا
            if (from > to)
                throw new Exception("تاريخ البداية أكبر من تاريخ النهاية");

            return dal.GetCreditReports(from, to);
        }
    }
}
