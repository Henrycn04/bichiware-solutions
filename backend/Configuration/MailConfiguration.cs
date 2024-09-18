namespace backend.Configuration
{
    public class MailConfiguration
    {
        public string OriginEmailAddress { get; set; }
        public string OriginEmailName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
