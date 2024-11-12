using Microsoft.AspNetCore.Mvc;

namespace backend.Models
{
    public class PerishableProductModel : Controller
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CompanyID { get; set; }
        public string ImageURL { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string ProductDescription { get; set; }
        public string DeliveryDays { get; set; }
        public int ProductionLimit { get; set; }
        public string CompanyName { get; set; }
    }
}
