using backend.Models;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public interface ICancelledOrdersHandler
    {
        int CheckIfUserExists(int userID);
        int CheckIfCompanyExists(int companyID);
        int CheckIfUserIsAdminOrEntrepeneur(int userID);
        List<CancelledOrdersModel> GetCancelledOrders(FiltersCompletedOrdersModel filters);
    }

    public class CancelledOrdersHandler : ICancelledOrdersHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public CancelledOrdersHandler() {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }
        
        public int CheckIfUserExists(int userID)
        {
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID";
            using (SqlCommand commandForQuery = new SqlCommand(query, _connection))
            {
                commandForQuery.Parameters.AddWithValue("@UserID", userID);
                _connection.Open();
                int userCount = (int)commandForQuery.ExecuteScalar();
                _connection.Close();
                return userCount;
            }
        }

        public int CheckIfCompanyExists(int companyID)
        {
            string query = "SELECT COUNT(1) FROM Company WHERE CompanyID = @CompanyID";
            using (SqlCommand commandForQuery = new SqlCommand(query, _connection))
            {
                commandForQuery.Parameters.AddWithValue("@CompanyID", companyID);
                _connection.Open();
                int companyCount = (int)commandForQuery.ExecuteScalar();
                _connection.Close();
                return companyCount;
            }
        }

        public int CheckIfUserIsAdminOrEntrepeneur(int userID)
        {
            string query = "SELECT UserType FROM UserData WHERE UserID = @UserID";
            using (SqlCommand commandForQuery = new SqlCommand(query, _connection))
            {
                commandForQuery.Parameters.AddWithValue("@UserID", userID);
                _connection.Open();
                int userType = (int)commandForQuery.ExecuteScalar();
                _connection.Close();
                return userType;
            }
        }

        public List<CancelledOrdersModel> GetCancelledOrders(FiltersCompletedOrdersModel filters)
        {
            List<CancelledOrdersModel> cancelledOrders = new List<CancelledOrdersModel>();
            string getOrdersQuery = QueryBuilderWithFilters(filters);

            _connection.Open();
                using (SqlCommand commandForQuery = new SqlCommand(getOrdersQuery, _connection))
                {
                    using (var cancelledOrdersReader = commandForQuery.ExecuteReader())
                    {
                        while (cancelledOrdersReader.Read())
                        {
                            var order = new CancelledOrdersModel
                            {
                                OrderID = (int)cancelledOrdersReader["OrderID"],
                                AllCompanies = cancelledOrdersReader["AllCompanies"] != DBNull.Value ? cancelledOrdersReader["AllCompanies"].ToString() : null,
                                Quantity = cancelledOrdersReader["Quantity"] as int?,
                                CreationDate = cancelledOrdersReader["CreationDate"] as DateTime?,
                                CancellationDate = cancelledOrdersReader["CancellationDate"] as DateTime?,
                                ProductCost = (decimal)cancelledOrdersReader["ProductCost"],
                                CancelledBy = cancelledOrdersReader["CancelledBy"] is int cancelledByValue
                                    ? cancelledByValue switch
                                    {
                                        1 => "Usuario",
                                        2 => "Empresario",
                                        3 => "Administrador",
                                    }
                                    : null,
                                ShippingCost = cancelledOrdersReader["ShippingCost"] as decimal?,
                                Total = (decimal)cancelledOrdersReader["Total"]
                            };
                            cancelledOrders.Add(order);
                        }
                    }
                }
           _connection.Close();
            return cancelledOrders;
        }

        private string QueryBuilderWithFilters(FiltersCompletedOrdersModel filters)
        {
            string query = "EXEC GetCancelledOrdersByFilters ";

            var parameters = new Dictionary<string, object>
            {
                { "@CompanyID", filters.CompanyID ?? (object)DBNull.Value }
            };

            foreach (var param in parameters)
            {
                if (param.Value == DBNull.Value)
                {
                    query += $"{param.Key} = NULL, ";
                }
                else if (param.Value is string)
                {
                    query += $"{param.Key} = '{param.Value}', ";
                }
                else if (param.Value is DateTime || param.Value is DateTime?)
                {
                    query += $"{param.Key} = '{((DateTime)param.Value).ToString("yyyy-MM-dd HH:mm:ss")}', ";
                }
                else
                {
                    query += $"{param.Key} = {param.Value}, ";
                }
            }
            query = query.TrimEnd(',', ' ');
            return query;
        }
    }
}
