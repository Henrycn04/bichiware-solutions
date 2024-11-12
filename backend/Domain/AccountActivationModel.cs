namespace backend.Domain
{
    public class AccountActivationModel
    {
        public string userId { get; set; }
        public string confirmationCode { get; set; }
        public DateTime dateTimeLastCode { get; set; }
    }
}
