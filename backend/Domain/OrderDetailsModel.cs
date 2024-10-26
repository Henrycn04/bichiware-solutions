using backend.Domain;

namespace backend.Models
{
    public class OrderDetailsModel
    {
        public string CustomerName {set; get;}
        public string CustomerEmail { get; set; }
        public List<OrderProductModel> OrderProducts { get; set; }
        public double Taxes { get; set; }
        public double ShippingCost { get; set; }
        public double ProductCost { get; set; }
    }
}
