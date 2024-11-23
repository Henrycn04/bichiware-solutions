namespace backend.Models
{
    public class EntrepreneurOrdersModel
    {
        public int OrderID { get; set; }
        public int? UserID { get; set; }
        public int? AddressID { get; set; }
        public int? FeeID { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal ProductCost { get; set; }
        public int? OrderStatus { get; set; }
        public DateTime DeliveryDate { get; set; }

        public List<UserOrderProductModel> Products { get; set; } = new List<UserOrderProductModel>();

        public string CompanyName {get; set;}
    }
}
