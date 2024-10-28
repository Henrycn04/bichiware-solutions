using backend.Handlers;
using backend.Models;
using System;
using System.Text.RegularExpressions;

namespace backend.Commands
{
    public class UpdateCompanyCommand
    {
        private readonly UpdateCompanyHandler _updateCompanyHandler;

        public UpdateCompanyCommand()
        {
            this._updateCompanyHandler = new UpdateCompanyHandler();
        }

        public void ModifyCompanyData(CompanyProfileModel newData)
        {
            CheckEmail(newData.Email);
            CheckCompanyName(newData.Name);
            CheckPhoneNumber(newData.PhoneNumber);
            CheckLegalID(newData.LegalId);
            this._updateCompanyHandler.modifyCompanyData(newData);
        }

        private void CheckEmail(string email)
        {
            Console.WriteLine(email);
            string regex = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            if(!Regex.IsMatch(email, regex))
            {
                throw new FormatException("Formato de correo electrónico incorrecto");
            }
        }

        private void CheckCompanyName(string companyName)
        {
            string regex = "[a-zA-Z0-9.-_]+";
            if (!Regex.IsMatch(companyName, regex))
            {
                throw new FormatException("Formato de nombre de empresa incorrecto");
            }
        }

        private void CheckPhoneNumber(string phoneNumber)
        {
            string regex = "\\d{8}";
            if (!Regex.IsMatch(phoneNumber, regex))
            {
                throw new FormatException("Formato de número de teléfono incorrecto");
            }
        }

        private void CheckLegalID(string legalID)
        {
            string regex = "\\d{9}";
            if (!Regex.IsMatch(legalID, regex))
            {
                throw new FormatException("Formato de cédula incorrecto");
            }
        }
    }
}
