using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend;
using backend.Models;
using backend.Domain;
using backend.Services;
using backend.Application;

namespace UnitTesting
{
    public class DeleteAddressQueryTest
    {
        DeleteAddressQuery query;
        [SetUp]
        public void Setup()
        {
            this.query = new DeleteAddressQuery();
        }

        [Test]
        public void DeleteAddressQuery_Works_With_NonValidID()
        {
            bool result = this.query.CheckDelete(-1);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void DeleteAddressCommand_Works_With_ValidID()
        {
            bool result = this.query.CheckDelete(1);
            Assert.AreEqual(false, result);
        }
    }
}
