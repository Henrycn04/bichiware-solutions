namespace backend.Models
{
    public class AccountSecurityDataModel
    {
        public string password { get; set; }

        public string userId {  get; set; }

        public string securityCode { get; set; }

        public DateTime dateTimeLastCode { get; set; }
    }
}
