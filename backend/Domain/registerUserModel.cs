namespace backend.Models
{
    public class registerUserModel
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int cedula { get; set; }
        public string password { get; set; }
        public int phoneNumber {  get; set; }
        public string province { get; set; }
        public string canton { get; set; }
        public string district { get; set; }
        public string exactAddress { get; set; }
        
    }
}
