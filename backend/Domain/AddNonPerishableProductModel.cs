namespace backend.Models
{
    public class AddNonPerishableProductModel
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
    public class AddNonPerishableProductToOrderModel
    {
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
