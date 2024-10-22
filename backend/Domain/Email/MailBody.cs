using HtmlAgilityPack;

namespace backend.Domain
{
    public class MailBody
    {
        public HtmlDocument Html { get; set; }

        public string ToString()
        {
            // inject the css into the html
            return Html.DocumentNode.InnerHtml;
        }
    }
}
