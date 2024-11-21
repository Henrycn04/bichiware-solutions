using System.Text.RegularExpressions;
using backend.Domain;
namespace backend.Services
{
    public class ValidateMonthlyShippingCostRequestService
    {
        private readonly Regex dateFormat;
        public ValidateMonthlyShippingCostRequestService()
        {
            dateFormat = new Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$");
        }
        public bool ValidateData(MonthlyShippingRequestModel request)
        {
            return ValidateDate(request.startDate) && ValidateDate(request.endDate) &&
                    ValidateInterval(request.startDate, request.endDate);
        }

        private bool ValidateDate(string date)
        {
            bool valid = string.IsNullOrWhiteSpace(date) || dateFormat.IsMatch(date);
            if (!valid) Console.WriteLine("Error: " + date + " is not a valid date");
            return valid;
        }

        private bool ValidateInterval (string start, string end)
        {
            bool valid = start.CompareTo(end) <= 0;
            if (!valid) Console.WriteLine("Invalid interval (Start date is later than End date");
            return valid;
        }
    }
}
