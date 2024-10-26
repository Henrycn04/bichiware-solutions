namespace backend.Models
{
    public class CartProductModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public bool IsPerishable { get; set; }
        public int CurrentStock { get;  set; }
        public int CurrentCartQuantity { get;  set; }
    }

}