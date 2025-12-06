using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RewearApi.BL
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        // סיסמה גולמית (אם תרצה לעבוד איתה בצד לקוח)
        public string? Password { get; set; }

        // זה השדה שמתאים לעמודה PasswordHash ב־DB
        public string PasswordHash { get; set; } = "";

        public string? Phone { get; set; }
        public string City { get; set; } = "";
        public string UserType { get; set; } = "Regular"; // Regular / Admin

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public string Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(FullName))
                errors.Add("FullName חובה");

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add("Email חובה");
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email לא תקין");

            if (string.IsNullOrWhiteSpace(City))
                errors.Add("City חובה");

            if (string.IsNullOrWhiteSpace(UserType))
                errors.Add("UserType חובה");

            return string.Join("; ", errors);
        }
    }
}
