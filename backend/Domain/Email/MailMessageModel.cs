using backend.Domain;
using MimeKit;

namespace backend.Domain
{
    public class MailMessageModel
    {
        public string ReceiverMailAddress { get; set; }
        public string ReceiverMailName { get; set; }
        public string EmailSubject { get; set; }
        public MailBody EmailBody { get; set; }
    }
}
