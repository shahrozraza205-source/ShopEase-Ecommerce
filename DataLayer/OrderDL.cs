// ============================================================
//   DataLayer/OrderDL.cs
//   All database operations for Orders & Payments
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EcommerceStore.Models;

namespace EcommerceStore.DataLayer
{
    public class OrderDL
    {
        // Place a new order
        public static int PlaceOrder(int userID, decimal totalAmount, List<CartItem> items, string paymentMethod)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                // Insert into Orders
                string orderQuery = "INSERT INTO Orders (UserID, TotalAmount, Status) VALUES (@uid, @total, 'Pending')";
                MySqlCommand orderCmd = new MySqlCommand(orderQuery, conn, transaction);
                orderCmd.Parameters.AddWithValue("@uid",   userID);
                orderCmd.Parameters.AddWithValue("@total", totalAmount);
                orderCmd.ExecuteNonQuery();
                int orderID = Convert.ToInt32(orderCmd.LastInsertedId);

                // Insert into OrderItems
                foreach (var item in items)
                {
                    string itemQuery = "INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice) " +
                                       "VALUES (@oid, @pid, @qty, @price)";
                    MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn, transaction);
                    itemCmd.Parameters.AddWithValue("@oid",   orderID);
                    itemCmd.Parameters.AddWithValue("@pid",   item.ProductID);
                    itemCmd.Parameters.AddWithValue("@qty",   item.Quantity);
                    itemCmd.Parameters.AddWithValue("@price", item.Price);
                    itemCmd.ExecuteNonQuery();

                    // Reduce stock
                    string stockQuery = "UPDATE Products SET Stock = Stock - @qty WHERE ProductID=@pid";
                    MySqlCommand stockCmd = new MySqlCommand(stockQuery, conn, transaction);
                    stockCmd.Parameters.AddWithValue("@qty", item.Quantity);
                    stockCmd.Parameters.AddWithValue("@pid", item.ProductID);
                    stockCmd.ExecuteNonQuery();
                }

                // Insert into Payments
                string payQuery = "INSERT INTO Payments (OrderID, Amount, PaymentMethod, PaymentStatus) " +
                                  "VALUES (@oid, @amt, @method, 'Pending')";
                MySqlCommand payCmd = new MySqlCommand(payQuery, conn, transaction);
                payCmd.Parameters.AddWithValue("@oid",    orderID);
                payCmd.Parameters.AddWithValue("@amt",    totalAmount);
                payCmd.Parameters.AddWithValue("@method", paymentMethod);
                payCmd.ExecuteNonQuery();

                transaction.Commit();
                conn.Close();
                return orderID;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Order Error: " + ex.Message);
                return -1;
            }
        }

        // Get all orders for a user
        public static List<Order> GetUserOrders(int userID)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Orders WHERE UserID=@uid ORDER BY OrderDate DESC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uid", userID);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderID     = Convert.ToInt32(reader["OrderID"]),
                            UserID      = Convert.ToInt32(reader["UserID"]),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            Status      = reader["Status"].ToString(),
                            OrderDate   = Convert.ToDateTime(reader["OrderDate"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return orders;
        }

        // Admin: Get all orders
        public static List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = @"SELECT o.*, u.FullName FROM Orders o 
                                     JOIN Users u ON o.UserID = u.UserID 
                                     ORDER BY o.OrderDate DESC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderID     = Convert.ToInt32(reader["OrderID"]),
                            UserID      = Convert.ToInt32(reader["UserID"]),
                            CustomerName= reader["FullName"].ToString(),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            Status      = reader["Status"].ToString(),
                            OrderDate   = Convert.ToDateTime(reader["OrderDate"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return orders;
        }

        // Update order status (Admin)
        public static bool UpdateOrderStatus(int orderID, string status)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "UPDATE Orders SET Status=@status WHERE OrderID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id",     orderID);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }
    }
}
