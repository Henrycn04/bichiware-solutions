namespace backend.Domain
{
    public class MailBody
    {
        string Html { get; set; }
        string Css { get; set; }

        string toString()
        {
            // inject the css into the html
            return Html;
        }
    }
}
