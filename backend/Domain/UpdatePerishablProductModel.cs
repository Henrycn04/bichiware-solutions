namespace backend.Domain
{
    public class UpdatePerishablProductModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string DeliveryDays { get; set; }
        public int Limit { get; set; }
        public decimal Weight { get; set; }
    }
}
