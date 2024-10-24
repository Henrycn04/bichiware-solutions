namespace backend.Domain
{
    public class GeneralProductModel
    {   public int ProductID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? Stock { get; set; }
        public string? DeliveryDays { get; set; }
        public int? Limit { get; set; }
        public decimal Weight { get; set; }
    }
}
