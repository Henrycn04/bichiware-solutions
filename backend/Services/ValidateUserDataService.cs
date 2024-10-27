using System;
using System.Text.RegularExpressions;
using backend.Domain;
using backend.Models;

namespace backend.Services
{
    public class ValidateUserDataService
    {
        private readonly Regex spanishCharacters;
        private readonly Regex spanishCharactersNumbers;
        private readonly Regex emailCheck;
        private readonly string[] provinces;

        public ValidateUserDataService()
        {
            // Corrected regex patterns without spaces between character ranges
            this.spanishCharacters = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]{1,60}$");
            this.spanishCharactersNumbers = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚ0-9\s]{1,300}$");
            this.emailCheck = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            this.provinces = new string[] {"San Jose", "Alajuela", "Cartago",
                                           "Heredia", "Guanacaste", "Puntarenas", "Limon"};
        }

        public bool ValidateUser(registerUserModel data)
        {
            return this.CheckName(data.name + " " + data.lastName) &&
                    this.CheckEmail(data.email) &&
                    this.CheckCedula(data.cedula) &&
                    this.CheckAddress(data.province, data.canton, data.district, data.exactAddress) &&
                    this.CheckPhone(data.phoneNumber);
        }

        private bool CheckName(string fullName)
        {
            bool result = !string.IsNullOrEmpty(fullName) &&
                          this.spanishCharacters.IsMatch(fullName) &&
                          fullName.Length <= 60;
            if (!result) Console.WriteLine("Invalid Name");
            return result;
        }

        private bool CheckEmail(string email)
        {
            bool result = !string.IsNullOrEmpty(email) &&
                          this.emailCheck.IsMatch(email) &&
                          email.Length <= 50;
            if (!result) Console.WriteLine("Invalid email");
            return result;
        }

        private bool CheckCedula(int cedula)
        {
            bool result = cedula >= 100000000 && cedula <= 999999999;
            if (!result) Console.WriteLine("Invalid cedula");
            return result;
        }

        private bool CheckAddress(string province, string canton, string district, string exactAddress)
        {
            bool resultProvince = !string.IsNullOrEmpty(province) &&
                                  Array.IndexOf(this.provinces, province) != -1;
            if (!resultProvince) Console.WriteLine("Invalid province");

            bool resultCanton = !string.IsNullOrEmpty(canton) &&
                                this.spanishCharacters.IsMatch(canton) &&
                                canton.Length <= 50;
            if (!resultCanton) Console.WriteLine("Invalid canton");

            bool resultDistrict = !string.IsNullOrEmpty(district) &&
                                  this.spanishCharacters.IsMatch(district) &&
                                  district.Length <= 50;
            if (!resultDistrict) Console.WriteLine("Invalid district");

            bool resultExactAddress = !string.IsNullOrEmpty(exactAddress) &&
                                      this.spanishCharactersNumbers.IsMatch(exactAddress) &&
                                      exactAddress.Length <= 300;
            if (!resultExactAddress) Console.WriteLine("Invalid exact address");

            return resultProvince && resultCanton && resultDistrict && resultExactAddress;
        }

        private bool CheckPhone(int phoneNumber)
        {
            return phoneNumber >= 10000000 && phoneNumber <= 99999999;
        }

        public bool ValidateAddress(PhysicalAddress data)
        {
            return this.CheckAddress(data.province, data.canton, data.district, data.exactAddress);
        }

        public bool ValidateUserUpdate(UserDataModel data)
        {
            return this.CheckName(data.name) &&
                    this.CheckEmail(data.emailAddress) &&
                    this.CheckPhone(data.phoneNumber);
        }
    }
}
