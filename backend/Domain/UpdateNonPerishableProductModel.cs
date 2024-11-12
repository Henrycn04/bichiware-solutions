namespace backend.Domain
{
    public class UpdateNonPerishableProductModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Weight { get; set; }
    }
}
