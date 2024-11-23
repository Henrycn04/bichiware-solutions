using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend;
using backend.Domain;
using backend.Models;
using backend.Services;

namespace UnitTesting
{
    public class ValidateClientReportRequestServiceTest
    {
        ValidateClientReportRequestService validator;
        [SetUp]
        public void Setup()
        {
           validator = new ValidateClientReportRequestService();
        }

        [Test]
        public void ValidateUser_True_When_Invalid_Id ()
        {
            ClientReportRequestModel model = new ClientReportRequestModel();
            model.RequestType = 2;
            model.UserID = -1;
            bool result = this.validator.ValidateData (model);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ValidateUser_False_When_Invalid_RequestType()
        {
            ClientReportRequestModel model = new ClientReportRequestModel();
            model.RequestType = 1;
            bool result = this.validator.ValidateData(model);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateUser_False_When_Invalid_CreationDate()
        {
            ClientReportRequestModel model = new ClientReportRequestModel();
            model.RequestType = 2;
            model.CreationStartDate = "Error";
            bool result = this.validator.ValidateData(model);
            Assert.AreEqual(false, result);
        }

    }
}
