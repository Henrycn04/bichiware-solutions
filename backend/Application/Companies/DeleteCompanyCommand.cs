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
        
        public bool DeleteCompany(int companyId)
        {
            if (this.ValidateCompany(companyId))
            {
                this.companyHandler.DeleteCompanyProducts(companyId);
                return this.companyHandler.DeleteCompany(companyId);
            }
            return false;
        }


        private bool ValidateCompany(int companyId)
        {
            return ValidateCompanyId(companyId)
                && ValidateCompanyExistence(companyId)
                && ValidateCompanyCanBeDeleted(companyId);
        }

        private bool ValidateCompanyExistence(int companyId)
        {
            if (!this.companyHandler.CheckCompanyExistence(companyId))
            {
                throw new ArgumentException("El nombre de la compañia no fue encontrado.");
            }
            return true;
        }

        private bool ValidateCompanyCanBeDeleted(int companyId)
        {
            if (this.companyHandler.IsHeadquarters(companyId))
            {
                throw new InvalidOperationException("La compañia no se puede eliminar porque es la casa matriz.");
            }

            if (this.companyHandler.HasPendingOrders(companyId))
            {
                throw new InvalidOperationException("La compañia no puede tener pedidos pendientes.");
            }
            return true;
        }

        private bool ValidateCompanyId(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentOutOfRangeException("La identificacion de empresa debe ser un numero valido.");
            }
            return true;
        }
    }
}
