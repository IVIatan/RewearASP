using System;
using System.Collections.Generic;

namespace RewearApi.BL
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int BuyerUserId { get; set; }
        public int SellerUserId { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public string? ItemTitle { get; set; }
        public string? BuyerFullName { get; set; }
        public string? SellerFullName { get; set; }

        public string Validate()
        {
            var errors = new List<string>();

            if (ItemId <= 0) errors.Add("ItemId חובה");
            if (BuyerUserId <= 0) errors.Add("BuyerUserId חובה");
            if (SellerUserId <= 0) errors.Add("SellerUserId חובה");
            if (TotalAmount <= 0) errors.Add("TotalAmount חייב להיות חיובי");
            if (string.IsNullOrWhiteSpace(Status)) errors.Add("Status חובה");

            return string.Join("; ", errors);
        }
    }
}
