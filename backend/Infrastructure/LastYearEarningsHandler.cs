using backend.Domain;
using backend.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using backend.Domain;
namespace backend.Infrastructure
{
    public interface ILastYearEarningsHandler
    {
        Task<bool> UserExists(int userId);
        Task<int> UserIsAdminOrEntrepeneur(int userId);
        Task<bool> CompanyExists(int companyID);
        Task<List<LastYearOrdersModel>> GetOrdersByFilterAsync(LastYearEarningsElementsModel filter);
    }
    public class LastYearEarningsHandler : ILastYearEarningsHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public LastYearEarningsHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public async Task<bool> UserExists(int userId)
        {
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID AND Deleted != 1";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();
                int count = (int)await cmd.ExecuteScalarAsync();
                _connection.Close();
                return count > 0;
            }
        }
        public async Task<int> UserIsAdminOrEntrepeneur(int userId)
        {
            string query = "SELECT UserType FROM UserData WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();
                var userType = await cmd.ExecuteScalarAsync();
                _connection.Close();

                if (userType != null && ((int)userType == 2 || (int)userType == 3))
                {
                    return (int)userType;
                }
            }

            return -1;
        }


        public async Task<bool> CompanyExists(int companyID)
        {
            string query = "SELECT COUNT(1) FROM Company WHERE CompanyID = @CompanyID";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@CompanyID", companyID);
                _connection.Open();
                int count = (int)await cmd.ExecuteScalarAsync();
                _connection.Close();
                return count > 0;
            }
        }

        public async Task<List<LastYearOrdersModel>> GetOrdersByFilterAsync(LastYearEarningsElementsModel filter)
        {
            var orders = new List<LastYearOrdersModel>();
            string getOrdersQuery = QueryBuilderWithFilters(filter);

            _connection.Open();
            try
            {
                using (var ordersCmd = new SqlCommand(getOrdersQuery, _connection))
                {
                    using (var ordersReader = await ordersCmd.ExecuteReaderAsync())
                    {
                        while (await ordersReader.ReadAsync())
                        {
                            var order = new LastYearOrdersModel
                            {
                                OrderID = (int)ordersReader["OrderID"],
                                CreationDate = ordersReader["CreationDate"] as DateTime?,
                                SentDate = ordersReader["SentDate"] as DateTime?,
                                DeliveredDate = ordersReader["DeliveredDate"] as DateTime?,
                                ProductCost = (decimal)ordersReader["ProductCost"],
                                ShippingCost = ordersReader["ShippingCost"] as decimal?,
                                Total = (decimal)ordersReader["Total"]
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }

            return orders;
        }

        private string QueryBuilderWithFilters(LastYearEarningsElementsModel filter)
        {
            string getOrdersQuery = "EXEC GetLastYearOrders ";

            var parameters = new Dictionary<string, object>
            {
                { "@CompanyID", filter.CompanyID ?? (object)DBNull.Value }
            };

            foreach (var param in parameters)
            {
                if (param.Value == DBNull.Value)
                {
                    getOrdersQuery += $"{param.Key} = NULL, ";
                }
                else if (param.Value is string)
                {
                    getOrdersQuery += $"{param.Key} = '{param.Value}', ";
                }
                else if (param.Value is DateTime || param.Value is DateTime?)
                {
                    getOrdersQuery += $"{param.Key} = '{((DateTime)param.Value).ToString("yyyy-MM-dd HH:mm:ss")}', ";
                }
                else
                {
                    getOrdersQuery += $"{param.Key} = {param.Value}, ";
                }
            }
            getOrdersQuery = getOrdersQuery.TrimEnd(',', ' ');
            return getOrdersQuery;
        }
    }
}
