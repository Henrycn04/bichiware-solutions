using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using OpenQA.Selenium;

namespace UnitTesting
{
    internal class MailHandlerTest
    {
        private IMailService service;
        private MailHandler handler;

        [SetUp]
        public void Setup()
        {
            // var builder = WebApplication.CreateBuilder();
            // builder.Services.Configure<MailConfiguration>(builder.Configuration.GetSection("MailSettings"));


            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../..", "backend"));
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            MailConfiguration config = configuration.GetSection("MailSettings").Get<MailConfiguration>();

            this.service = new MailService(Options.Create(config));
            this.handler = new MailHandler(this.service);
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
