using System;
using System.Collections.Generic;

namespace RewearApi.BL
{
    public class ChatMessage
    {
        public int MessageId { get; set; }

        public int ChatId { get; set; }

        public string SenderType { get; set; } = "";
        public int SenderId { get; set; }

        public string MessageText { get; set; } = "";

        public DateTime SentAt { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (ChatId <= 0)
                errors.Add("ChatId חובה");

            if (string.IsNullOrWhiteSpace(SenderType))
                errors.Add("SenderType חובה");

            if (SenderId <= 0)
                errors.Add("SenderId חובה");

            if (string.IsNullOrWhiteSpace(MessageText))
                errors.Add("MessageText חובה");

            return errors;
        }
    }
}