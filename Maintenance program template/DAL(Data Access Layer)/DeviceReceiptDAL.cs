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
    internal class DeviceReceiptDAL
    {
        public List<DeviceReceipt> GetWaitingClients(
     int page,
     int pageSize)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = 'WaitingClient'
        ORDER BY ReceiveDate DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Offset",
                        SqlDbType.Int).Value =
                        (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize",
                        SqlDbType.Int).Value =
                        pageSize;

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public void MoveToInProgress(int receiptID)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
        UPDATE DeviceReceipts
        SET DeviceStatus = 'InProgress'
        WHERE ReceiptID = @ReceiptID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ReceiptID", SqlDbType.Int)
                        .Value = receiptID;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public int GetWaitingClientsCount()
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT COUNT(*)
        FROM DeviceReceipts
        WHERE DeviceStatus = 'WaitingClient'";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    con.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar());
                }
            }
        }
        public List<DeviceReceipt> SearchWaitingClients(
    string keyword)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = 'WaitingClient'
        AND
        (
            CAST(ReceiptID AS NVARCHAR) LIKE @Keyword
            OR PhoneNumber LIKE @Keyword
            OR CustomerName LIKE @Keyword
            OR ReceiptNumber LIKE @Keyword
            OR SerialNumber LIKE @Keyword
        )
        ORDER BY ReceiveDate DESC";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Keyword",
                        SqlDbType.NVarChar).Value =
                        "%" + keyword + "%";

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public List<DeviceReceipt> GetInProgressDevices(
    int page,
    int pageSize)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = 'InProgress'
        ORDER BY ReceiveDate DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Offset",
                        SqlDbType.Int).Value =
                        (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize",
                        SqlDbType.Int).Value =
                        pageSize;

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetInProgressDevicesCount()
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT COUNT(*)
        FROM DeviceReceipts
        WHERE DeviceStatus = 'InProgress'";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    con.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar());
                }
            }
        }
        public void MoveToCompleted(int receiptID)
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        UPDATE DeviceReceipts
        SET DeviceStatus = 'Completed'
        WHERE ReceiptID = @ReceiptID";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ReceiptID",
                        SqlDbType.Int).Value =
                        receiptID;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<DeviceReceipt> SearchInProgressDevices(
    string keyword)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = N'InProgress'
        AND
        (
            CAST(ReceiptID AS NVARCHAR) LIKE @Keyword
            OR PhoneNumber LIKE @Keyword
            OR CustomerName LIKE @Keyword
            OR ReceiptNumber LIKE @Keyword
            OR SerialNumber LIKE @Keyword
        )
        ORDER BY ReceiveDate DESC;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Keyword",
                        SqlDbType.NVarChar).Value =
                        "%" + keyword + "%";

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public List<DeviceReceipt> GetCompletedDevices(
    int page,
    int pageSize)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = 'Completed'
        ORDER BY ReceiveDate DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Offset",
                        SqlDbType.Int).Value =
                        (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize",
                        SqlDbType.Int).Value =
                        pageSize;

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetCompletedCount()
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT COUNT(*)
        FROM DeviceReceipts
        WHERE DeviceStatus = 'Completed'";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    con.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar());
                }
            }
        }
        public List<DeviceReceipt> SearchCompletedDevices(
    string keyword)
        {
            List<DeviceReceipt> list =
                new List<DeviceReceipt>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT
            ReceiptID,
            CustomerName,
            PhoneNumber,
            ProductType,
            ProblemDescription,
            DeviceStatus,
            ReceiptNumber,
            SerialNumber,
            ReceiveDate,
            ReceivedBy
        FROM DeviceReceipts
        WHERE DeviceStatus = 'Completed'
        AND
        (
            CAST(ReceiptID AS NVARCHAR) LIKE @Keyword
            OR PhoneNumber LIKE @Keyword
            OR CustomerName LIKE @Keyword
            OR ReceiptNumber LIKE @Keyword
            OR SerialNumber LIKE @Keyword
        )
        ORDER BY ReceiveDate DESC;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Keyword",
                        SqlDbType.NVarChar).Value =
                        "%" + keyword + "%";

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DeviceReceipt
                            {
                                ReceiptID =
                                    Convert.ToInt32(reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"].ToString(),

                                PhoneNumber =
                                    reader["PhoneNumber"].ToString(),

                                ProductType =
                                    reader["ProductType"].ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"].ToString(),

                                DeviceStatus =
                                    reader["DeviceStatus"].ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"].ToString(),

                                SerialNumber =
                                    reader["SerialNumber"].ToString(),

                                ReceiveDate =
                                    Convert.ToDateTime(reader["ReceiveDate"]),

                                ReceivedBy =
                                    reader["ReceivedBy"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public void DeliverDevice(int receiptID)
        {
            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        UPDATE DeviceReceipts
        SET DeviceStatus = 'Delivered'
        WHERE ReceiptID = @ReceiptID";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ReceiptID",
                        SqlDbType.Int).Value =
                        receiptID;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<RepairWarranty> GetRepairs(
    int page,
    int pageSize)
        {
            List<RepairWarranty> list =
                new List<RepairWarranty>();

            using (SqlConnection con =
                DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT 
            r.RepairID,
            r.ReceiptID,
            d.CustomerName,
            d.ProblemDescription,
            d.ReceiptNumber,
            d.SerialNumber,
            r.WorkDescription,
            r.ClientNotes,
            r.RepairCost,
            r.PaymentMethod,
            r.DeliveryDate,
            r.WarrantyPeriod
        FROM Repairs r
        INNER JOIN DeviceReceipts d 
            ON r.ReceiptID = d.ReceiptID
        ORDER BY r.RepairID DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Offset",
                        SqlDbType.Int).Value =
                        (page - 1) * pageSize;

                    cmd.Parameters.Add("@PageSize",
                        SqlDbType.Int).Value =
                        pageSize;

                    con.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RepairWarranty
                            {
                                RepairID =
                                    Convert.ToInt32(
                                        reader["RepairID"]),

                                ReceiptID =
                                    Convert.ToInt32(
                                        reader["ReceiptID"]),

                                CustomerName =
                                    reader["CustomerName"]
                                    .ToString(),

                                ProblemDescription =
                                    reader["ProblemDescription"]
                                    .ToString(),

                                ReceiptNumber =
                                    reader["ReceiptNumber"]
                                    .ToString(),

                                SerialNumber =
                                    reader["SerialNumber"]
                                    .ToString(),

                                WorkDescription =
                                    reader["WorkDescription"]
                                    .ToString(),

                                ClientNotes =
                                    reader["ClientNotes"]
                                    .ToString(),

                                RepairCost =
                                    Convert.ToDecimal(
                                        reader["RepairCost"]),

                                PaymentMethod =
                                    reader["PaymentMethod"]
                                    .ToString(),

                                DeliveryDate =
                                    Convert.ToDateTime(
                                        reader["DeliveryDate"]),

                                WarrantyPeriod =
                                    reader["WarrantyPeriod"]
                                    .ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public int GetRepairsCount()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Repairs";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public List<RepairWarranty> SearchRepairs(string keyword)
        {
            List<RepairWarranty> list = new List<RepairWarranty>();

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"
                SELECT 
                    r.RepairID,
                    r.ReceiptID,
                    d.CustomerName,
                    d.ProblemDescription,
                    d.ReceiptNumber,
                    d.SerialNumber,
                    r.WorkDescription,
                    r.ClientNotes,
                    r.RepairCost,
                    r.PaymentMethod,
                    r.DeliveryDate,
                    r.WarrantyPeriod
                FROM Repairs r
                INNER JOIN DeviceReceipts d 
                    ON r.ReceiptID = d.ReceiptID
                WHERE
                    CAST(r.RepairID AS NVARCHAR) LIKE @Keyword
                    OR CAST(r.ReceiptID AS NVARCHAR) LIKE @Keyword
                    OR d.CustomerName LIKE @Keyword
                    OR d.ReceiptNumber LIKE @Keyword
                    OR d.SerialNumber LIKE @Keyword
                ORDER BY r.RepairID DESC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar).Value =
                        "%" + keyword + "%";

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new RepairWarranty
                            {
                                RepairID = Convert.ToInt32(reader["RepairID"]),
                                ReceiptID = Convert.ToInt32(reader["ReceiptID"]),
                                CustomerName = reader["CustomerName"].ToString(),
                                ProblemDescription = reader["ProblemDescription"].ToString(),
                                ReceiptNumber = reader["ReceiptNumber"].ToString(),
                                SerialNumber = reader["SerialNumber"].ToString(),
                                WorkDescription = reader["WorkDescription"].ToString(),
                                ClientNotes = reader["ClientNotes"].ToString(),
                                RepairCost = Convert.ToDecimal(reader["RepairCost"]),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                                WarrantyPeriod = reader["WarrantyPeriod"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
