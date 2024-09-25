namespace backend.Models
{
    public class CompanyModel
    {
        public string CompanyName { get; set; }
        public int Cedula { get; set; }
        public string EmailAddress { get; set; }
        public int PhoneNumber { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string ExactAddress { get; set; }

    }
}