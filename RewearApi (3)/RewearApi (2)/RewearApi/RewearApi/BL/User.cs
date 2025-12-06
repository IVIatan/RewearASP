using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RewearApi.BL
{
    public class User
    {

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }


        public User()
        {
        }


        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(FullName))
            {
                errors.Add("FullName is required");
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                errors.Add("Email is required");
            }
            else
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(Email))
                {
                    errors.Add("Email format is not valid");
                }
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add("Password is required");
            }
            else if (Password.Length < 6)
            {
                errors.Add("Password must be at least 6 characters long");
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                errors.Add("City is required");
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                var phoneRegex = new Regex(@"^05\d-?\d{7}$");
                if (!phoneRegex.IsMatch(Phone))
                {
                    errors.Add("Phone number is not a valid Israeli mobile format");
                }
            }

            if (string.IsNullOrWhiteSpace(UserType))
            {
                errors.Add("UserType is required");
            }
            else
            {
                if (UserType != "Regular" && UserType != "Admin")
                {
                    errors.Add("UserType must be 'Regular' or 'Admin'");
                }
            }

            return errors;
        }
    }

}


