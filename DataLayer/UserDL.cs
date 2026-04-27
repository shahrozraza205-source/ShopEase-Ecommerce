// ============================================================
//   DataLayer/UserDL.cs
//   All database operations for Users
// ============================================================

using System;
using MySql.Data.MySqlClient;
using EcommerceStore.Models;

namespace EcommerceStore.DataLayer
{
    public class UserDL
    {
        // Register new user
        public static bool RegisterUser(User user)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash, Phone, Address, Role) " +
                                   "VALUES (@name, @email, @pass, @phone, @address, 'Customer')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name",    user.FullName);
                    cmd.Parameters.AddWithValue("@email",   user.Email);
                    cmd.Parameters.AddWithValue("@pass",    user.PasswordHash);
                    cmd.Parameters.AddWithValue("@phone",   user.Phone);
                    cmd.Parameters.AddWithValue("@address", user.Address);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // Login user — returns User object if found
        public static User LoginUser(string email, string password)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Users WHERE Email=@email AND PasswordHash=@pass";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pass",  password);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserID       = Convert.ToInt32(reader["UserID"]),
                            FullName     = reader["FullName"].ToString(),
                            Email        = reader["Email"].ToString(),
                            Phone        = reader["Phone"].ToString(),
                            Address      = reader["Address"].ToString(),
                            Role         = reader["Role"].ToString()
                        };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        // Check if email already exists
        public static bool EmailExists(string email)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Email=@email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch { return false; }
        }
    }
}
