-- ============================================================
--   ECOMMERCE STORE - MySQL Database Schema
--   Project: ShopEase Ecommerce System
--   Author : Shahroz Raza | UET Lahore
--   Stack  : C# Console + MySQL Backend
-- ============================================================

CREATE DATABASE IF NOT EXISTS ecommerce_db;
USE ecommerce_db;

-- ─────────────────────────────────────────
-- 1. CATEGORIES TABLE
-- ─────────────────────────────────────────
CREATE TABLE Categories (
    CategoryID   INT AUTO_INCREMENT PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL,
    Description  VARCHAR(255),
    CreatedAt    DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ─────────────────────────────────────────
-- 2. USERS TABLE
-- ─────────────────────────────────────────
CREATE TABLE Users (
    UserID       INT AUTO_INCREMENT PRIMARY KEY,
    FullName     VARCHAR(100) NOT NULL,
    Email        VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Phone        VARCHAR(20),
    Address      TEXT,
    Role         ENUM('Customer', 'Admin') DEFAULT 'Customer',
    CreatedAt    DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ─────────────────────────────────────────
-- 3. PRODUCTS TABLE
-- ─────────────────────────────────────────
CREATE TABLE Products (
    ProductID    INT AUTO_INCREMENT PRIMARY KEY,
    CategoryID   INT NOT NULL,
    ProductName  VARCHAR(150) NOT NULL,
    Description  TEXT,
    Price        DECIMAL(10, 2) NOT NULL,
    Stock        INT DEFAULT 0,
    ImageURL     VARCHAR(255),
    CreatedAt    DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE
);

-- ─────────────────────────────────────────
-- 4. CART TABLE
-- ─────────────────────────────────────────
CREATE TABLE Cart (
    CartID     INT AUTO_INCREMENT PRIMARY KEY,
    UserID     INT NOT NULL,
    ProductID  INT NOT NULL,
    Quantity   INT NOT NULL DEFAULT 1,
    AddedAt    DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID)    REFERENCES Users(UserID)    ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- ─────────────────────────────────────────
-- 5. ORDERS TABLE
-- ─────────────────────────────────────────
CREATE TABLE Orders (
    OrderID      INT AUTO_INCREMENT PRIMARY KEY,
    UserID       INT NOT NULL,
    TotalAmount  DECIMAL(10, 2) NOT NULL,
    Status       ENUM('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled') DEFAULT 'Pending',
    OrderDate    DATETIME DEFAULT CURRENT_TIMESTAMP,
    DeliveryDate DATETIME,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- ─────────────────────────────────────────
-- 6. ORDER ITEMS TABLE
-- ─────────────────────────────────────────
CREATE TABLE OrderItems (
    OrderItemID INT AUTO_INCREMENT PRIMARY KEY,
    OrderID     INT NOT NULL,
    ProductID   INT NOT NULL,
    Quantity    INT NOT NULL,
    UnitPrice   DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderID)   REFERENCES Orders(OrderID)   ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- ─────────────────────────────────────────
-- 7. PAYMENTS TABLE
-- ─────────────────────────────────────────
CREATE TABLE Payments (
    PaymentID     INT AUTO_INCREMENT PRIMARY KEY,
    OrderID       INT NOT NULL,
    Amount        DECIMAL(10, 2) NOT NULL,
    PaymentMethod ENUM('Cash on Delivery', 'Credit Card', 'Easypaisa', 'JazzCash') DEFAULT 'Cash on Delivery',
    PaymentStatus ENUM('Pending', 'Completed', 'Failed', 'Refunded') DEFAULT 'Pending',
    PaymentDate   DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE
);

-- ─────────────────────────────────────────
-- SAMPLE DATA
-- ─────────────────────────────────────────
INSERT INTO Categories (CategoryName, Description) VALUES
('Electronics',  'Mobile phones, laptops, gadgets'),
('Clothing',     'Men and women fashion'),
('Books',        'Academic and fiction books'),
('Home & Kitchen','Furniture and kitchen appliances'),
('Sports',       'Sports equipment and accessories');

INSERT INTO Users (FullName, Email, PasswordHash, Phone, Address, Role) VALUES
('Admin User',     'admin@shopease.com',   'admin123',    '03001234567', 'Lahore, Pakistan', 'Admin'),
('Shahroz Raza',   'shahroz@example.com',  'pass1234',    '03211234567', 'Lahore, Pakistan', 'Customer'),
('Ali Ahmed',      'ali@example.com',      'ali1234',     '03451234567', 'Karachi, Pakistan','Customer');

INSERT INTO Products (CategoryID, ProductName, Description, Price, Stock) VALUES
(1, 'Samsung Galaxy A54',  '6.4" AMOLED, 128GB Storage',         85000.00, 20),
(1, 'HP Laptop 15',        'Intel Core i5, 8GB RAM, 512GB SSD',  110000.00, 10),
(2, 'Men Casual Shirt',    'Cotton slim fit casual shirt',          1500.00, 50),
(3, 'Data Science Book',   'Python for Data Analysis - 3rd Ed',    2500.00, 30),
(4, 'Kitchen Blender',     'High speed 1000W electric blender',    8000.00, 15),
(5, 'Cricket Bat',         'English Willow Grade 1 bat',           5500.00, 25);
