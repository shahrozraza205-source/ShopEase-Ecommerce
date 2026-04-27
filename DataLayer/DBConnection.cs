// ============================================================
//   DataLayer/DBConnection.cs
//   Handles MySQL database connection
// ============================================================

using System;
using MySql.Data.MySqlClient;

namespace EcommerceStore.DataLayer
{
    public class DBConnection
    {
        private static string connectionString =
            "Server=localhost;Database=ecommerce_db;Uid=root;Pwd=your_password;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database Connection Error: " + ex.Message);
                return null;
            }
        }
    }
}
