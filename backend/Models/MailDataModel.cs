namespace backend.Models
{
    public class MailDataModel
    {
        public string DestinationEmailAddress { get; set; }
        public string DestinationEmailName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
