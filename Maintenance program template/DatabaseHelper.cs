using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace technicalSupport
{
    public static class DatabaseHelper
    {
        // 🔹 عدّل اسم السيرفر وقاعدة البيانات حسب جهازك
        private static string connectionString =
            "Server=localhost;Database=MaintenanceCenter;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
