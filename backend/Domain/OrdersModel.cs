namespace backend.Models
{
    public class OrdersModel
    {
        public int OrderID { get; set; }
        public string ClientName { get; set; }
        public string OrderAddress { get; set; }
        public class OrderProductsModel
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
        }
        public List <OrderProductsModel> OrderProducts { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
