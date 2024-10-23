using HtmlAgilityPack;

namespace backend.Domain
{
    public class MailBody
    {
        public HtmlDocument Html { get; set; }

        public string ToString()
        {
            return Html.DocumentNode.InnerHtml;
        }
    }
}
