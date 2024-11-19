using System.Text.RegularExpressions;
using backend.Domain;

namespace backend.Services
{
    public class ValidateClientReportRequestService
    {
        private readonly Regex dateFormat;
        private readonly Regex companyFormat;
        public ValidateClientReportRequestService() {
            dateFormat = new Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$");
            companyFormat = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]{0,20}$");
        }

        public bool ValidateData(ClientReportRequestModel request)
        {
            
            return ValidateRequestType(request.RequestType) &&
                ValidateDate(request.CreationStartDate) && ValidateDate(request.CreationEndDate) &&
                ValidateDate(request.SentStartDate) && ValidateDate(request.SentEndDate) &&
                ValidateDate(request.DeliveredStartDate) && ValidateDate(request.DeliveredEndDate) &&
                ValidateDate(request.CancelledStartDate) && ValidateDate(request.CancelledEndDate) &&
                ValidateCancelled(request.CancelledBy) && 
                ValidateNumberInterval(request.minShippingCost, request.maxShippingCost) &&
                ValidateNumberInterval(request.minProductCost, request.maxProductCost) &&
                ValidateNumberInterval(request.minTotalCost, request.maxTotalCost) &&
                ValidateNumberInterval(request.minQuantity, request.maxQuantity) &&
                ValidateComapnyName(request.CompanyName);
        }

        private bool ValidateRequestType (int type)
        {
            bool valid = (type == 5 || type == 2 || type == 3);
            if (!valid) Console.WriteLine("Error: " + type + " is not a valid request type");
            return valid;
        }

        private bool ValidateDate (string date)
        {
            bool valid = dateFormat.IsMatch(date) || date == "";
            if (!valid) Console.WriteLine("Error: " + date + " is not a valid date");
            return valid;
        }
        private bool ValidateCancelled(int CancelledBy)
        {
            bool valid = CancelledBy >= 0 && CancelledBy <= 3;
            if (!valid) Console.WriteLine("Error: " + CancelledBy + " is not a valid CancelledBy value");
            return valid;
        }

        private bool ValidateNumberInterval (int min, int max) 
        {
            bool valid = (min <= 0 || max <= 0) || min <= max;
            if (!valid) Console.WriteLine("Error: " + min + ", " + max + " is not a valid interval");
            return valid;
        }

        private bool ValidateNumberInterval(double min, double max)
        {
            bool valid = (min <= 0 || max <= 0) || min <= max;
            if (!valid) Console.WriteLine("Error: " + min + ", " + max + " is not a valid interval");
            return valid;
        }

        private bool ValidateCompanyName(string name)
        {
            bool valid = companyFormat.IsMatch(name);
            if (!valid) Console.WriteLine("Error: " + name + " is not a valid company name");
            return valid;
        }

    }
}
