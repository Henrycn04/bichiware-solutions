using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using backend.Domain;
namespace backend.Infrastructure
{
    public class MonthlyShippingCostHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        private string queryString;
        public MonthlyShippingCostHandler() 
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
            queryString = "GetMonthlyShippingCost";
        }

        public List<MonthlyShippingResponseModel> GetMonthlyShippingCost(MonthlyShippingRequestModel request)
        {
            var command = GenerateQuery(request);
            return GetData(command);
        }

        private SqlCommand GenerateQuery(MonthlyShippingRequestModel request)
        {
            var query = new SqlCommand(queryString, _connection);
            query.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrWhiteSpace(request.startDate))
            {
                query.Parameters.AddWithValue("@StartDate", request.startDate);
            }
            if (!string.IsNullOrWhiteSpace(request.endDate))
            {
                query.Parameters.AddWithValue("@EndDate", request.endDate);
            }
            return query;
        }

        private List<MonthlyShippingResponseModel> GetData(SqlCommand query)
        {
            List<MonthlyShippingResponseModel> orderList = new List<MonthlyShippingResponseModel>();
            int month, year;
            double cost;
            _connection.Open();
            var reader = query.ExecuteReader();
            while (reader.Read())
            {
                month = Int32.Parse(reader["Month"].ToString());
                year = Int32.Parse(reader["Year"].ToString());
                cost = Double.Parse(reader["Cost"].ToString());
                orderList.Add(CreateResponse(month, year, cost));
            }
            _connection.Close();
            return orderList;
        }

        private MonthlyShippingResponseModel CreateResponse(int month, int  year, double cost)
        {
            MonthlyShippingResponseModel response = new MonthlyShippingResponseModel();
            response.month = month;
            response.year = year;
            response.cost = cost;
            return response;
        }
    }
}
