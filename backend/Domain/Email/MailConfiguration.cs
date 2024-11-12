namespace backend.Domain
{
    public class MailConfiguration
    {
        public string SenderMailAddress { get; set; }
        public string SenderMailName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
