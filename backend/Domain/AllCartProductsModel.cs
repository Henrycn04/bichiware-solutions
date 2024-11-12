namespace backend.Domain
{
    public class AllCartProductsModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public bool IsPerishable { get; set; }
        public int CurrentStock { get; set; }
        public int CurrentCartQuantity { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public string CompanyName { get; set; }
        public string ImageURL { get; set; }
        public decimal Weight { get; set; }
    }
}
