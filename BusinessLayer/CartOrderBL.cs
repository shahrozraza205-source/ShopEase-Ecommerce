// ============================================================
//   BusinessLayer/CartBL.cs
//   Business logic for Cart & Order
// ============================================================

using System;
using System.Collections.Generic;
using EcommerceStore.DataLayer;
using EcommerceStore.Models;

namespace EcommerceStore.BusinessLayer
{
    public class CartBL
    {
        public static (bool success, string message) AddToCart(int userID, int productID, int quantity)
        {
            if (quantity <= 0) return (false, "Quantity must be at least 1.");
            bool result = CartDL.AddToCart(userID, productID, quantity);
            return result ? (true, "Product added to cart!") : (false, "Failed to add product.");
        }

        public static List<CartItem> ViewCart(int userID)
        {
            return CartDL.GetCartItems(userID);
        }

        public static decimal GetCartTotal(int userID)
        {
            var items = CartDL.GetCartItems(userID);
            decimal total = 0;
            foreach (var item in items) total += item.Total;
            return total;
        }

        public static (bool success, string message) RemoveItem(int cartID)
        {
            bool result = CartDL.RemoveFromCart(cartID);
            return result ? (true, "Item removed.") : (false, "Failed to remove item.");
        }
    }

    public class OrderBL
    {
        public static (bool success, string message, int orderID) PlaceOrder(int userID, string paymentMethod)
        {
            var items = CartDL.GetCartItems(userID);
            if (items.Count == 0)
                return (false, "Your cart is empty!", -1);

            decimal total = 0;
            foreach (var item in items) total += item.Total;

            int orderID = OrderDL.PlaceOrder(userID, total, items, paymentMethod);
            if (orderID > 0)
            {
                CartDL.ClearCart(userID);
                return (true, $"Order placed! Order ID: #{orderID}", orderID);
            }
            return (false, "Failed to place order. Try again.", -1);
        }

        public static List<Order> GetMyOrders(int userID)
        {
            return OrderDL.GetUserOrders(userID);
        }

        public static List<Order> GetAllOrders()
        {
            return OrderDL.GetAllOrders();
        }

        public static (bool success, string message) UpdateStatus(int orderID, string status)
        {
            bool result = OrderDL.UpdateOrderStatus(orderID, status);
            return result ? (true, "Order status updated.") : (false, "Update failed.");
        }
    }
}
