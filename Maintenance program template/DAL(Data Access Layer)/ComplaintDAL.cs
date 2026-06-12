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
    public class ComplaintDAL
    {
        public DataTable GetComplaints(int pageNumber, int pageSize)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    ComplaintID        AS N'رقم الشكوى',
                    CustomerName       AS N'اسم العميل',
                    Region             AS N'المنطقة',
                    PhoneNumber        AS N'رقم الهاتف',
                    ProductNumber      AS N'رقم المنتج',
                    ProblemType        AS N'نوع المشكلة',
                    ProblemDescription AS N'وصف المشكلة',
                    ComplaintDate      AS N'تاريخ الشكوى',
                    EmployeeName       AS N'اسم الموظف',
                    ComplaintStatus    AS N'حالة الشكوى',
                    SerialNumber       AS N'الرقم التسلسلي'
                FROM Complaints
                ORDER BY ComplaintID DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public int GetTotalComplaintsCount()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Complaints";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public DataTable SearchComplaintByID(int complaintId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    ComplaintID        AS N'رقم الشكوى',
                    CustomerName       AS N'اسم العميل',
                    Region             AS N'المنطقة',
                    PhoneNumber        AS N'رقم الهاتف',
                    ProductNumber      AS N'رقم المنتج',
                    ProblemType        AS N'نوع المشكلة',
                    ProblemDescription AS N'وصف المشكلة',
                    ComplaintDate      AS N'تاريخ الشكوى',
                    EmployeeName       AS N'اسم الموظف',
                    ComplaintStatus    AS N'حالة الشكوى',
                    SerialNumber       AS N'الرقم التسلسلي'
                FROM Complaints
                WHERE ComplaintID = @ComplaintID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ComplaintID", complaintId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public int AddComplaint(Complaint complaint)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        INSERT INTO Complaints
        (CustomerName, PhoneNumber, Region, SerialNumber, EmployeeName,
         ComplaintStatus, ProblemDescription, ComplaintDate, ProductNumber, ProblemType)
        VALUES
        (@CustomerName, @PhoneNumber, @Region, @SerialNumber, @EmployeeName,
         @ComplaintStatus, @ProblemDescription, @ComplaintDate, @ProductNumber, @ProblemType);

        SELECT SCOPE_IDENTITY();"; // 🔥 هذا السطر المهم

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CustomerName", complaint.CustomerName);
                cmd.Parameters.AddWithValue("@PhoneNumber", complaint.PhoneNumber);
                cmd.Parameters.AddWithValue("@Region", complaint.Region);
                cmd.Parameters.AddWithValue("@SerialNumber", complaint.SerialNumber);
                cmd.Parameters.AddWithValue("@EmployeeName", complaint.EmployeeID);
                cmd.Parameters.AddWithValue("@ComplaintStatus", complaint.ComplaintStatus);
                cmd.Parameters.AddWithValue("@ProblemDescription", complaint.ProblemDescription);
                cmd.Parameters.AddWithValue("@ComplaintDate", complaint.ComplaintDate);
                cmd.Parameters.AddWithValue("@ProductNumber", complaint.ProductID);
                cmd.Parameters.AddWithValue("@ProblemType", complaint.ProblemType);

                con.Open();

                // 🔥 هنا نسترجع ID
                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                return newId;
            }
        }
        public DataTable GetEmployees()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT EmployeeID, EmployeeName FROM Employees", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetProducts()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ProductID, ProductName FROM Products", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
