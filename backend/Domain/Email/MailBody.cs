using HtmlAgilityPack;

namespace backend.Domain
{
    public class MailBody
    {
        public HtmlDocument Html { get; set; }

        public string toString()
        {
            // inject the css into the html
            return Html.ToString();
        }
    }
}
