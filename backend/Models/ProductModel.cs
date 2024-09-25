namespace backend.Models
{
    public class NonPerishableModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Stock { get; set; }

    }

    public class PerishableModel
    {
         public int CompanyID { get; set; }
         public string CompanyName { get; set; }
         public string Name { get; set; }
         public string Image { get; set; }
         public string Category { get; set; }
         public decimal Price { get; set; }
         public string Description { get; set; }
         public string DeliveryDays { get; set; }
         public int Limit { get; set; }

    }

    public class DeliveryModel
    {
        public int ProductID { get; set; }
        public int BatchNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ReservedUnits { get; set; }

    }
}
