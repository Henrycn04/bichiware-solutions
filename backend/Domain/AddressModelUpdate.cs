namespace backend.Models
{
    public class AddressModelUpdate
    {
        public int AddressID { get; set; }
        public string Province { get; set; }
        public string Canton {  get; set; }
        public string District { get; set; }
        public string Exact { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public bool IsCompany { get; set; } = false;
    }
}
