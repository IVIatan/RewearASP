using System;
using System.Collections.Generic;

namespace RewearApi.BL
{
    public class Store
    {
        public int StoreId { get; set; }
        public int OwnerUserId { get; set; }

        public string StoreName { get; set; } = "";
        public string Purpose { get; set; } = "";   // Donation / Recycling
        public string City { get; set; } = "";

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public string? OwnerFullName { get; set; }

        public string Validate()
        {
            var errors = new List<string>();

            if (OwnerUserId <= 0) errors.Add("OwnerUserId חובה");
            if (string.IsNullOrWhiteSpace(StoreName)) errors.Add("StoreName חובה");
            if (string.IsNullOrWhiteSpace(Purpose)) errors.Add("Purpose חובה");
            if (string.IsNullOrWhiteSpace(City)) errors.Add("City חובה");

            return string.Join("; ", errors);
        }
    }
}
