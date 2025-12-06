using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RewearApi.BL
{
    public class ClothingItem
    {
        public int ItemId { get; set; }           
        public int OwnerUserId { get; set; }      
        public int? StoreId { get; set; }         
        public int? DestinationStoreId { get; set; } 

        public string Title { get; set; }       
        public string? Description { get; set; }    
        public string Category { get; set; }       
        public string Size { get; set; }            
        public string Condition { get; set; }      
        public string Color { get; set; }         
        public decimal? Price { get; set; }        
        public string Status { get; set; }         
        public DateTime? ExpirationDate { get; set; } 
        public DateTime CreatedAt { get; set; }    

        public string? OwnerFullName { get; set; }
        public string? StoreName { get; set; }
        public string? DestinationStoreName { get; set; }

        public ClothingItem()
        {
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Title))
            {
                errors.Add("Title is required");
            }

            if (string.IsNullOrWhiteSpace(Category))
            {
                errors.Add("Category is required");
            }

            if (string.IsNullOrWhiteSpace(Size))
            {
                errors.Add("Size is required");
            }

            if (string.IsNullOrWhiteSpace(Condition))
            {
                errors.Add("Condition is required");
            }

            if (string.IsNullOrWhiteSpace(Color))
            {
                errors.Add("Color is required");
            }

            if (Price.HasValue && Price.Value < 0)
            {
                errors.Add("Price cannot be negative");
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status is required");
            }
            else
            {
                string[] validStatuses =
                {
                    "Available", "Reserved", "Sold",
                    "Donated", "Recycled", "Expired"
                };

                if (Array.IndexOf(validStatuses, Status) == -1)
                {
                    errors.Add("Status is not valid");
                }
            }

            if (ExpirationDate.HasValue &&
                CreatedAt != DateTime.MinValue &&
                ExpirationDate.Value < CreatedAt)
            {
                errors.Add("ExpirationDate cannot be earlier than CreatedAt");
            }

            return errors;
        }
    }
}
