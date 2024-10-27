using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using backend.Domain;
using backend.Models;
using MailKit.Search;

namespace backend.Handlers
{
    public class ConfirmOrdersToCompaniesHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public ConfirmOrdersToCompaniesHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public List<ConfirmOrderForCompaniesModel> GetDataForEmails(int orderID)
        {
            List<ConfirmOrderForCompaniesModel> companiesData = new List<ConfirmOrderForCompaniesModel>();
            SqlCommand commandForQuery = new SqlCommand("FindCompaniesDataFromOrderID", _connection);
            commandForQuery.CommandType = CommandType.StoredProcedure;
            commandForQuery.Parameters.Add(new SqlParameter("@OrderID", orderID));
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    ConfirmOrderForCompaniesModel confirmOrderForCompaniesModel = new ConfirmOrderForCompaniesModel
                    {
                        CompanyId = reader.GetInt32("CompanyID"),
                        CompanyName = reader["CompanyName"].ToString(),
                        CompanyEmail = reader["EmailAddress"].ToString(),
                    };
                    List<OrderProductModel> products = new List<OrderProductModel>();
                    confirmOrderForCompaniesModel.OrderProducts = products;
                    companiesData.Add(confirmOrderForCompaniesModel);
                }
            }
            _connection.Close();
            for (int  i = 0; i < companiesData.Count; i++)
            {
                companiesData[i] = GetProducts(companiesData[i], orderID, companiesData[i].CompanyId);
                companiesData[i] = GetCosts(companiesData[i], orderID, companiesData[i].CompanyId);
            }
            return companiesData;
        }

        private ConfirmOrderForCompaniesModel GetProducts(ConfirmOrderForCompaniesModel model, int orderID, int companyID)
        {
            SqlCommand commandForQuery = new SqlCommand("FindOrderedProductsRelatedToACompany", _connection);
            commandForQuery.CommandType = CommandType.StoredProcedure;
            commandForQuery.Parameters.Add(new SqlParameter("@OrderID", orderID));
            commandForQuery.Parameters.Add(new SqlParameter("@CompanyID", companyID));
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrderProductModel product = new OrderProductModel
                    {
                        Name = reader["ProductName"].ToString(),
                        Category = reader["Category"].ToString(),
                        Company = reader["CompanyName"].ToString(),
                        PriceInColones = (double)reader.GetDecimal("ProductPrice"),
                        Amount = (int)reader.GetInt32("Quantity"),
                        ImageURL = reader["ImageURL"].ToString(),
                        CompanyID = (int)reader.GetInt32("CompanyID"),

                    };
                    model.OrderProducts.Add(product);
                }
            }
            _connection.Close();
            return model;
        }

        private ConfirmOrderForCompaniesModel GetCosts(ConfirmOrderForCompaniesModel model, int orderID, int companyID)
        {
            double totalCost = 0;
            for (int i = 0; i < model.OrderProducts.Count; i++)
            {
                totalCost += (model.OrderProducts[i].Amount * model.OrderProducts[i].PriceInColones);
            }
            model.ProductCost = totalCost;
            // 13% IVA
            model.Taxes = totalCost * 0.13;
            model.ShippingCost = 0;
            return model;
        }
    }
}
