namespace backend.Models
{
    public class AddAddressModel
    {
        public string province { get; set; }
        public string canton { get; set; }
        public string district { get; set; }
        public string exactAddress { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int userID { get; set; }
    }
}
