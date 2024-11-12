namespace backend.Domain
{
    public class AddOrderModel
    {
        public int UserID { get; set; }
        public int AddressID { get; set; }
        public int FeeID { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal ProductCost { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
