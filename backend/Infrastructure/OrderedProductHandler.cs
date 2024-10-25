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

        public List<OrderedProductModel> GetProducts(int orderID) {
            List<OrderedProductModel> perishables = this.GetProducts(orderID, "FindOrderedPerishables");
            List<OrderedProductModel> nonPerishables = this.GetProducts(orderID, "FindOrderedNonPerishables");
            List<OrderedProductModel> fullList = new List<OrderedProductModel>();
            for (int i = 0; i < perishables.Count; i++) fullList.Add(perishables[i]);
            for(int i = 0;i < nonPerishables.Count; i++) fullList.Add(nonPerishables[i]);
            if (fullList.Count <= 0) throw new Exception("Couldnt find Products in order");
            return fullList;
        }

        private List<OrderedProductModel> GetProducts(int orderID, string procedure)
        {
            List<OrderedProductModel> productList = new List<OrderedProductModel>();
            var commandGetter = new SqlCommand(procedure, _connection);
            commandGetter.CommandType = CommandType.StoredProcedure;
            string name;
            float price;
            int quantity, companyID;
            commandGetter.Parameters.AddWithValue("@OID", orderID);
            _connection.Open();
            var reader = commandGetter.ExecuteReader();
            while (reader.Read())
            {
                name = reader["ProductName"].ToString();
                price = float.Parse(reader["Cost"].ToString());
                quantity = Int32.Parse(reader["Quantity"].ToString());
                companyID = Int32.Parse(reader["CompanyID"].ToString());
                productList.Add(CreateModel(name, price, quantity, companyID));
            }
            _connection.Close();
            return productList;
        }

        private OrderedProductModel CreateModel(string name, float price,
            int quantity, int companyID)
        {
            OrderedProductModel model = new OrderedProductModel();
            model.productName = name;
            model.totalPrice = price;
            model.quantity = quantity;
            model.companyID = companyID;
            return model;
        }
    }
}
