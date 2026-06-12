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
    public class ComplaintBLL
    {
        private ComplaintDAL dal = new ComplaintDAL();

        public DataTable GetComplaints(int pageNumber, int pageSize)
        {
            return dal.GetComplaints(pageNumber, pageSize);
        }

        public int GetTotalPages(int pageSize)
        {
            int totalRecords = dal.GetTotalComplaintsCount();
            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        public DataTable SearchComplaintByID(int complaintId)
        {
            return dal.SearchComplaintByID(complaintId);
        }
        public int AddComplaint(Complaint complaint)
        {
            if (complaint == null)
                throw new ArgumentNullException("بيانات الشكوى غير موجودة");

            if (string.IsNullOrWhiteSpace(complaint.CustomerName))
                throw new Exception("اسم العميل مطلوب");

            if (complaint.CustomerName.Length < 3)
                throw new Exception("اسم العميل قصير جداً");

            if (string.IsNullOrWhiteSpace(complaint.PhoneNumber))
                throw new Exception("رقم الهاتف مطلوب");

            if (!System.Text.RegularExpressions.Regex.IsMatch(complaint.PhoneNumber, @"^\d{7,15}$"))
                throw new Exception("رقم الهاتف غير صالح");

            if (string.IsNullOrWhiteSpace(complaint.Region))
                throw new Exception("المنطقة مطلوبة");

            if (complaint.EmployeeID <= 0)
                throw new Exception("يجب اختيار موظف");

            if (complaint.ProductID <= 0)
                throw new Exception("يجب اختيار المنتج");

            if (string.IsNullOrWhiteSpace(complaint.ProblemType))
                throw new Exception("يجب اختيار نوع المشكلة");

            if (string.IsNullOrWhiteSpace(complaint.ProblemDescription))
                throw new Exception("وصف المشكلة مطلوب");

            if (complaint.ProblemDescription.Length < 5)
                throw new Exception("وصف المشكلة قصير جداً");

            if (string.IsNullOrWhiteSpace(complaint.ComplaintStatus))
                throw new Exception("يجب تحديد حالة الشكوى");

            if (complaint.ComplaintDate > DateTime.Now)
                throw new Exception("تاريخ الشكوى لا يمكن أن يكون في المستقبل");

            if (!string.IsNullOrWhiteSpace(complaint.SerialNumber))
            {
                if (complaint.SerialNumber.Length < 3)
                    throw new Exception("الرقم التسلسلي غير صحيح");
            }

            // 🔥 هنا التعديل
            return dal.AddComplaint(complaint);
        }
        public DataTable GetEmployees()
        {
            return dal.GetEmployees();
        }

        public DataTable GetProducts()
        {
            return dal.GetProducts();
        }
    }
}
