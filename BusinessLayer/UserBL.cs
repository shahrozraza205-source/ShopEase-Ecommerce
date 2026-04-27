// ============================================================
//   BusinessLayer/UserBL.cs
//   Business logic for User operations
// ============================================================

using System;
using EcommerceStore.DataLayer;
using EcommerceStore.Models;

namespace EcommerceStore.BusinessLayer
{
    public class UserBL
    {
        public static (bool success, string message) Register(string name, string email, string password, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(name))     return (false, "Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email))    return (false, "Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(password)) return (false, "Password cannot be empty.");
            if (password.Length < 6)                 return (false, "Password must be at least 6 characters.");
            if (!email.Contains("@"))                return (false, "Invalid email format.");
            if (UserDL.EmailExists(email))           return (false, "Email already registered.");

            User user = new User
            {
                FullName     = name,
                Email        = email,
                PasswordHash = password,
                Phone        = phone,
                Address      = address
            };

            bool result = UserDL.RegisterUser(user);
            return result ? (true, "Registration successful!") : (false, "Registration failed. Try again.");
        }

        public static (User user, string message) Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (null, "Email and password are required.");

            User user = UserDL.LoginUser(email, password);
            return user != null ? (user, "Login successful!") : (null, "Invalid email or password.");
        }
    }
}
