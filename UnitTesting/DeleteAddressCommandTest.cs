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
    public class DeleteAddressCommandTest
    {
        DeleteAddressCommand command;
        [SetUp]
        public void Setup()
        {
            this.command = new DeleteAddressCommand();
        }

        [Test]
        public void DeleteAddressCommand_Works_With_NonValidID()
        {
            try
            {
                this.command.DeleteAddress(-1, true);
                Assert.AreEqual(false, true);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Address deletion unsuccesful: No rows affected with address -1",
                    e.Message);
            }
        }

        [Test]
        public void DeleteAddressCommand_Works_With_NonValidID_LogicalDelete()
        {
            try
            {
                this.command.DeleteAddress(-1, false);
                Assert.AreEqual(false, true);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Logical address deletion unsuccesful: No rows affected with address -1",
                    e.Message);
            }
        }
    }
}
