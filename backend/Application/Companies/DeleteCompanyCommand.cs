using backend.Infrastructure;
using backend.Models;

namespace backend.Application
{
    public class DeleteCompanyCommand
    {
        private readonly IUpdateCompanyHandler companyHandler;

        public DeleteCompanyCommand(IUpdateCompanyHandler companyHandler)
        {
            this.companyHandler = companyHandler;
        }
        
        public bool DeleteCompany(CompaniesIDModel company)
        {
            if (this.ValidateCompany(company))
            {
                this.companyHandler.DeleteCompanyProducts(company.CompanyID);
                return this.companyHandler.DeleteCompany(company.CompanyID);
            }
            return false;
        }


        private bool ValidateCompany(CompaniesIDModel company)
        {
            return ValidateNullCompany(company)
                && ValidateCompanyId(company.CompanyID)
                && ValidateCompanyExistence(company)
                && ValidateCompanyCanBeDeleted(company);
        }


        private bool ValidateNullCompany(CompaniesIDModel company)
        {
            if (company == null)
            {
                throw new NullReferenceException("La compañia a borrar no puede ser nula.");
            }
            return true;
        }

        private bool ValidateCompanyExistence(CompaniesIDModel company)
        {
            if (!this.companyHandler.CheckCompanyExistence(company))
            {
                throw new ArgumentException("El nombre de la compañia no fue encontrado.");
            }
            return true;
        }

        private bool ValidateCompanyCanBeDeleted(CompaniesIDModel company)
        {
            if (this.companyHandler.IsHeadquarters(company))
            {
                throw new InvalidOperationException($"La compañia {company.CompanyName} no se puede eliminar porque es la casa matriz.");
            }

            if (this.companyHandler.HasPendingOrders(company))
            {
                throw new InvalidOperationException($"La compañia {company.CompanyName} no puede tener pedidos pendientes.");
            }
            return true;
        }

        private bool ValidateCompanyId(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentOutOfRangeException("La identificacion de la empresa debe ser un numero valido.");
            }
            return true;
        }
    }
}
