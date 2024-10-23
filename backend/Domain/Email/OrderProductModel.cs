namespace backend.Domain
{
    public class OrderProductModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Company { get; set; }
        public double PriceInColones { get; set; }
        public int Amount { get; set; }
        public string ImageURL { get; set; }
    }
}
