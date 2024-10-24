using System.Data.SqlClient;
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
            List<OrderedProductModel> perishables = this.GetPerishable(orderID);
            List<OrderedProductModel> nonPerishables = this.GetNonPerishable(orderID);
            List<OrderedProductModel> fullList = new List<OrderedProductModel>();
            for (int i = 0; i < perishables.Count; i++) fullList.Add(perishables[i]);
            for(int i = 0;i < nonPerishables.Count; i++) fullList.Add(nonPerishables[i]);
            if (fullList.Count <= 0) throw new Exception("Couldnt find Products in order");
            return fullList;
        }

        private List<OrderedProductModel> GetPerishable(int orderID)
        {
            // TODO Finish method
            List<OrderedProductModel> productList = new List<OrderedProductModel>();
            return productList;
        }

        private List<OrderedProductModel> GetNonPerishable(int orderID)
        {
            // TODO Finish method
            List<OrderedProductModel> productList = new List<OrderedProductModel>();
            return productList;
        }

    }
}
