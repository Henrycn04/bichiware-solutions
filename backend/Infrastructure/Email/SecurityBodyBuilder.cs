using backend.Application;
using backend.Domain;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace backend.Infrastructure
{
    public class SecurityBodyBuilder : MailBodyMessageInjector, IMailBodyBuilder
    {
        private MailBody body { get; set; }
        private string securityCode { get; set; }
        private string reason { get; set; }

        private string pathToConfiguration = @"Configuration/Email/Security/";
        private readonly Dictionary<string, string> emailSecurityFiles = new Dictionary<string, string>()
        {
            { "Messages",    "SecurityMessages.json"     },
            { "Html",        "Security.html"             }
        };


        public MailBody createBody()
        {
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory, pathToConfiguration, emailSecurityFiles["Html"]);
                HtmlDocument html = new HtmlDocument();
                html.Load(path);

                MailBodyMessagesModel message = GetBodyMessage(pathToConfiguration, emailSecurityFiles["Messages"], SecurityReasonFilter);
                this.InjectBodyMessage(message, html);
                this.InjectSecurityCode(html);

                body.Html = html;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return body;
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
    }
}
