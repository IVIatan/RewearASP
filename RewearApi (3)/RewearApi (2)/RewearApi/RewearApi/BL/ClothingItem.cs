using System;
using System.Collections.Generic;

namespace RewearApi.BL
{
    public class ClothingItem
    {
        public int ItemId { get; set; }
        public int OwnerUserId { get; set; }
        public int? StoreId { get; set; }

        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "";
        public string Size { get; set; } = "";
        public string Condition { get; set; } = "";
        public string Color { get; set; } = "";

        public decimal? Price { get; set; }
        public string Status { get; set; } = "Available";
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? DestinationStoreId { get; set; }

        public string? OwnerFullName { get; set; }
        public string? StoreName { get; set; }
        public string? DestinationStoreName { get; set; }

        public string? ImagePath { get; set; }

        public string Validate()
        {
            var errors = new List<string>();

            if (OwnerUserId <= 0) errors.Add("OwnerUserId חובה");
            if (string.IsNullOrWhiteSpace(Title)) errors.Add("Title חובה");
            if (string.IsNullOrWhiteSpace(Category)) errors.Add("Category חובה");
            if (string.IsNullOrWhiteSpace(Size)) errors.Add("Size חובה");
            if (string.IsNullOrWhiteSpace(Condition)) errors.Add("Condition חובה");
            if (string.IsNullOrWhiteSpace(Color)) errors.Add("Color חובה");
            if (string.IsNullOrWhiteSpace(Status)) errors.Add("Status חובה");
            if (Price < 0) errors.Add("Price לא יכול להיות שלילי");

            return string.Join("; ", errors);
        }
    }
}
