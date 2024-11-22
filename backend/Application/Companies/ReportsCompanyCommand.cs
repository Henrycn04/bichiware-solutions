using backend.Domain;
using backend.Infrastructure;
using backend.Models;

namespace backend.Application
{
    public class ReportsCompanyCommand
    {
        private readonly IReportsCompanyHandler reportsCompanyHandler;
        private readonly IUpdateCompanyHandler updateCompanyHandler;

        public ReportsCompanyCommand(IReportsCompanyHandler reportsCompanyHandler)
        {
            this.reportsCompanyHandler = reportsCompanyHandler;
        }

        public List<PendingOrderReport> GetPendingOrdersReport(FiltersCompletedOrdersModel filter)
        {
            if (this.ValidateFilters(filter))
            {
                // return this.reportsCompanyHandler.GeneratePendingOrdersReport(filter);
            }
            return new List<PendingOrderReport>();
        }

        private bool ValidateFilters(FiltersCompletedOrdersModel filter)
        {
            return true;
        }

        private bool ValidateValidUserId(int userId)
        {
            if (userId < 0)
            {
                throw new ArgumentException("La identificacion de usuario no es valida.");
            }
            return true;
        }

        private bool ValidateValidCompanyId(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentException("La identificacion de compañia no es valida.");
            }
            return true;
        }

        private bool ValidateUserType(int userId)
        {
            //if (!this.reportsCompanyHandler.CheckUserType(userId))
            //{
            //    throw new UnauthorizedAccessException("Usted no es un administrador o empresario para realizar esta operacion.");
            //}
            return true;
        }

        private bool ValidateCompanyHasPendingOrders(int companyId)
        {
            if (!updateCompanyHandler.HasPendingOrders(companyId))
            {
                throw new KeyNotFoundException("No se encontraron pedidos pendientes para esta compañia.");
            }
            return true;
        }
    }
}
