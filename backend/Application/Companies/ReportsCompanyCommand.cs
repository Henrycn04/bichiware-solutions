using backend.Domain;
using backend.Infrastructure;
using backend.Models;

namespace backend.Application
{
    public class ReportsCompanyCommand
    {
        private readonly IReportsCompanyHandler reportsCompanyHandler;
        private readonly IUpdateCompanyHandler updateCompanyHandler;

        public ReportsCompanyCommand(IReportsCompanyHandler reportsCompanyHandler,
            IUpdateCompanyHandler updateCompanyHandler)
        {
            this.reportsCompanyHandler = reportsCompanyHandler;
            this.updateCompanyHandler = updateCompanyHandler;
        }

        public List<PendingOrderReport> GetPendingOrdersReport(FiltersCompletedOrdersModel filter)
        {
            if (this.ValidateFilters(filter))
            {
                return this.reportsCompanyHandler.GeneratePendingOrdersReport(filter);
            }
            return new List<PendingOrderReport>();
        }

        private bool ValidateFilters(FiltersCompletedOrdersModel filter)
        {
            return ValidateNullFilter(filter)
                && ValidateValidUserId(filter.UserID)
                && ValidateValidCompanyId(filter.CompanyID)
                && ValidateUserType(filter.UserID)
                && ValidateCompanyHasPendingOrders(filter.CompanyID);
        }

        private bool ValidateValidUserId(int userId)
        {
            if (userId < 0)
            {
                throw new ArgumentException("La identificacion de usuario no es valida.");
            }
            return true;
        }

        private bool ValidateValidCompanyId(int? companyId)
        {
            if (companyId == null || companyId < 0)
            {
                throw new ArgumentException("La identificacion de compañia no es valida.");
            }
            return true;
        }

        private bool ValidateUserType(int userId)
        {
            if (!this.reportsCompanyHandler.CheckUserType(userId))
            {
                throw new UnauthorizedAccessException("Usted no es un administrador o empresario para realizar esta operacion.");
            }
            return true;
        }

        private bool ValidateCompanyHasPendingOrders(int? companyId)
        {
            if (companyId != null && !updateCompanyHandler.HasPendingOrders(companyId.Value))
            {
                throw new KeyNotFoundException("No se encontraron pedidos pendientes para esta compañia.");
            }
            return true;
        }

        private bool ValidateNullFilter(FiltersCompletedOrdersModel filter)
        {
            if (filter == null)
            {
                throw new NullReferenceException("La identificación de usuario no puede ser nula.");
            }
            return true;
        }
    }
}
