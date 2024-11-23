using backend.Models;
using MailKit.Search;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public interface IAdmin_EntrepreneurOrdersHandler
    {
        List<UserOrdersModel> GetOrdersForAdmin();
        List<EntrepreneurOrdersModel> GetOrdersForEntrepreneur(int userID);
        int CheckIfUserHasCompanies(int userID);
        int CheckIfUserExists(int userID);
    }

    public class Admin_EntrepreneurOrdersHandler : IAdmin_EntrepreneurOrdersHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public Admin_EntrepreneurOrdersHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int CheckIfUserExists(int userID)
        {
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID AND Deleted != 1";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UserID", userID);
            _connection.Open();
            int count = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return count;
        }

        public int CheckIfUserHasCompanies(int userID)
        {
            var query =
                @"
                 SELECT COUNT(*)
                FROM CompanyProfiles
                WHERE UserID = @userID
                ";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@userID", userID);
            _connection.Open();
            int numberOfCompanies = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return numberOfCompanies;
        }

        public List<UserOrdersModel> GetOrdersForAdmin()
        {
            var orders = GetOrders_Admin();

            foreach (var order in orders)
            {
                var products = GetProductsByOrderId(order.OrderID);
                order.Products.AddRange(products);
            }

            return orders;
        }

        public List<EntrepreneurOrdersModel> GetOrdersForEntrepreneur(int userID)
        {
            int companyID = GetCompanyRelatedToUserID(userID);
            List<int> orderIDs = GetOrdersRelatedToACompany(companyID);
            if (orderIDs.Count > 0)
            {
                var orders = GetOrders_Entrepreneur(orderIDs);
                orders = GetCompanyName(companyID, orders);

                foreach (var order in orders)
                {
                    var products = GetProductsByOrderId(order.OrderID);
                    order.Products.AddRange(products);
                }
                return orders;

            }
            List<EntrepreneurOrdersModel> emptyList = new List<EntrepreneurOrdersModel>();
            return emptyList;
        }

        private List<UserOrdersModel> GetOrders_Admin()
        {
            var orders = new List<UserOrdersModel>();
            string getOrdersQuery = "EXEC GetActiveOrdersForAdmins";

            _connection.Open();
            try
            {
                using (var ordersCmd = new SqlCommand(getOrdersQuery, _connection))
                {
                    using (var ordersReader = ordersCmd.ExecuteReader())
                    {
                        while (ordersReader.Read())
                        {
                            var order = new UserOrdersModel
                            {
                                OrderID = (int)ordersReader["OrderID"],
                                UserID = ordersReader["UserID"] as int?,
                                AddressID = ordersReader["AddressID"] as int?,
                                FeeID = ordersReader["FeeID"] as int?,
                                Tax = (decimal)ordersReader["Tax"],
                                ShippingCost = (decimal)ordersReader["ShippingCost"],
                                ProductCost = (decimal)ordersReader["ProductCost"],
                                OrderStatus = ordersReader["OrderStatus"] as int?,
                                DeliveryDate = (DateTime)ordersReader["DeliveryDate"]
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

        private int GetCompanyRelatedToUserID(int userID)
        {
            var query =
                @"SELECT TOP 1 
                    CompanyID
                    FROM CompanyProfiles
                    WHERE UserID = @userID;
                    ";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@userID", userID);
            _connection.Open();
            int companyID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return companyID;
        }

        private List<int> GetOrdersRelatedToACompany(int companyID)
        {
            List<int> orderIDs = new List<int>();
            var query =
                @"SELECT TOP 10 OrderID
                    FROM (
                        SELECT o.OrderID
                        FROM Orders o
                        INNER JOIN OrderedNonPerishable onp ON onp.OrderID = o.OrderID
                        INNER JOIN NonPerishableProduct npp ON npp.ProductID = onp.ProductID
                        WHERE CompanyID = @companyID1 AND o.OrderStatus IN (1,2,4) 

                        UNION

                        SELECT o.OrderID
                        FROM Orders o
                        INNER JOIN OrderedPerishable op ON op.OrderID = o.OrderID
                        INNER JOIN PerishableProduct pp ON pp.ProductID = op.ProductID
                        WHERE CompanyID = @companyID2 AND o.OrderStatus IN (1,2,4)
                    ) AS CombinedResults;
                    ";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@companyID1", companyID);
            commandForQuery.Parameters.AddWithValue("@companyID2", companyID);

            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    orderIDs.Add(reader.GetInt32(0));
                }
            }
            _connection.Close();
            return orderIDs;
        }

        private List<EntrepreneurOrdersModel> GetOrders_Entrepreneur(List<int> orderIDs)
        {
            var orders = new List<EntrepreneurOrdersModel>();

            foreach (var orderID in orderIDs)
            {
                string getOrdersQuery = "EXEC GetActiveOrdersForEntrepreneurs @orderID";

                _connection.Open();
                try
                {
                    using (var ordersCmd = new SqlCommand(getOrdersQuery, _connection))
                    {
                        ordersCmd.Parameters.AddWithValue("@orderID", orderID);
                        using (var ordersReader = ordersCmd.ExecuteReader())
                        {
                            while (ordersReader.Read())
                            {
                                var order = new EntrepreneurOrdersModel
                                {
                                    OrderID = (int)ordersReader["OrderID"],
                                    UserID = ordersReader["UserID"] as int?,
                                    AddressID = ordersReader["AddressID"] as int?,
                                    FeeID = ordersReader["FeeID"] as int?,
                                    Tax = (decimal)ordersReader["Tax"],
                                    ShippingCost = (decimal)ordersReader["ShippingCost"],
                                    ProductCost = (decimal)ordersReader["ProductCost"],
                                    OrderStatus = ordersReader["OrderStatus"] as int?,
                                    DeliveryDate = (DateTime)ordersReader["DeliveryDate"]
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
            }

            return orders;
        }

        private List<EntrepreneurOrdersModel> GetCompanyName(int companyID, List<EntrepreneurOrdersModel> orders)
        {
            var query =
                @"SELECT
                    CompanyName
                    FROM Company
                    WHERE CompanyID = @companyID
                    ";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@companyID", companyID);
            _connection.Open();
            string companyName = (string)commandForQuery.ExecuteScalar();
            _connection.Close();

            foreach (var order in orders)
            {
                order.CompanyName = companyName;
            }

            return orders;
        }

        private List<UserOrderProductModel> GetProductsByOrderId(int orderId)
        {
            var products = new List<UserOrderProductModel>();
            string getProductsQuery = "EXEC GetProductsByOrderID @OrderID";

            _connection.Open();
            try
            {
                using (var productsCmd = new SqlCommand(getProductsQuery, _connection))
                {
                    productsCmd.Parameters.AddWithValue("@OrderID", orderId);

                    using (var productsReader = productsCmd.ExecuteReader())
                    {
                        while (productsReader.Read())
                        {
                            var product = new UserOrderProductModel
                            {
                                ProductID = (int)productsReader["ProductID"],
                                ProductName = productsReader["ProductName"].ToString(),
                                Quantity = (int)productsReader["Quantity"],
                                ProductPrice = (decimal)productsReader["ProductPrice"],
                                BatchNumber = productsReader["BatchNumber"] as int?,
                                ProductType = productsReader["ProductType"].ToString()
                            };

                            products.Add(product);
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }

            return products;
        }
    }
}
