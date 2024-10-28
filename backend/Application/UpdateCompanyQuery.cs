using backend.Handlers;
using backend.Models;

namespace backend.Commands
{
    public class UpdateCompanyQuery
    {
        private readonly CompanyMainDataHandler _companyMainDataHandler;

        public UpdateCompanyQuery()
        {
            this._companyMainDataHandler = new CompanyMainDataHandler();
        }

        public CompanyProfileModel GetCompanyMainData(int companyID)
        {
            CompanyProfileModel model = this._companyMainDataHandler.GetCompanyMainData(companyID);
            return model;
        }

        private void CheckID(int OrderID) { 
            if (OrderID <= 0)
            {
                throw new FormatException("Formato de ID de empresa");
            }
        }
    }
}
