using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using HtmlAgilityPack;
using Moq;

namespace UnitTesting
{
    internal class MailHandlerTest
    {
        private IMailService service;
        private MailHandler handler;

        [SetUp]
        public void Setup()
        {
            this.service = new Mock<IMailService>().Object;
            this.handler = new MailHandler(service);
        }

        [Test]
        public void SendMailWithoutBodyBuilder()
        {
            // Arrange
            MailMessageModel msg = new MailMessageModel()
            {
                ReceiverMailAddress =   "dmorarod16@gmail.com",
                ReceiverMailName =      "Daniel"
            };
            // Act
            bool response = this.handler.SendMail(msg);
            // Assert
            Assert.IsFalse(response);
        }

        [Test]
        public void SendMailWithNullMsg()
        {
            // Arrange
            MailMessageModel            msg                = null;
            SecurityBodyBuilder         builder            = new SecurityBodyBuilder();
            builder.SetReason("Verification");
            builder.SetSecurityCode("000000");
            // Act
            this.handler.SetBodyBuilder(builder);
            bool response = this.handler.SendMail(msg);
            // Assert
            Assert.IsFalse(response);
        }

        [Test]
        public void SendMailWithInvalidDomain()
        {
            // Arrange
            MailMessageModel msg = new MailMessageModel()
            {
                ReceiverMailAddress = "r@r.r",
                ReceiverMailName = "R"
            };
            SecurityBodyBuilder builder = new SecurityBodyBuilder();
            builder.SetReason("Verification");
            builder.SetSecurityCode("909090");
            // Act
            this.handler.SetBodyBuilder(builder);
            bool response = this.handler.SendMail(msg);
            // Assert
            Assert.IsFalse(response);
        }

        [Test]
        public void SendMailWithManualBody()
        {
            // Arrange
            HtmlDocument html = new HtmlDocument();
            html.DocumentNode.InnerHtml = "Hola como estas";
            MailMessageModel msg = new MailMessageModel()
            {
                ReceiverMailAddress = "dmorarod16@gmail.com",
                ReceiverMailName = "Daniel",
                EmailBody = new MailBody()
                {
                    Html = html
                },
                EmailSubject = "Mensaje"
            };
            SecurityBodyBuilder builder = new SecurityBodyBuilder();
            builder.SetReason("Verification");
            builder.SetSecurityCode("909090");
            // Act
            this.handler.SetBodyBuilder(builder);
            bool response = this.handler.SendMail(msg);
            // Assert
            Assert.IsTrue(response);
        }

        [Test]
        public void SendMailWithoutSettingDependentData()
        {
            // Arrange
            MailMessageModel msg = new MailMessageModel()
            {
                ReceiverMailAddress = "dmorarod16@gmail.com",
                ReceiverMailName = "Daniel"
            };
            SecurityBodyBuilder builder = new SecurityBodyBuilder();
            builder.SetReason("Verification");
            // Act
            this.handler.SetBodyBuilder(builder);
            bool response = this.handler.SendMail(msg);
            // Assert
            Assert.IsFalse(response);
        }
    }
}
