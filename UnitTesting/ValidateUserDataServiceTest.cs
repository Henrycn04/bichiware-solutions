using backend;
using backend.Models;
using backend.Domain;
using backend.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTesting
{
    public class ValidateUserdataServiceTest
    {
        ValidateUserDataService validator;
        [SetUp]
        public void Setup()
        {
            this.validator = new ValidateUserDataService();
        }

        [Test]
        public void ValidateUser_Works_With_Empty()
        {
            registerUserModel data = new registerUserModel();
            bool result = this.validator.ValidateUser(data);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateUser_Works_With_Normal_Data()
        {
            registerUserModel data = new registerUserModel();
            data.name = "Andre";
            data.lastName = "Salas";
            data.phoneNumber = 12341234;
            data.cedula = 112123434;
            data.email = "andre@gmail.com";
            data.province = "San José";
            data.canton = "Escazu";
            data.district = "Escazu";
            data.exactAddress = "500 metros norte del parque";
            bool result = this.validator.ValidateUser(data);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ValidateAddress_Works_With_Empty()
        {
            PhysicalAddress address = new PhysicalAddress();
            bool result = this.validator.ValidateAddress(address);
            Assert.AreEqual(false, result);

        }
        
        [Test]
        public void ValidateAddress_Works_With_Invalid_Province()
        {
            PhysicalAddress address = new PhysicalAddress();
            address.province = "Mexico DF";
            address.canton = "Escazu";
            address.district = "Escazu";
            address.exactAddress = "500 metros norte del parque";
            bool result = this.validator.ValidateAddress(address);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateUserUpdate_Works_With_Number_In_Name()
        {
            UserDataModel data = new UserDataModel();
            data.name = "name21";
            data.emailAddress = "andre@gmail.cr.com";
            data.phoneNumber = 12341234;
            bool result = this.validator.ValidateUserUpdate(data);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidateUserUpdate_Works_With_Empty()
        {
            UserDataModel data = new UserDataModel();
            bool result = this.validator.ValidateUserUpdate(data);
            Assert.AreEqual(false, result);
        }

    }
}