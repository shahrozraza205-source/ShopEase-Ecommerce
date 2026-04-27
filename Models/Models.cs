// ============================================================
//   Models/Models.cs
//   All model classes for the Ecommerce System
// ============================================================

using System;

namespace EcommerceStore.Models
{
    public class User
    {
        public int    UserID       { get; set; }
        public string FullName     { get; set; }
        public string Email        { get; set; }
        public string PasswordHash { get; set; }
        public string Phone        { get; set; }
        public string Address      { get; set; }
        public string Role         { get; set; }
    }

    public class Category
    {
        public int    CategoryID   { get; set; }
        public string CategoryName { get; set; }
        public string Description  { get; set; }
    }

    public class Product
    {
        public int     ProductID    { get; set; }
        public int     CategoryID   { get; set; }
        public string  CategoryName { get; set; }
        public string  ProductName  { get; set; }
        public string  Description  { get; set; }
        public decimal Price        { get; set; }
        public int     Stock        { get; set; }
    }

    public class CartItem
    {
        public int     CartID      { get; set; }
        public int     ProductID   { get; set; }
        public string  ProductName { get; set; }
        public decimal Price       { get; set; }
        public int     Quantity    { get; set; }
        public decimal Total       => Price * Quantity;
    }

    public class Order
    {
        public int      OrderID      { get; set; }
        public int      UserID       { get; set; }
        public string   CustomerName { get; set; }
        public decimal  TotalAmount  { get; set; }
        public string   Status       { get; set; }
        public DateTime OrderDate    { get; set; }
    }

    public class Payment
    {
        public int      PaymentID     { get; set; }
        public int      OrderID       { get; set; }
        public decimal  Amount        { get; set; }
        public string   PaymentMethod { get; set; }
        public string   PaymentStatus { get; set; }
        public DateTime PaymentDate   { get; set; }
    }
}
