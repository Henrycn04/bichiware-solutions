namespace backend.Models
{
    public class ChangePasswordModel
    {
        public string userId { get; set; }

        public string newPassword { get; set; }

        public string securityCode { get; set; }
    }
}
