using backend.Application;
using backend.Domain;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace backend.Infrastructure
{
    public class SecurityBodyBuilder : MailBodyMessageInjector, IMailBodyBuilder
    {
        private string securityCode;
        private string reason;

        private string pathToConfiguration = @"Configuration/Email/Security/";
        private readonly Dictionary<string, string> emailSecurityFiles = new Dictionary<string, string>()
        {
            { "Messages",    "SecurityMessages.json"     },
            { "Html",        "Security.html"             }
        };


        public MailMessageModel createBody(MailMessageModel mail)
        {
            try
            {
                mail.EmailBody = new MailBody();
                string path = Path.Combine(Environment.CurrentDirectory, pathToConfiguration, emailSecurityFiles["Html"]);
                HtmlDocument html = new HtmlDocument();
                html.Load(path);

                MailBodyMessagesModel message = GetBodyMessage(pathToConfiguration, emailSecurityFiles["Messages"], SecurityReasonFilter);
                mail.EmailSubject = message.Subject;
                this.InjectBodyMessage(message, html);
                this.InjectSecurityCode(html);

                mail.EmailBody.Html = html;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return mail;
        }


        private IEnumerable<MailBodyMessagesModel> SecurityReasonFilter(List<MailBodyMessagesModel> messages)
        {
            return messages.Where(p => p.Name.Equals(reason));
        }


        private void InjectSecurityCode(HtmlDocument html)
        {
            HtmlNode securityCodeNode = html.GetElementbyId("SecurityCode");
            securityCodeNode.InnerHtml = securityCode;
        }


        public void SetSecurityCode(string securityCode)
        {
            this.securityCode = securityCode;
        }

        public void SetReason(string reason)
        {
            this.reason = reason;
        }
    }
}
