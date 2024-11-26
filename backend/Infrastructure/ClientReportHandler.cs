using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using backend.Domain;
using MailKit.Search;

namespace backend.Infrastructure
{
    public class ClientReportHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        private string procedure;
        public ClientReportHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
            procedure = "ClientGetOrders";
        }

        private bool[] GenerateString(ClientReportRequestModel request)
        {
            // This array is later used to dinamically add parameters to the query
            bool[] used = new bool[20];
            used[0] = request.UserID > 0;
            used[1] = !string.IsNullOrWhiteSpace(request.CreationStartDate);
            used[2] = !string.IsNullOrWhiteSpace(request.CreationEndDate);
            used[3] = !string.IsNullOrWhiteSpace(request.SentStartDate);
            used[4] = !string.IsNullOrWhiteSpace(request.SentEndDate);
            used[5] = !string.IsNullOrWhiteSpace(request.DeliveredStartDate);
            used[6] = !string.IsNullOrWhiteSpace(request.DeliveredEndDate);
            used[7] = !string.IsNullOrWhiteSpace(request.CancelledStartDate);
            used[8] = !string.IsNullOrWhiteSpace(request.CancelledEndDate);
            used[9] = request.CancelledBy > 0;
            used[10] = request.minShippingCost > 0;
            used[11] = request.maxShippingCost > 0;
            used[12] = request.minProductCost > 0;
            used[13] = request.maxProductCost > 0;
            used[14] = request.minTotalCost > 0;
            used[15] = request.maxTotalCost > 0;
            used[16] = request.minQuantity > 0;
            used[17] = request.maxQuantity > 0;
            used[18] = request.orderID > 0;
            used[19] = !string.IsNullOrWhiteSpace(request.CompanyName);
            return used;
        }

        private SqlCommand GenerateCommand(ClientReportRequestModel request, bool[] used)
        {
            SqlCommand command = new SqlCommand(procedure, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@OrderStatus", request.RequestType);
            if (used[0]) command.Parameters.AddWithValue("@UID", request.UserID);
            if (used[1]) command.Parameters.AddWithValue("@CreationStartDate", request.CreationStartDate);
            if (used[2]) command.Parameters.AddWithValue("@CreationEndDate", request.CreationEndDate);
            if (used[3]) command.Parameters.AddWithValue("@SentStartDate", request.SentStartDate);
            if (used[4]) command.Parameters.AddWithValue("@SentEndDate", request.SentEndDate);
            if (used[5]) command.Parameters.AddWithValue("@DeliveredStartDate", request.DeliveredStartDate);
            if (used[6]) command.Parameters.AddWithValue("@DeliveredEndDate", request.DeliveredEndDate);
            if (used[7]) command.Parameters.AddWithValue("@CancelledStartDate", request.CancelledStartDate);
            if (used[8]) command.Parameters.AddWithValue("@CancelledEndDate", request.CancelledEndDate);
            if (used[9]) command.Parameters.AddWithValue("@CancelledBy", request.CancelledBy);
            if (used[10]) command.Parameters.AddWithValue("@minShippingCost", request.minShippingCost);
            if (used[11]) command.Parameters.AddWithValue("@maxShippingCost", request.maxShippingCost);
            if (used[12]) command.Parameters.AddWithValue("@minProductCost", request.minProductCost);
            if (used[13]) command.Parameters.AddWithValue("@maxProductCost", request.maxProductCost);
            if (used[14]) command.Parameters.AddWithValue("@minTotal", request.minTotalCost);
            if (used[15]) command.Parameters.AddWithValue("@maxTotal", request.maxTotalCost);
            if (used[16]) command.Parameters.AddWithValue("@minQuantity", request.minQuantity);
            if (used[17]) command.Parameters.AddWithValue("@maxQuantity", request.maxQuantity);
            if (used[18]) command.Parameters.AddWithValue("@OrderID", request.orderID);
            if (used[19]) command.Parameters.AddWithValue("@CompanyName", request.CompanyName); 

            return command;
        }

        public List<ClientReportResponseModel> GetCompletedReport (ClientReportRequestModel request)
        {
            bool[] data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data);
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
            bool[] data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data);
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
                CancelledDate = reader["CancellationDate"].ToString();
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
            bool[] data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data);
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
                Status = reader["OrderStatus"].ToString();
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
