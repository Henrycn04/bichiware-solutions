namespace backend.Models
{
    public class ShoppingCartModel
    {
        public int UserID { get; set; }
        public decimal ProductCost { get; set; }
        public decimal ShippingCost { get; set; }
        public List<CartProductModel> Products { get; set; } = new List<CartProductModel>();
    }
}
