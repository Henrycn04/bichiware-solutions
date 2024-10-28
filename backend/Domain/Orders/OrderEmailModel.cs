namespace backend.Domain
{
    public class OrderEmailModel
    {
        public int orderId {  get; set; }
        public int addressId { get; set; }
        public int userId { get; set; }
        public double tax { get; set; }
        public string customerName { get; set; }
    }
}
