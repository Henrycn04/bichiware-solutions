namespace backend.Models
{
    public class UserOrderProductModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public int? BatchNumber { get; set; }
        public string ProductType { get; set; } 
    }
}
