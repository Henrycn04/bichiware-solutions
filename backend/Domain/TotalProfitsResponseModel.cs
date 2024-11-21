namespace backend.Domain
{
    public class TotalProfitsResponseModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalShippingCost { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }
}
