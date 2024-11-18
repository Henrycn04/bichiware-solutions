using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml.Linq;
using backend.Domain;

namespace backend.Infrastructure
{
    public class ClientReportHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public ClientReportHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("RegisterUser");
            _connection = new SqlConnection(_routeConnection);
        }

        public List<ClientReportResponseModel> GetCompletedReport (ClientReportRequestModel request)
        {
            string query = "";
            var commandForQuery = new SqlCommand(query, _connection);
            List<ClientReportResponseModel> reportList = GenerateCompletedReports(commandForQuery);
            return reportList;
        }

        private List<ClientReportResponseModel> GenerateCompletedReports (SqlCommand commandForQuery)
        {
            List<ClientReportResponseModel> orderList = new List<ClientReportResponseModel>();
            string orderID, AllCompanies, Quantity, CreationDate, SentDate,
                DeliveredDate, ProductCost, DeliveryCost, Total;
            _connection.Open();
            var reader = commandForQuery.ExecuteReader();
            while (reader.Read())
            {
                orderID = reader["OrderID"].ToString();
                AllCompanies = reader["AllCompanies"].ToString();
                Quantity = reader["Quantity"].ToString();
                CreationDate = reader["CreationDate"].ToString();
                SentDate = reader["SentDate"].ToString();
                DeliveredDate = reader["DeliveredDate"].ToString();
                ProductCost = reader["ProductCost"].ToString();
                DeliveryCost = reader["ShippingCost"].ToString();
                Total = reader["Total"].ToString();
                orderList.Add(CreateCompletedModel(orderID, AllCompanies, 
                    Quantity, CreationDate, SentDate, DeliveredDate, 
                    ProductCost, DeliveryCost, Total));
            }
            _connection.Close();
            return orderList;
        }

        private ClientReportResponseModel CreateCompletedModel(
            string orderID, string AllCompanies, string Quantity,
            string CreationDate, string SentDate, string DeliveredDate, 
            string ProductCost, string DeliveryCost, string Total)
        {
            ClientReportResponseModel report = new ClientReportResponseModel();
            report.OrderID = Int32.Parse(orderID);
            report.Companies = AllCompanies;
            report.Quantity = Int32.Parse(Quantity);
            report.CreationDate = CreationDate;
            report.SentDate = SentDate;
            report.DeliveredDate = DeliveredDate;
            report.ProductCost = Double.Parse(ProductCost);
            report.DeliveryCost = Double.Parse(DeliveryCost);
            report.TotalCost = Double.Parse(Total);
            return report;
        }

        public List<ClientReportResponseModel> GetCancelledReport (ClientReportRequestModel request)
        {
            string query = "";
            var commandForQuery = new SqlCommand(query, _connection);
            List<ClientReportResponseModel> reportList = this.GenerateDeletedReports(commandForQuery);

            return reportList;
        }

        private List<ClientReportResponseModel> GenerateDeletedReports(SqlCommand commandForQuery)
        {
            List<ClientReportResponseModel> orderList = new List<ClientReportResponseModel>();
            string orderID, AllCompanies, Quantity, CreationDate, CancelledDate,
                CancelledBy, ProductCost, DeliveryCost, Total;
            _connection.Open();
            var reader = commandForQuery.ExecuteReader();
            while (reader.Read())
            {
                orderID = reader["OrderID"].ToString();
                AllCompanies = reader["AllCompanies"].ToString();
                Quantity = reader["Quantity"].ToString();
                CreationDate = reader["CreationDate"].ToString();
                CancelledDate = reader["CancelledDate"].ToString();
                CancelledBy = reader["CancelledBy"].ToString();
                ProductCost = reader["ProductCost"].ToString();
                DeliveryCost = reader["ShippingCost"].ToString();
                Total = reader["Total"].ToString();
                orderList.Add(CreateDeletedModel(orderID, AllCompanies,
                    Quantity, CreationDate, CancelledDate, CancelledBy,
                    ProductCost, DeliveryCost, Total));
            }
            _connection.Close();
            return orderList;
        }

        private ClientReportResponseModel CreateDeletedModel(
            string orderID, string AllCompanies, string Quantity,
            string CreationDate, string CancelledDate, string CancelledBy,
            string ProductCost, string DeliveryCost, string Total)
        {
            ClientReportResponseModel report = new ClientReportResponseModel();
            report.OrderID = Int32.Parse(orderID);
            report.Companies = AllCompanies;
            report.Quantity = Int32.Parse(Quantity);
            report.CreationDate = CreationDate;
            report.CancelledDate = CancelledDate;
            report.CancelledBy = Int32.Parse(CancelledBy);
            report.ProductCost = Double.Parse(ProductCost);
            report.DeliveryCost = Double.Parse(DeliveryCost);
            report.TotalCost = Double.Parse(Total);
            return report;
        }

        public List<ClientReportResponseModel> GetCurrentReport(ClientReportRequestModel request)
        {
            string query = "";
            var commandForQuery = new SqlCommand(query, _connection);
            List<ClientReportResponseModel> reportList = this.GenerateCurrentReports(commandForQuery);

            return reportList;
        }

        private List<ClientReportResponseModel> GenerateCurrentReports(SqlCommand commandForQuery)
        {
            List<ClientReportResponseModel> orderList = new List<ClientReportResponseModel>();
            string orderID, AllCompanies, Quantity, CreationDate, SentDate,
                Status, ProductCost, DeliveryCost, Total;
            _connection.Open();
            var reader = commandForQuery.ExecuteReader();
            while (reader.Read())
            {
                orderID = reader["OrderID"].ToString();
                AllCompanies = reader["AllCompanies"].ToString();
                Quantity = reader["Quantity"].ToString();
                CreationDate = reader["CreationDate"].ToString();
                SentDate = reader["SentDate"].ToString();
                Status = reader["Status"].ToString();
                ProductCost = reader["ProductCost"].ToString();
                DeliveryCost = reader["ShippingCost"].ToString();
                Total = reader["Total"].ToString();
                orderList.Add(CreateCurrentModel(orderID, AllCompanies,
                    Quantity, CreationDate, SentDate, Status,
                    ProductCost, DeliveryCost, Total));
            }
            _connection.Close();
            return orderList;
        }

        private ClientReportResponseModel CreateCurrentModel(
            string orderID, string AllCompanies, string Quantity,
            string CreationDate, string SentDate, string Status,
            string ProductCost, string DeliveryCost, string Total)
        {
            ClientReportResponseModel report = new ClientReportResponseModel();
            report.OrderID = Int32.Parse(orderID);
            report.Companies = AllCompanies;
            report.Quantity = Int32.Parse(Quantity);
            report.CreationDate = CreationDate;
            report.SentDate = SentDate;
            report.Status = Int32.Parse(Status);
            report.ProductCost = Double.Parse(ProductCost);
            report.DeliveryCost = Double.Parse(DeliveryCost);
            report.TotalCost = Double.Parse(Total);
            return report;
        }


    }
}
