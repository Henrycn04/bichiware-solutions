using backend.Domain;
using HtmlAgilityPack;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace backend.Application
{
    public class OrdersBodyBuilder : MailBodyMessageInjector
    {
        protected MailBody mailBody { get; set; }
        protected List<OrderProductModel> products = null;
        protected double taxInColones;
        protected double shippingFee;
        protected string typeOfMessage;

        protected string pathToConfiguration = @"Configuration/Email/Orders";
        protected readonly Dictionary<string, string> emailOrdersFiles = new Dictionary<string, string>()
        {
            { "Messages",    "OrderMessages.json"     },
            { "Html",        "Orders.html"             }
        };

        public void SetOrderDetails(List<OrderProductModel> products, double taxInColones, double shippingFee)
        {
            if (this.products == null)
            {
                this.products = new List<OrderProductModel>();
            }
            this.products = products;
            this.taxInColones = taxInColones;
            this.shippingFee = shippingFee;
        }

        protected HtmlDocument LoadHtml()
        {
            string path = Path.Combine(Environment.CurrentDirectory, pathToConfiguration, emailOrdersFiles["Html"]);
            HtmlDocument document = new HtmlDocument();
            document.Load(path);
            return document;
        }

        protected MailBodyMessagesModel InjectBodyMessage(HtmlDocument document, MailMessageModel mail)
        {
            MailBodyMessagesModel message = GetBodyMessage(pathToConfiguration, emailOrdersFiles["Messages"], PlatformRoleFilter);
            this.InjectBodyMessage(message, document);
            mail.EmailSubject = message.Subject;
            return message;
        }


        protected IEnumerable<MailBodyMessagesModel> PlatformRoleFilter(List<MailBodyMessagesModel> messages)
        {
            return messages.Where(p => p.Name.Equals(this.typeOfMessage));
        }


        protected HtmlDocument InjectOrderReceipt(HtmlDocument document)
        {
            if (this.products == null || this.products.Count <= 0)
            {
                throw new Exception("The order of products were not provided. Aborting sending an email");
            }

            double subtotalInColones = InjectProducts(document, products);
            return this.InjectReceiptDetails(subtotalInColones, document);
        }


        protected double InjectOneProduct(HtmlNode node, OrderProductModel product)
        {
            node.SelectSingleNode($".//*[@id='Amount']").InnerHtml = Convert.ToString(product.Amount);
            node.SelectSingleNode($".//*[@id='ProductImage']").SetAttributeValue("src", product.ImageURL);
            node.SelectSingleNode($".//*[@id='ProductName']").InnerHtml = product.Name;
            node.SelectSingleNode($".//*[@id='ProductCategory']").InnerHtml = product.Category;
            node.SelectSingleNode($".//*[@id='ProductVendor']").InnerHtml = product.Company;
            node.SelectSingleNode($".//*[@id='ProductPrice']").InnerHtml = Convert.ToString(product.PriceInColones);
            return product.Amount * product.PriceInColones;
        }


        protected double InjectProducts(HtmlDocument document, List<OrderProductModel> products)
        {
            double subtotalInColones = 0;
            HtmlNode firstProductNode = document.GetElementbyId("Product");
            HtmlNode productList = document.GetElementbyId("ProductList");
            int numberOfProduct = 0;

            foreach (OrderProductModel product in products)
            {
                HtmlNode nodeClone = firstProductNode.Clone();
                nodeClone.SetAttributeValue("id", $"Product{numberOfProduct++}" );
                subtotalInColones += InjectOneProduct(nodeClone, product);
                productList.AppendChild(nodeClone);
            }

            // This is dumb I know, but works.
            firstProductNode.RemoveAll();

            return subtotalInColones;
        }


        protected HtmlDocument InjectReceiptDetails(double subtotalInColones, HtmlDocument document)
        {
            if (!this.ValidateReceiptDetails() && subtotalInColones < 0)
            {
                throw new Exception("Receipt details are not consistent. Aborting sending an email");
            }

            HtmlNode subtotalNode = document.GetElementbyId("Subtotal");
            subtotalNode.InnerHtml = Convert.ToString(subtotalInColones);

            HtmlNode taxNode = document.GetElementbyId("Tax");
            taxNode.InnerHtml = Convert.ToString(taxInColones);

            HtmlNode shippingNode = document.GetElementbyId("Shipping");
            shippingNode.InnerHtml = Convert.ToString(shippingFee);

            double total = subtotalInColones + taxInColones + shippingFee;
            HtmlNode totalNode = document.GetElementbyId("Total");
            totalNode.InnerHtml = Convert.ToString(total);

            return document;
        }


        protected bool ValidateReceiptDetails()
        {
            return products != null && products.Count > 0;
        }
    }
}
