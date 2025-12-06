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
        public string Status { get; set; }         

     
        public string ItemTitle { get; set; }          
        public string BuyerFullName { get; set; }      
        public string SellerFullName { get; set; }    


        public Order()
        {

        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (ItemId <= 0)
            {
                errors.Add("ItemId must be a positive number");
            }

            if (BuyerUserId <= 0)
            {
                errors.Add("BuyerUserId must be a positive number");
            }

            if (SellerUserId <= 0)
            {
                errors.Add("SellerUserId must be a positive number");
            }

            if (BuyerUserId > 0 && SellerUserId > 0 && BuyerUserId == SellerUserId)
            {
                errors.Add("BuyerUserId and SellerUserId cannot be the same");
            }

            if (TotalAmount <= 0)
            {
                errors.Add("TotalAmount must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                errors.Add("Status is required");
            }
            else
            {
                string[] validStatuses = { "Pending", "Paid", "Cancelled", "Completed" };

                if (Array.IndexOf(validStatuses, Status) == -1)
                {
                    errors.Add("Status is not valid");
                }
            }

            if (OrderDate != DateTime.MinValue && OrderDate > DateTime.Now.AddDays(1))
            {
                errors.Add("OrderDate cannot be in the far future");
            }

            return errors;
        }
    }
}
