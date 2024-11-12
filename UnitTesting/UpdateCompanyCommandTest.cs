using backend.Commands;

namespace UnitTesting
{
    class UpdateCompanyCommandTest
    {
        UpdateCompanyCommand _updateCompanyCommand;

        [SetUp]

        public void SetUp()
        {
            this._updateCompanyCommand = new UpdateCompanyCommand();
        }

        [Test]
        public void CheckNullEmailTest()
        {
            // Arrange
            string email = null;
            // Act and Assert
            var exception = Assert.Throws<NullReferenceException>(() => this._updateCompanyCommand.CheckEmail(email));
        }

        [Test]
        public void CheckEmptyEmailTest()
        {
            // Arrange
            string email = "";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
        }

        [Test]

        public void CheckEmailWithoutAtTest()
        {
            // Arrange
            string email = "agmail.com";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
            //Assert
        } 

        [Test]
        public void CheckEmailWithoutDotTest()
        {
            // Arrange
            string email = "a@gmailcom";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
            //Assert
        }

        [Test]
        public void CheckIncompleteEmailTest1()
        {
            // Arrange
            string email = "a@gmail.";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
            //Assert
        }

        [Test]
        public void CheckIncompleteEmailTest2()
        {
            // Arrange
            string email = "@gmail.com";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
            //Assert
        }

        [Test]
        public void CheckIncompleteEmailTest3()
        {
            // Arrange
            string email = "a@.com";
            // Act and Assert
            var exception = Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckEmail(email));
            //Assert
        }

        [Test]
        public void CheckCorrectEmailTest()
        {
            // Arrange
            string email = "a@gmail.com";
            // Act and Assert
            Assert.DoesNotThrow(() => this._updateCompanyCommand.CheckEmail(email));
        }

        [Test]
        public void CheckNullPhoneNumberTest()
        {
            // Arrange
            string phoneNumber = null;
            // Act and Assert
            Assert.Throws<NullReferenceException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckEmptyPhoneNumberTest()
        {
            // Arrange
            string phoneNumber = "";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckPhoneNumberWithSymbolsTest()
        {
            // Arrange
            string phoneNumber = "1234*567";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckPhoneNumberWithLettersTest()
        {
            // Arrange
            string phoneNumber = "1234d567";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckPhoneNumberWithLessDigitsTest()
        {
            // Arrange
            string phoneNumber = "1234567";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckPhoneNumberWithMoreDigitsTest()
        {
            // Arrange
            string phoneNumber = "123456789";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckCorrectPhoneNumberTest()
        {
            // Arrange
            string phoneNumber = "12345678";
            // Act and Assert
            Assert.DoesNotThrow(() => this._updateCompanyCommand.CheckPhoneNumber(phoneNumber));
        }

        [Test]
        public void CheckNullCompanyNameTest()
        {
            // Arrange
            string companyName = null;
            // Act and Assert
            Assert.Throws<NullReferenceException>(() => this._updateCompanyCommand.CheckCompanyName(companyName));
        }

        [Test]
        public void CheckEmptyCompanyNameTest()
        {
            // Arrange
            string companyName = "";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckCompanyName(companyName));
        }

        [Test]
        public void CheckCompanyNameWithInvalidSymbolsTest()
        {
            // Arrange
            string companyName = "Name/";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckCompanyName(companyName));
        }

        [Test]
        public void CheckCorrectCompanyNameTest()
        {
            // Arrange
            string companyName = ".-_name_-.";
            // Act and Assert
            Assert.DoesNotThrow(() => this._updateCompanyCommand.CheckCompanyName(companyName));
        }

        [Test]
        public void CheckNullLegalID()
        {
            // Arrange
            string legalID = null;
            // Act and Assert
            Assert.Throws<NullReferenceException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckEmptyLegalID()
        {
            // Arrange
            string legalID = "";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckLegalIDWithSymbolsTest()
        {
            // Arrange
            string legalID = "12345678/";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckLegalIDWithLettersTest()
        {
            // Arrange
            string legalID = "12345p789";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckLegalIDWithLessDigitsTest()
        {
            // Arrange
            string legalID = "12345678";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckLegalIDWithMoreDigitsTest()
        {
            // Arrange
            string legalID = "0123456789";
            // Act and Assert
            Assert.Throws<FormatException>(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }

        [Test]
        public void CheckCorrectLegalIDTest()
        {
            // Arrange
            string legalID = "123456789";
            // Act and Assert
            Assert.DoesNotThrow(() => this._updateCompanyCommand.CheckLegalID(legalID));
        }
    }
}
