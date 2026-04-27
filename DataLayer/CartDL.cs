// ============================================================
//   DataLayer/CartDL.cs
//   All database operations for Shopping Cart
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EcommerceStore.Models;

namespace EcommerceStore.DataLayer
{
    public class CartDL
    {
        // Add product to cart
        public static bool AddToCart(int userID, int productID, int quantity)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    // If product already in cart, update quantity
                    string checkQuery = "SELECT CartID FROM Cart WHERE UserID=@uid AND ProductID=@pid";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@uid", userID);
                    checkCmd.Parameters.AddWithValue("@pid", productID);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        string updateQuery = "UPDATE Cart SET Quantity = Quantity + @qty WHERE UserID=@uid AND ProductID=@pid";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@qty", quantity);
                        updateCmd.Parameters.AddWithValue("@uid", userID);
                        updateCmd.Parameters.AddWithValue("@pid", productID);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@uid, @pid, @qty)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@uid", userID);
                        insertCmd.Parameters.AddWithValue("@pid", productID);
                        insertCmd.Parameters.AddWithValue("@qty", quantity);
                        insertCmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }

        // Get all cart items for a user
        public static List<CartItem> GetCartItems(int userID)
        {
            List<CartItem> items = new List<CartItem>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = @"SELECT c.CartID, c.Quantity, p.ProductID, p.ProductName, p.Price
                                     FROM Cart c 
                                     JOIN Products p ON c.ProductID = p.ProductID
                                     WHERE c.UserID = @uid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uid", userID);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(new CartItem
                        {
                            CartID      = Convert.ToInt32(reader["CartID"]),
                            ProductID   = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            Price       = Convert.ToDecimal(reader["Price"]),
                            Quantity    = Convert.ToInt32(reader["Quantity"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return items;
        }

        // Remove item from cart
        public static bool RemoveFromCart(int cartID)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Cart WHERE CartID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", cartID);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }

        // Clear entire cart after order placed
        public static bool ClearCart(int userID)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Cart WHERE UserID=@uid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uid", userID);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }
    }
}
