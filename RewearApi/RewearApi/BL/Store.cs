using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RewearApi.BL
{
    public class Store
    {

        public int StoreId  { get; set; }          
        public string StoreName { get; set; }    
        public string StoreType { get; set; }     
        public string City { get; set; }          
        public string? Address { get; set; }    
        public string? Phone { get; set; }      
        public string? Email { get; set; }        
        public bool IsActive { get; set; }       
        public DateTime CreatedAt { get; set; }   


        public Store()
        {

        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(StoreName))
            {
                errors.Add("StoreName is required");
            }

            if (string.IsNullOrWhiteSpace(StoreType))
            {
                errors.Add("StoreType is required");
            }
            else
            {
                string[] validTypes = { "Donation", "Recycling", "Mixed" };

                if (Array.IndexOf(validTypes, StoreType) == -1)
                {
                    errors.Add("StoreType is not valid");
                }
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                errors.Add("City is required");
            }

            if (Address != null && string.IsNullOrWhiteSpace(Address))
            {
                errors.Add("Address cannot be empty if provided");
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                var phoneRegex = new Regex(@"^0\d-?\d{7,8}$"); 
                if (!phoneRegex.IsMatch(Phone))
                {
                    errors.Add("Phone number format is not valid");
                }
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(Email))
                {
                    errors.Add("Email format is not valid");
                }
            }

            return errors;
        }
    }
}
