using Maintenance_program_template.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using technicalSupport;

public class SparePartRepository
{
    public void AddSparePart(string name, int quantity, string supplier, decimal price)
    {
        string query = @"INSERT INTO SpareParts 
                        (PartName, Quantity, Supplier, Price)
                        VALUES 
                        (@PartName, @Quantity, @Supplier, @Price)";

        using (SqlConnection con = DatabaseHelper.GetConnection())
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.Add("@PartName", SqlDbType.NVarChar).Value = name;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
            cmd.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = supplier;
            cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public int GetNextPartId()
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        using (SqlCommand cmd = new SqlCommand(
            "SELECT ISNULL(MAX(PartID), 0) + 1 FROM SpareParts WITH (UPDLOCK, HOLDLOCK)", con))
        {
            con.Open();
            return (int)cmd.ExecuteScalar();
        }
    }
    public DataTable GetSuppliers()
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT SupplierID, SupplierName FROM Suppliers", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
    }
    public DataTable GetParts(int pageNumber, int pageSize)
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = @"SELECT 
                            PartID AS [معرف القطعة], 
                            PartName AS [أسم القطعة], 
                            Quantity AS [الكمية], 
                            Supplier AS [الشركة الموردة], 
                            Price AS [السعر] 
                         FROM SpareParts
                         ORDER BY PartID DESC
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
    public SparePart SearchByName(string name)
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = @"SELECT TOP 1 PartID, PartName, Quantity, Supplier, Price
                         FROM SpareParts
                         WHERE PartName LIKE @Name";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new SparePart
                {
                    PartID = Convert.ToInt32(dr["PartID"]),
                    PartName = dr["PartName"].ToString(),
                    Quantity = Convert.ToInt32(dr["Quantity"]),
                    Supplier = dr["Supplier"].ToString(),
                    Price = Convert.ToDecimal(dr["Price"])
                };
            }

            return null;
        }
    }
    public void UpdatePart(SparePart part)
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = @"UPDATE SpareParts
                         SET PartName = @PartName,
                             Quantity = @Quantity,
                             Supplier = @Supplier,
                             Price = @Price
                         WHERE PartID = @PartID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@PartID", part.PartID);
            cmd.Parameters.AddWithValue("@PartName", part.PartName);
            cmd.Parameters.AddWithValue("@Quantity", part.Quantity);
            cmd.Parameters.AddWithValue("@Supplier", part.Supplier);
            cmd.Parameters.AddWithValue("@Price", part.Price);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
    //public bool IsPartUsed(int partId)
    //{
    //    using (SqlConnection con = DatabaseHelper.GetConnection())
    //    {
    //        string query = @"SELECT COUNT(*) 
    //                     FROM SpareParts
    //                     WHERE PartID = @PartID";

    //        SqlCommand cmd = new SqlCommand(query, con);
    //        cmd.Parameters.AddWithValue("@PartID", partId);

    //        con.Open();

    //        int count = (int)cmd.ExecuteScalar();

    //        return count > 0;
    //    }
    //}
    public void DeletePart(int partId)
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = "DELETE FROM SpareParts WHERE PartID = @PartID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PartID", partId);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
    public DataTable GetSpareParts()
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = "SELECT PartID, PartName FROM SpareParts";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
    }
    public decimal GetPartPrice(int partId)
    {
        using (SqlConnection con = DatabaseHelper.GetConnection())
        {
            string query = "SELECT Price FROM SpareParts WHERE PartID = @ID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ID", partId);

            con.Open();

            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }
    }
}