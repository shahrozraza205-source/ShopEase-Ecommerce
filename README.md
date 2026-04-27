# 🛒 ShopEase — Ecommerce Store

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Status](https://img.shields.io/badge/Status-Complete-00ff88?style=for-the-badge)

> A fully functional **Console-based Ecommerce System** built with **C# .NET** and **MySQL** backend, following a clean **3-Layer Architecture** (Data Layer → Business Layer → UI Layer).

---

## 📁 Project Structure

```
EcommerceStore/
├── Database/
│   └── ecommerce_db.sql        ← MySQL schema + sample data
├── Models/
│   └── Models.cs               ← User, Product, Cart, Order, Payment models
├── DataLayer/
│   ├── DBConnection.cs         ← MySQL connection handler
│   ├── UserDL.cs               ← User database operations
│   ├── ProductDL.cs            ← Product database operations
│   ├── CartDL.cs               ← Cart database operations
│   └── OrderDL.cs              ← Order & Payment operations
├── BusinessLayer/
│   ├── UserBL.cs               ← User business logic & validation
│   └── CartOrderBL.cs          ← Cart & Order business logic
├── UILayer/
│   └── Program.cs              ← Main console UI
└── README.md
```

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔐 User Auth | Register & Login with role-based access |
| 📦 Products | Browse, search & filter products by category |
| 🛒 Shopping Cart | Add, remove, update quantities |
| 📋 Orders | Place orders with transaction handling |
| 💳 Payments | Cash on Delivery, Easypaisa, JazzCash, Card |
| 🛠️ Admin Panel | Manage products, view & update all orders |

---

## 🗄️ Database Schema (ERD Overview)

```
Users ──────┬──── Cart ────── Products ──── Categories
            │
            └──── Orders ─── OrderItems ─── Products
                      │
                      └──── Payments
```

---

## 🚀 How to Run

### Step 1 — Setup Database
1. Open **MySQL Workbench**
2. Run `Database/ecommerce_db.sql`
3. Database `ecommerce_db` will be created with sample data

### Step 2 — Configure Connection
Open `DataLayer/DBConnection.cs` and update:
```csharp
"Server=localhost;Database=ecommerce_db;Uid=root;Pwd=YOUR_PASSWORD;"
```

### Step 3 — Install MySQL NuGet Package
```bash
Install-Package MySql.Data
```

### Step 4 — Run
Press `F5` in Visual Studio or:
```bash
dotnet run
```

---

## 🖥️ Console Screenshots

```
╔══════════════════════════════════════════════╗
║      🛒  ShopEase — Ecommerce Store          ║
║      C# Console + MySQL Backend              ║
║      Author: Shahroz Raza | UET Lahore       ║
╚══════════════════════════════════════════════╝

  [1] Login
  [2] Register
  [0] Exit
```

---

## 🛠️ Tech Stack

- **Language:** C# (.NET)
- **Database:** MySQL
- **Architecture:** 3-Layer (DL → BL → UI)
- **IDE:** Visual Studio 2022
- **DB Tool:** MySQL Workbench

---

## 👨‍💻 Author

**Muhammad Shahroz Raza**  
BS Data Science | UET Lahore  
📧 shahrozraza307@gmail.com  
🔗 [LinkedIn](https://www.linkedin.com/in/shahroz-raza-600538402) | [GitHub](https://github.com/shahrozraza205-source)
