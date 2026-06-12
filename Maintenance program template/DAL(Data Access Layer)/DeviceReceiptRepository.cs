using Maintenance_program_template.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using technicalSupport;

namespace Maintenance_program_template.DAL_Data_Access_Layer_
{
    internal class DeviceReceiptRepository
    {
        //✔ التحقق من وجود السند
        public bool IsReceiptExists(string receiptNumber)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT 1 FROM DeviceReceipts WHERE ReceiptNumber = @ReceiptNumber";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);

                con.Open();

                return cmd.ExecuteScalar() != null;
            }
        }
        //✔ إدخال السند
        public int InsertReceipt(DeviceReceipt receipt)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO DeviceReceipts 
        (CustomerName, PhoneNumber, ProductType, ProblemDescription, ReceiveDate,
         ReceivedBy, SerialNumber, ReceiptNumber, DeviceStatus)
        OUTPUT INSERTED.ReceiptID
        VALUES 
        (@CustomerName, @PhoneNumber, @ProductType, @ProblemDescription, @ReceiveDate,
         @ReceivedBy, @SerialNumber, @ReceiptNumber, @DeviceStatus)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CustomerName", receipt.CustomerName);
                cmd.Parameters.AddWithValue("@PhoneNumber", receipt.PhoneNumber);
                cmd.Parameters.AddWithValue("@ProductType", receipt.ProductType);
                cmd.Parameters.AddWithValue("@ProblemDescription", receipt.ProblemDescription);
                cmd.Parameters.AddWithValue("@ReceiveDate", receipt.ReceiveDate);
                cmd.Parameters.AddWithValue("@ReceivedBy", receipt.ReceivedBy);
                cmd.Parameters.AddWithValue("@SerialNumber", receipt.SerialNumber);
                cmd.Parameters.AddWithValue("@ReceiptNumber", receipt.ReceiptNumber);
                cmd.Parameters.AddWithValue("@DeviceStatus", receipt.DeviceStatus);

                con.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public DataTable GetEmployees()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT EmployeeID, EmployeeName FROM Employees", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable GetReceipts(
    int pageNumber,
    int pageSize)
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    ReceiptID       AS [رقم الاستلام],
                    CustomerName    AS [اسم العميل],
                    PhoneNumber     AS [رقم الهاتف],
                    ProductType     AS [نوع الجهاز],
                    ProblemDescription AS [وصف المشكلة],
                    ReceiveDate     AS [تاريخ الاستلام],
                    ReceivedBy      AS [المستلم],
                    SerialNumber    AS [السيريال],
                    ReceiptNumber   AS [رقم السند],
                    DeviceStatus    AS [حالة الجهاز]
                FROM DeviceReceipts
                ORDER BY ReceiptID DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Offset",
                    (pageNumber - 1) * pageSize);

                cmd.Parameters.AddWithValue(
                    "@PageSize",
                    pageSize);

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }
        public DataTable SearchReceipts(string keyword)
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT 
            ReceiptID       AS [رقم الاستلام],
            CustomerName    AS [اسم العميل],
            PhoneNumber     AS [رقم الهاتف],
            ProductType     AS [نوع الجهاز],
            ProblemDescription AS [وصف المشكلة],
            ReceiveDate     AS [تاريخ الاستلام],
            ReceivedBy      AS [المستلم],
            SerialNumber    AS [السيريال],
            ReceiptNumber   AS [رقم السند],
            DeviceStatus    AS [حالة الجهاز]
        FROM DeviceReceipts
        WHERE 
            CAST(ReceiptID AS NVARCHAR) LIKE @Key
            OR PhoneNumber LIKE @Key
            OR ReceiptNumber LIKE @Key
            OR CustomerName LIKE @Key
            OR SerialNumber LIKE @Key
        ORDER BY ReceiptID DESC";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Key",
                    "%" + keyword + "%");

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }
        public DeviceReceiptSearchResult Search(string keyword)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT TOP 1
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProblemDescription,
            SerialNumber,
            ReceiptNumber
        FROM DeviceReceipts
        WHERE 
            PhoneNumber = @Key
            OR SerialNumber = @Key
            OR ReceiptNumber = @Key
            OR CAST(ReceiptID AS NVARCHAR) = @Key
        ORDER BY ReceiveDate DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Key", keyword);

                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new DeviceReceiptSearchResult
                    {
                        ReceiptID = Convert.ToInt32(rd["ReceiptID"]),
                        CustomerName = rd["CustomerName"].ToString(),
                        PhoneNumber = rd["PhoneNumber"].ToString(),
                        ProblemDescription = rd["ProblemDescription"].ToString(),
                        SerialNumber = rd["SerialNumber"].ToString(),
                        ReceiptNumber = rd["ReceiptNumber"].ToString()
                    };
                }

                return null;
            }
        }
        public void SaveRepair(Repair repair)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                SqlTransaction tx = con.BeginTransaction();

                try
                {
                    // 1️⃣ Insert Repair
                    SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Repairs
            (ReceiptID, EmployeeID, WorkDescription, ClientNotes, RepairCost, PaymentMethod, DeliveryDate, WarrantyPeriod, Currency)
            OUTPUT INSERTED.RepairID
            VALUES (@ReceiptID, @EmployeeID, @WorkDescription, @ClientNotes, @RepairCost, @PaymentMethod, GETDATE(), @WarrantyPeriod, @Currency)
            ", con, tx);

                    cmd.Parameters.AddWithValue("@ReceiptID", repair.ReceiptID);
                    cmd.Parameters.AddWithValue("@EmployeeID", repair.EmployeeID);
                    cmd.Parameters.AddWithValue("@WorkDescription", (object)repair.WorkDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClientNotes", (object)repair.ClientNotes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RepairCost", repair.RepairCost);
                    cmd.Parameters.AddWithValue("@PaymentMethod", repair.PaymentMethod);
                    cmd.Parameters.AddWithValue("@WarrantyPeriod", repair.WarrantyPeriod);
                    cmd.Parameters.AddWithValue("@Currency", repair.Currency);

                    int repairID = Convert.ToInt32(cmd.ExecuteScalar());

                    // 2️⃣ Parts
                    foreach (var part in repair.Parts)
                    {
                        // خصم المخزون
                        SqlCommand cmdStock = new SqlCommand(@"
                UPDATE SpareParts
                SET Quantity = Quantity - @QTY
                WHERE PartID = @ID AND Quantity >= @QTY
                ", con, tx);

                        cmdStock.Parameters.AddWithValue("@ID", part.PartID);
                        cmdStock.Parameters.AddWithValue("@QTY", part.Quantity);

                        int affected = cmdStock.ExecuteNonQuery();
                        if (affected == 0)
                            throw new Exception($"المخزون غير كافٍ للقطعة رقم {part.PartID}");

                        // إدخال القطع
                        SqlCommand cmdPart = new SqlCommand(@"
                INSERT INTO RepairParts (RepairID, PartID, Quantity, Price)
                VALUES (@RepairID, @PartID, @Quantity, @Price)
                ", con, tx);

                        cmdPart.Parameters.AddWithValue("@RepairID", repairID);
                        cmdPart.Parameters.AddWithValue("@PartID", part.PartID);
                        cmdPart.Parameters.AddWithValue("@Quantity", part.Quantity);
                        cmdPart.Parameters.AddWithValue("@Price", part.Price);

                        cmdPart.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        public DataTable GetRepairs(int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT 
            RepairID AS [رقم العملية],
            ReceiptID AS [رقم السند],
            EmployeeID AS [الموظف],
            WorkDescription AS [وصف العمل],
            ClientNotes AS [ملاحظات العميل],
            RepairCost AS [التكلفة],
            PaymentMethod AS [طريقة الدفع],
            DeliveryDate AS [تاريخ التسليم],
            WarrantyPeriod AS [الضمان],
            Currency AS [العملة]
        FROM Repairs
        ORDER BY RepairID DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable SearchRepairs(string search)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    R.RepairID AS [رقم العملية],
                    R.ReceiptID AS [رقم السند],
                    R.EmployeeID AS [الموظف],
                    R.WorkDescription AS [وصف العمل],
                    R.ClientNotes AS [ملاحظات العميل],
                    R.RepairCost AS [التكلفة],
                    R.PaymentMethod AS [طريقة الدفع],
                    R.DeliveryDate AS [تاريخ التسليم],
                    R.WarrantyPeriod AS [مدة الضمان],
                    R.Currency AS [العملة]
                FROM Repairs R
                WHERE 
                    -- البحث برقم السند
                    R.ReceiptID = @ReceiptID

                    OR

                    -- البحث برقم الهاتف (بدون عرض الجدول الثاني)
                    R.ReceiptID IN (
                        SELECT ReceiptID 
                        FROM DeviceReceipts 
                        WHERE PhoneNumber LIKE '%' + @Phone + '%'
                    )

                ORDER BY R.RepairID DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                // رقم السند
                if (int.TryParse(search, out int receiptID))
                    cmd.Parameters.AddWithValue("@ReceiptID", receiptID);
                else
                    cmd.Parameters.AddWithValue("@ReceiptID", -1); // مستحيل

                // الهاتف
                cmd.Parameters.AddWithValue("@Phone", search);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable GetReceiptForWhatsApp(int receiptID)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    CustomerName,
                    PhoneNumber,
                    ProblemDescription
                FROM DeviceReceipts
                WHERE ReceiptID = @ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", receiptID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public DataTable GetReceiptsPaged(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    ReceiptID AS N'رقم الاستلام',
                    CustomerName AS N'اسم العميل',
                    PhoneNumber AS N'رقم الهاتف',
                    ProductType AS N'نوع الجهاز',
                    ProblemDescription AS N'وصف المشكلة',
                    ReceiveDate AS N'تاريخ الاستلام',
                    ReceivedBy AS N'تم الاستلام بواسطة',
                    SerialNumber AS N'الرقم التسلسلي',
                    ReceiptNumber AS N'رقم السند',
                    DeviceStatus AS N'حالة الجهاز'
                FROM DeviceReceipts
                WHERE (@FromDate IS NULL OR ReceiveDate >= @FromDate)
                  AND (@ToDate IS NULL OR ReceiveDate < DATEADD(DAY, 1, @ToDate))
                ORDER BY ReceiptID DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public int GetReceiptsCount(DateTime? fromDate, DateTime? toDate)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT COUNT(*)
                FROM DeviceReceipts
                WHERE (@FromDate IS NULL OR ReceiveDate >= @FromDate)
                  AND (@ToDate IS NULL OR ReceiveDate <= @ToDate)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public DataTable GetRepairsPaged(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    r.RepairID AS N'رقم الإصلاح',
                    r.ReceiptID AS N'رقم الاستلام',
                    e.EmployeeName AS N'اسم الموظف',
                    r.WorkDescription AS N'وصف العمل',
                    r.ClientNotes AS N'ملاحظات العميل',
                    r.RepairCost AS N'تكلفة الإصلاح',
                    r.PaymentMethod AS N'طريقة الدفع',
                    r.DeliveryDate AS N'تاريخ التسليم',
                    r.WarrantyPeriod AS N'فترة الضمان',
                    r.Currency AS N'العملة'
                FROM Repairs r
                LEFT JOIN Employees e ON r.EmployeeID = e.EmployeeID
                WHERE r.DeliveryDate IS NOT NULL
                AND (@FromDate IS NULL OR r.DeliveryDate >= @FromDate)
                AND (@ToDate IS NULL OR r.DeliveryDate < DATEADD(DAY, 1, @ToDate))
                ORDER BY r.RepairID DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        public int GetRepairsCount(DateTime? fromDate, DateTime? toDate)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string countQuery = @"
                SELECT COUNT(*)
                FROM Repairs
                WHERE (@FromDate IS NULL OR DeliveryDate >= @FromDate)
                  AND (@ToDate IS NULL OR DeliveryDate < DATEADD(DAY, 1, @ToDate))";

                SqlCommand cmd = new SqlCommand(countQuery, con);

                cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public DataTable GetEmployeeReport(DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    e.EmployeeName AS [اسم الموظف],
                    COUNT(r.RepairID) AS [عدد الأجهزة التي تم إصلاحها]
                FROM Repairs r
                INNER JOIN Employees e ON r.EmployeeID = e.EmployeeID
                WHERE 
                    r.DeliveryDate >= @FromDate
                    AND r.DeliveryDate < @ToDate
                GROUP BY e.EmployeeName
                ORDER BY COUNT(r.RepairID) DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        string query = @"
        SELECT 
            ComplaintID AS [رقم الشكوى],
            CustomerName AS [اسم العميل],
            Region AS [المنطقة],
            PhoneNumber AS [رقم الهاتف],
            ProductNumber AS [نوع المنتج],
            ProblemType AS [نوع المشكلة],
            ProblemDescription AS [وصف المشكلة],
            ComplaintDate AS [تاريخ الشكوى],
            EmployeeName AS [اسم الموظف],
            ComplaintStatus AS [حالة الشكوى]
        FROM Complaints
        WHERE 
            ComplaintDate >= @FromDate
            AND ComplaintDate < @ToDate
        ORDER BY ComplaintDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        public DataTable GetComplaints(DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }
}
