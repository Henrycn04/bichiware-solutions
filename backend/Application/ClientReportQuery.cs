using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Services;
namespace backend.Application
{
    public class ClientReportQuery
    {
        private readonly ClientReportHandler Handler;
        private readonly ValidateClientReportRequestService checker;
        public ClientReportQuery()
        {
            checker = new ValidateClientReportRequestService();
            Handler = new ClientReportHandler();
        }

        public Task<List<ClientReportResponseModel>> GetReport(ClientReportRequestModel request)
        {
            if (!checker.ValidateData(request)) throw new Exception("Invalid Data");
            else
            {
                if(request.RequestType == 5) return Handler.GetCompletedReport(request);
                else if(request.RequestType == 3) return Handler.GetCancelledReport(request);
                else return Handler.GetCurrentReport(request);
            }
        }
    } 
}
