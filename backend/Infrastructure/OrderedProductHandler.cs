using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using backend.Domain;

namespace backend.Infrastructure
{
    public class OrderedProductHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public OrderedProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public List<OrderProductModel> GetProducts(int orderID) {
            List<OrderProductModel> perishables = this.GetProducts(orderID, "FindOrderedPerishables");
            List<OrderProductModel> nonPerishables = this.GetProducts(orderID, "FindOrderedNonPerishables");
            List<OrderProductModel> fullList = new List<OrderProductModel>();
            for (int i = 0; i < perishables.Count; i++) fullList.Add(perishables[i]);
            for(int i = 0;i < nonPerishables.Count; i++) fullList.Add(nonPerishables[i]);
            if (fullList.Count <= 0) throw new Exception("Couldnt find Products in order");
            return fullList;
        }

        private List<OrderProductModel> GetProducts(int orderID, string procedure)
        {
            List<OrderProductModel> productList = new List<OrderProductModel>();
            var commandGetter = new SqlCommand(procedure, _connection);
            commandGetter.CommandType = CommandType.StoredProcedure;
            string name, category, companyName, imageURL;
            double price;
            int quantity, companyID;
            commandGetter.Parameters.AddWithValue("@OID", orderID);
            _connection.Open();
            var reader = commandGetter.ExecuteReader();
            while (reader.Read())
            {
                name = reader["ProductName"].ToString();
                price = double.Parse(reader["Cost"].ToString());
                quantity = Int32.Parse(reader["Quantity"].ToString());
                companyID = Int32.Parse(reader["CompanyID"].ToString());
                category = reader["Category"].ToString();
                companyName = reader["CompanyName"].ToString();
                imageURL = reader["ImageURL"].ToString();
                productList.Add(CreateModel(name, price, quantity, companyID,
                   category, imageURL, companyName));
            }
            _connection.Close();
            return productList;
        }

        private OrderProductModel CreateModel(string name, double price,
            int quantity, int companyID, string category, string image,
            string companyName )
        {
            OrderProductModel model = new OrderProductModel();
            model.Name = name;
            model.PriceInColones = price;
            model.Amount = quantity;
            model.CompanyID = companyID;
            model.Category = category;
            model.ImageURL = image;
            model.Company = companyName;
            return model;
        }
    }
}
