using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using backend.Domain;

namespace backend.Infrastructure
{
    public class ClientReportHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        private string query;
        public ClientReportHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("RegisterUser");
            _connection = new SqlConnection(_routeConnection);
            query = "EXEC ClientGetOrders @OrderStatus";
        }

        public List<ClientReportResponseModel> GetCompletedReport (ClientReportRequestModel request)
        {
            var commandForQuery = new SqlCommand(this.query, _connection);
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
            var commandForQuery = new SqlCommand(this.query, _connection);
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
            string localQuery = this.query;

            var commandForQuery = new SqlCommand(localQuery, _connection);
            commandForQuery.CommandType = CommandType.StoredProcedure;
            List<ClientReportResponseModel> reportList = this.GenerateCurrentReports(commandForQuery);
            return reportList;
        }

        private (string, bool[]) GenerateString(ClientReportRequestModel request) 
        {
            string localQuery = this.query;
            bool[] used = new bool[19];
            if (request.UserID > 0) 
            {
                localQuery += ", @UID = @UIDEntry";
                used[0] = true;
            }
            if (request.CreationStartDate != "") 
            {
                localQuery += ", @CreationStartDate = @CSDEntry";
                used[1] = true;
            }
            if(request.CreationEndDate != "")
            {
                localQuery += ", @CreationEndDate = @CEDEntry";
                used[2] = true;
            }
            if (request.SentStartDate != "")
            {
                localQuery += ", @SentStartDate = @SSDEntry";
                used[3] = true;
            }
            if (request.SentEndDate != "")
            {
                localQuery += ", @SentEndDate = @SEDEntry";
                used[4] = true;
            }
            if (request.DeliveredStartDate != "")
            {
                localQuery += ", @DeliveredStartDate = @DSDEntry";
                used[5] = true;
            }
            if (request.DeliveredEndDate != "")
            {
                localQuery += ", @DeliveredEndDate = @DEDEntry";
                used[6] = true;
            }
            if (request.CancelledStartDate != "")
            {
                localQuery += ", @CancelledStartDate = @CaSDEntry";
                used[7] = true;
            }
            if (request.CancelledEndDate != "")
            {
                localQuery += ", @CancelledEndDate = @CaEDEntry";
                used[8] = true;
            }
            if (request.CancelledBy > 0)
            {
                localQuery += ", @CancelledBy = @CaByEntry";
                used[9] = true;
            }
            if (request.minShippingCost > 0)
            {
                localQuery += ", @minShippingCost = @minSCEntry";
                used[10] = true;
            }
            if (request.maxShippingCost > 0)
            {
                localQuery += ", @maxShippingCost = @maxSCEntry";
                used[11] = true;
            }
            if (request.minProductCost > 0)
            {
                localQuery += ", @minProductCost = @minPCEntry";
                used[12] = true;
            }
            if (request.maxProductCost > 0)
            {
                localQuery += ", @maxProductCost = @maxPCEntry";
                used[13] = true;
            }
            if (request.minTotalCost > 0)
            {
                localQuery += ", @minTotalCost = @minTCEntry";
                used[14] = true;
            }
            if (request.maxTotalCost > 0)
            {
                localQuery += ", @maxTotalCost = @maxTCEntry";
                used[15] = true;
            }
            if (request.minQuantity > 0)
            {
                localQuery += ", @minQuantity = @minQEntry";
                used[16] = true;
            }
            if (request.maxQuantity > 0)
            {
                localQuery += ", @maxQuantity = @maxQEntry";
                used[17] = true;
            }
            if (request.orderID > 0)
            {
                localQuery += ", @OrderID = @OIDEntry";
                used[18] = true;
            }

            return (localQuery, used);
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
