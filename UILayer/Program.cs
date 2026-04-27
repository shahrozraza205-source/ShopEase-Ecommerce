// ============================================================
//   UILayer/Program.cs
//   Main Console UI — ShopEase Ecommerce System
//   Author: Shahroz Raza | UET Lahore
// ============================================================

using System;
using System.Collections.Generic;
using EcommerceStore.BusinessLayer;
using EcommerceStore.DataLayer;
using EcommerceStore.Models;

namespace EcommerceStore
{
    class Program
    {
        static User currentUser = null;

        static void Main(string[] args)
        {
            Console.Title = "ShopEase — Ecommerce Store";
            while (true)
            {
                ShowMainMenu();
            }
        }

        // ─────────────────────────────────────────
        //  MAIN MENU
        // ─────────────────────────────────────────
        static void ShowMainMenu()
        {
            Console.Clear();
            PrintBanner();
            Console.WriteLine("  [1] Login");
            Console.WriteLine("  [2] Register");
            Console.WriteLine("  [0] Exit");
            Console.Write("\n  Choose: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": LoginMenu();    break;
                case "2": RegisterMenu(); break;
                case "0": Console.WriteLine("\n  Goodbye! 👋"); Environment.Exit(0); break;
                default:  Console.WriteLine("  Invalid choice."); Pause(); break;
            }
        }

        // ─────────────────────────────────────────
        //  REGISTER
        // ─────────────────────────────────────────
        static void RegisterMenu()
        {
            Console.Clear();
            PrintHeader("REGISTER");
            Console.Write("  Full Name   : "); string name    = Console.ReadLine();
            Console.Write("  Email       : "); string email   = Console.ReadLine();
            Console.Write("  Password    : "); string pass    = Console.ReadLine();
            Console.Write("  Phone       : "); string phone   = Console.ReadLine();
            Console.Write("  Address     : "); string address = Console.ReadLine();

            var (success, message) = UserBL.Register(name, email, pass, phone, address);
            PrintResult(success, message);
            Pause();
        }

        // ─────────────────────────────────────────
        //  LOGIN
        // ─────────────────────────────────────────
        static void LoginMenu()
        {
            Console.Clear();
            PrintHeader("LOGIN");
            Console.Write("  Email    : "); string email = Console.ReadLine();
            Console.Write("  Password : "); string pass  = Console.ReadLine();

            var (user, message) = UserBL.Login(email, pass);
            if (user != null)
            {
                currentUser = user;
                PrintResult(true, $"Welcome, {user.FullName}!");
                Pause();
                if (user.Role == "Admin") AdminDashboard();
                else                     CustomerDashboard();
            }
            else
            {
                PrintResult(false, message);
                Pause();
            }
        }

        // ─────────────────────────────────────────
        //  CUSTOMER DASHBOARD
        // ─────────────────────────────────────────
        static void CustomerDashboard()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader($"CUSTOMER MENU — {currentUser.FullName}");
                Console.WriteLine("  [1] Browse All Products");
                Console.WriteLine("  [2] Search Products");
                Console.WriteLine("  [3] View My Cart");
                Console.WriteLine("  [4] Place Order");
                Console.WriteLine("  [5] My Orders");
                Console.WriteLine("  [0] Logout");
                Console.Write("\n  Choose: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": BrowseProducts();     break;
                    case "2": SearchProducts();     break;
                    case "3": ViewCart();           break;
                    case "4": PlaceOrder();         break;
                    case "5": ViewMyOrders();       break;
                    case "0": currentUser = null; return;
                    default:  Console.WriteLine("  Invalid."); Pause(); break;
                }
            }
        }

        // ─────────────────────────────────────────
        //  BROWSE PRODUCTS
        // ─────────────────────────────────────────
        static void BrowseProducts()
        {
            Console.Clear();
            PrintHeader("ALL PRODUCTS");
            var products = ProductDL.GetAllProducts();
            if (products.Count == 0) { Console.WriteLine("  No products found."); Pause(); return; }

            PrintProductTable(products);

            Console.Write("\n  Enter Product ID to add to cart (0 to go back): ");
            if (int.TryParse(Console.ReadLine(), out int pid) && pid != 0)
            {
                Console.Write("  Quantity: ");
                if (int.TryParse(Console.ReadLine(), out int qty))
                {
                    var (success, msg) = CartBL.AddToCart(currentUser.UserID, pid, qty);
                    PrintResult(success, msg);
                }
            }
            Pause();
        }

        // ─────────────────────────────────────────
        //  SEARCH PRODUCTS
        // ─────────────────────────────────────────
        static void SearchProducts()
        {
            Console.Clear();
            PrintHeader("SEARCH PRODUCTS");
            Console.Write("  Enter keyword: ");
            string keyword = Console.ReadLine();
            var products = ProductDL.SearchProducts(keyword);
            if (products.Count == 0) { Console.WriteLine("  No products found."); Pause(); return; }
            PrintProductTable(products);

            Console.Write("\n  Enter Product ID to add to cart (0 to skip): ");
            if (int.TryParse(Console.ReadLine(), out int pid) && pid != 0)
            {
                Console.Write("  Quantity: ");
                if (int.TryParse(Console.ReadLine(), out int qty))
                {
                    var (success, msg) = CartBL.AddToCart(currentUser.UserID, pid, qty);
                    PrintResult(success, msg);
                }
            }
            Pause();
        }

        // ─────────────────────────────────────────
        //  VIEW CART
        // ─────────────────────────────────────────
        static void ViewCart()
        {
            Console.Clear();
            PrintHeader("MY CART");
            var items = CartBL.ViewCart(currentUser.UserID);
            if (items.Count == 0) { Console.WriteLine("  Your cart is empty."); Pause(); return; }

            Console.WriteLine($"  {"ID",-5} {"Product",-25} {"Price",-12} {"Qty",-6} {"Total",-12}");
            Console.WriteLine("  " + new string('─', 62));
            foreach (var item in items)
                Console.WriteLine($"  {item.CartID,-5} {item.ProductName,-25} Rs.{item.Price,-9} {item.Quantity,-6} Rs.{item.Total,-9}");
            Console.WriteLine("  " + new string('─', 62));
            Console.WriteLine($"  {"TOTAL",-40} Rs.{CartBL.GetCartTotal(currentUser.UserID)}");

            Console.Write("\n  Remove item? Enter Cart ID (0 to go back): ");
            if (int.TryParse(Console.ReadLine(), out int cid) && cid != 0)
            {
                var (success, msg) = CartBL.RemoveItem(cid);
                PrintResult(success, msg);
            }
            Pause();
        }

        // ─────────────────────────────────────────
        //  PLACE ORDER
        // ─────────────────────────────────────────
        static void PlaceOrder()
        {
            Console.Clear();
            PrintHeader("PLACE ORDER");
            decimal total = CartBL.GetCartTotal(currentUser.UserID);
            if (total == 0) { Console.WriteLine("  Cart is empty!"); Pause(); return; }

            Console.WriteLine($"  Order Total: Rs. {total}");
            Console.WriteLine("\n  Payment Method:");
            Console.WriteLine("  [1] Cash on Delivery");
            Console.WriteLine("  [2] Easypaisa");
            Console.WriteLine("  [3] JazzCash");
            Console.WriteLine("  [4] Credit Card");
            Console.Write("\n  Choose payment: ");
            string payChoice = Console.ReadLine();
            string[] methods = { "", "Cash on Delivery", "Easypaisa", "JazzCash", "Credit Card" };
            string method = (payChoice == "1" || payChoice == "2" || payChoice == "3" || payChoice == "4")
                            ? methods[int.Parse(payChoice)] : "Cash on Delivery";

            Console.Write($"\n  Confirm order with '{method}'? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                var (success, msg, orderID) = OrderBL.PlaceOrder(currentUser.UserID, method);
                PrintResult(success, msg);
            }
            Pause();
        }

        // ─────────────────────────────────────────
        //  MY ORDERS
        // ─────────────────────────────────────────
        static void ViewMyOrders()
        {
            Console.Clear();
            PrintHeader("MY ORDERS");
            var orders = OrderBL.GetMyOrders(currentUser.UserID);
            if (orders.Count == 0) { Console.WriteLine("  No orders found."); Pause(); return; }

            Console.WriteLine($"  {"OrderID",-10} {"Total",-15} {"Status",-15} {"Date",-20}");
            Console.WriteLine("  " + new string('─', 62));
            foreach (var o in orders)
                Console.WriteLine($"  #{o.OrderID,-9} Rs.{o.TotalAmount,-12} {o.Status,-15} {o.OrderDate:dd-MM-yyyy HH:mm}");
            Pause();
        }

        // ─────────────────────────────────────────
        //  ADMIN DASHBOARD
        // ─────────────────────────────────────────
        static void AdminDashboard()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("ADMIN PANEL");
                Console.WriteLine("  [1] View All Products");
                Console.WriteLine("  [2] Add New Product");
                Console.WriteLine("  [3] Delete Product");
                Console.WriteLine("  [4] View All Orders");
                Console.WriteLine("  [5] Update Order Status");
                Console.WriteLine("  [0] Logout");
                Console.Write("\n  Choose: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AdminViewProducts();    break;
                    case "2": AdminAddProduct();      break;
                    case "3": AdminDeleteProduct();   break;
                    case "4": AdminViewOrders();      break;
                    case "5": AdminUpdateOrder();     break;
                    case "0": currentUser = null; return;
                    default:  Console.WriteLine("  Invalid."); Pause(); break;
                }
            }
        }

        static void AdminViewProducts()
        {
            Console.Clear();
            PrintHeader("ALL PRODUCTS (ADMIN)");
            var products = ProductDL.GetAllProducts();
            PrintProductTable(products);
            Pause();
        }

        static void AdminAddProduct()
        {
            Console.Clear();
            PrintHeader("ADD PRODUCT");
            Console.Write("  Category ID  : "); int.TryParse(Console.ReadLine(), out int catID);
            Console.Write("  Product Name : "); string name = Console.ReadLine();
            Console.Write("  Description  : "); string desc = Console.ReadLine();
            Console.Write("  Price (Rs.)  : "); decimal.TryParse(Console.ReadLine(), out decimal price);
            Console.Write("  Stock        : "); int.TryParse(Console.ReadLine(), out int stock);

            bool result = ProductDL.AddProduct(new Product
            { CategoryID = catID, ProductName = name, Description = desc, Price = price, Stock = stock });
            PrintResult(result, result ? "Product added!" : "Failed to add product.");
            Pause();
        }

        static void AdminDeleteProduct()
        {
            Console.Clear();
            PrintHeader("DELETE PRODUCT");
            AdminViewProducts();
            Console.Write("  Enter Product ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                bool result = ProductDL.DeleteProduct(pid);
                PrintResult(result, result ? "Product deleted." : "Failed.");
            }
            Pause();
        }

        static void AdminViewOrders()
        {
            Console.Clear();
            PrintHeader("ALL ORDERS");
            var orders = OrderBL.GetAllOrders();
            Console.WriteLine($"  {"OrderID",-10} {"Customer",-20} {"Total",-12} {"Status",-15} {"Date",-15}");
            Console.WriteLine("  " + new string('─', 74));
            foreach (var o in orders)
                Console.WriteLine($"  #{o.OrderID,-9} {o.CustomerName,-20} Rs.{o.TotalAmount,-9} {o.Status,-15} {o.OrderDate:dd-MM-yyyy}");
            Pause();
        }

        static void AdminUpdateOrder()
        {
            Console.Clear();
            PrintHeader("UPDATE ORDER STATUS");
            Console.Write("  Order ID : "); int.TryParse(Console.ReadLine(), out int oid);
            Console.WriteLine("  Statuses : Pending | Processing | Shipped | Delivered | Cancelled");
            Console.Write("  New Status: "); string status = Console.ReadLine();
            var (success, msg) = OrderBL.UpdateStatus(oid, status);
            PrintResult(success, msg);
            Pause();
        }

        // ─────────────────────────────────────────
        //  HELPER UI METHODS
        // ─────────────────────────────────────────
        static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  ╔══════════════════════════════════════════════╗
  ║      🛒  ShopEase — Ecommerce Store          ║
  ║      C# Console + MySQL Backend              ║
  ║      Author: Shahroz Raza | UET Lahore       ║
  ╚══════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void PrintHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ══════════ {title} ══════════\n");
            Console.ResetColor();
        }

        static void PrintResult(bool success, string message)
        {
            Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"\n  {(success ? "✔" : "✘")} {message}");
            Console.ResetColor();
        }

        static void PrintProductTable(List<Product> products)
        {
            Console.WriteLine($"  {"ID",-5} {"Name",-25} {"Category",-15} {"Price",-12} {"Stock",-6}");
            Console.WriteLine("  " + new string('─', 65));
            foreach (var p in products)
                Console.WriteLine($"  {p.ProductID,-5} {p.ProductName,-25} {p.CategoryName,-15} Rs.{p.Price,-9} {p.Stock,-6}");
        }

        static void Pause()
        {
            Console.Write("\n  Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
