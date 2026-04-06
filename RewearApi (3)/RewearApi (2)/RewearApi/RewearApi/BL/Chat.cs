using System;
using System.Collections.Generic;

namespace RewearApi.BL
{
    public class Chat
    {
        public int ChatId { get; set; }

        public int AssociationId { get; set; }
        public int StoreId { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (AssociationId <= 0)
                errors.Add("AssociationId חובה");

            if (StoreId <= 0)
                errors.Add("StoreId חובה");

            return errors;
        }
    }
}