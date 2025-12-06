namespace RewearApi.BL
{
    public class ClothingItem
    {
        public int ItemId { get; set; }
        public int OwnerUserId { get; set; }
        public int? StoreId { get; set; }
        public int? DestinationStoreId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public decimal? Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? OwnerFullName { get; set; }
        public string? StoreName { get; set; }
        public string? DestinationStoreName { get; set; }

        public string? ImagePath { get; set; }   // חדש – נתיב התמונה
    }
}
