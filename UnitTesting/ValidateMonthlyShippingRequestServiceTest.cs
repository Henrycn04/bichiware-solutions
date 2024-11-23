using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Services;
using backend.Models;
using backend.Domain;
using System.Collections;

namespace UnitTesting
{
    public class ValidateMonthlyShippingCostRequestServiceTest
    {
        ValidateMonthlyShippingCostRequestService validator;
        [SetUp]
        public void Setup()
        {
            this.validator = new ValidateMonthlyShippingCostRequestService();
        }

        [Test]
        public void ValidateModel_Works_With_Empty()
        {
            MonthlyShippingRequestModel data = new MonthlyShippingRequestModel();
            data.startDate = "";
            data.endDate = "";
            bool result = this.validator.ValidateData(data);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ValidateModel_Fails_With_Invalid_Date()
        {
            MonthlyShippingRequestModel data = new MonthlyShippingRequestModel();
            data.startDate = "2024-01-01";
            data.endDate = "Error";
            bool result = this.validator.ValidateData(data);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateModel_Fails_With_Invalid_Interval()
        {
            MonthlyShippingRequestModel data = new MonthlyShippingRequestModel();
            data.startDate = "2024-02-02";
            data.endDate = "2024-01-01";
            bool result = this.validator.ValidateData(data);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateModel_Works_With_Valid_Data()
        {
            MonthlyShippingRequestModel data = new MonthlyShippingRequestModel();
            data.startDate = "2024-01-02";
            data.endDate = "2024-03-01";
            bool result = this.validator.ValidateData(data);
            Assert.AreEqual(true, result);
        }
    }
}
