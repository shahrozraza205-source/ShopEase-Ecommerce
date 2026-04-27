// ============================================================
//   DataLayer/ProductDL.cs
//   All database operations for Products
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EcommerceStore.Models;

namespace EcommerceStore.DataLayer
{
    public class ProductDL
    {
        // Get all products
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = @"SELECT p.*, c.CategoryName 
                                     FROM Products p 
                                     JOIN Categories c ON p.CategoryID = c.CategoryID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID    = Convert.ToInt32(reader["ProductID"]),
                            CategoryID   = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            ProductName  = reader["ProductName"].ToString(),
                            Description  = reader["Description"].ToString(),
                            Price        = Convert.ToDecimal(reader["Price"]),
                            Stock        = Convert.ToInt32(reader["Stock"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return products;
        }

        // Search products by name
        public static List<Product> SearchProducts(string keyword)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = @"SELECT p.*, c.CategoryName 
                                     FROM Products p 
                                     JOIN Categories c ON p.CategoryID = c.CategoryID
                                     WHERE p.ProductName LIKE @keyword OR p.Description LIKE @keyword";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID    = Convert.ToInt32(reader["ProductID"]),
                            CategoryID   = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            ProductName  = reader["ProductName"].ToString(),
                            Description  = reader["Description"].ToString(),
                            Price        = Convert.ToDecimal(reader["Price"]),
                            Stock        = Convert.ToInt32(reader["Stock"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return products;
        }

        // Get products by category
        public static List<Product> GetByCategory(int categoryID)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = @"SELECT p.*, c.CategoryName 
                                     FROM Products p 
                                     JOIN Categories c ON p.CategoryID = c.CategoryID
                                     WHERE p.CategoryID = @catID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@catID", categoryID);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID    = Convert.ToInt32(reader["ProductID"]),
                            CategoryID   = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            ProductName  = reader["ProductName"].ToString(),
                            Description  = reader["Description"].ToString(),
                            Price        = Convert.ToDecimal(reader["Price"]),
                            Stock        = Convert.ToInt32(reader["Stock"])
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            return products;
        }

        // Add new product (Admin)
        public static bool AddProduct(Product p)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "INSERT INTO Products (CategoryID, ProductName, Description, Price, Stock) " +
                                   "VALUES (@cat, @name, @desc, @price, @stock)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cat",   p.CategoryID);
                    cmd.Parameters.AddWithValue("@name",  p.ProductName);
                    cmd.Parameters.AddWithValue("@desc",  p.Description);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@stock", p.Stock);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }

        // Delete product (Admin)
        public static bool DeleteProduct(int productID)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Products WHERE ProductID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", productID);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); return false; }
        }
    }
}
