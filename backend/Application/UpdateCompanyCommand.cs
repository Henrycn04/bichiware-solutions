using backend.Infrastructure;
using backend.Models;
using System.Text.RegularExpressions;

namespace backend.Commands
{
    public class UpdateCompanyCommand
    {
        private readonly IUpdateCompanyHandler _updateCompanyHandler;

        public UpdateCompanyCommand(IUpdateCompanyHandler companyHandler)
        {
            this._updateCompanyHandler = companyHandler;
        }

        public void ModifyCompanyData(CompanyProfileModel newData)
        {
            CheckEmail(newData.Email);
            CheckCompanyName(newData.Name);
            CheckPhoneNumber(newData.PhoneNumber);
            CheckLegalID(newData.LegalId);
            this._updateCompanyHandler.modifyCompanyData(newData);
        }

        public void CheckEmail(string email)
        {
            if (email == null)
            {
                throw new NullReferenceException("Correo electrónico nulo");
            }
            string regex = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, regex))
            {
                throw new FormatException("Formato de correo electrónico incorrecto");
            }
        }

        public void CheckCompanyName(string companyName)
        {
            if (companyName == null)
            {
                throw new NullReferenceException("Nombre de empresa nulo");
            }
            string regex = "^[a-zA-Z0-9._-]+$";
            if (!Regex.IsMatch(companyName, regex))
            {
                throw new FormatException("Formato de nombre de empresa incorrecto");
            }
        }

        public void CheckPhoneNumber(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                throw new NullReferenceException("Número de teléfono nulo");
            }
            string regex = "^[0-9]{8}$";
            if (!Regex.IsMatch(phoneNumber, regex))
            {
                throw new FormatException("Formato de número de teléfono incorrecto");
            }
        }

        public void CheckLegalID(string legalID)
        {
            if (legalID == null)
            {
                throw new NullReferenceException("Cédula nula");
            }
            string regex = "^[0-9]{9}$";
            if (!Regex.IsMatch(legalID, regex))
            {
                throw new FormatException("Formato de cédula incorrecto");
            }
        }
    }
}
