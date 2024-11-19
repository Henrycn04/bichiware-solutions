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
        private string query;
        public ClientReportHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
            query = "EXEC ClientGetOrders @OrderStatus = @OS";
        }

        private (string, bool[]) GenerateString(ClientReportRequestModel request)
        {
            string localQuery = this.query;
            // This array is later used to dinamically add parameters to the query
            bool[] used = new bool[20];
            if (request.UserID > 0)
            {
                localQuery += ", @UID = @UIDEntry";
                used[0] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.CreationStartDate))
            {
                localQuery += ", @CreationStartDate = @CSDEntry";
                used[1] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.CreationEndDate))
            {
                localQuery += ", @CreationEndDate = @CEDEntry";
                used[2] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.SentStartDate))
            {
                localQuery += ", @SentStartDate = @SSDEntry";
                used[3] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.SentEndDate))
            {
                localQuery += ", @SentEndDate = @SEDEntry";
                used[4] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveredStartDate))
            {
                localQuery += ", @DeliveredStartDate = @DSDEntry";
                used[5] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveredEndDate))
            {
                localQuery += ", @DeliveredEndDate = @DEDEntry";
                used[6] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.CancelledStartDate))
            {
                localQuery += ", @CancelledStartDate = @CaSDEntry";
                used[7] = true;
            }
            if (!string.IsNullOrWhiteSpace(request.CancelledEndDate))
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
            if (!string.IsNullOrWhiteSpace(request.CompanyName))
            {
                localQuery += ", @CompanyName = @ComNameEntry";
                used[19] = true;
            }
            return (localQuery, used);
        }

        private SqlCommand GenerateCommand(ClientReportRequestModel request, string query, bool[] used)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@OS", request.RequestType);
            if (used[0]) command.Parameters.AddWithValue("@UID", request.UserID);
            if (used[1]) command.Parameters.AddWithValue("@CSDEntry", request.CreationStartDate);
            if (used[2]) command.Parameters.AddWithValue("@CEDEntry", request.CreationEndDate);
            if (used[3]) command.Parameters.AddWithValue("@SSDEntry", request.SentStartDate);
            if (used[4]) command.Parameters.AddWithValue("@SEDEntry", request.SentEndDate);
            if (used[5]) command.Parameters.AddWithValue("@DSDEntry", request.DeliveredStartDate);
            if (used[6]) command.Parameters.AddWithValue("@DEDEntry", request.DeliveredEndDate);
            if (used[7]) command.Parameters.AddWithValue("@CaSDEntry", request.CancelledStartDate);
            if (used[8]) command.Parameters.AddWithValue("@CaEDEntry", request.CancelledEndDate);
            if (used[9]) command.Parameters.AddWithValue("@CaByEntry", request.CancelledBy);
            if (used[10]) command.Parameters.AddWithValue("@minSCEntry", request.minShippingCost);
            if (used[11]) command.Parameters.AddWithValue("@maxSCEntry", request.maxShippingCost);
            if (used[12]) command.Parameters.AddWithValue("@minPCEntry", request.minProductCost);
            if (used[13]) command.Parameters.AddWithValue("@maxPCEntry", request.maxProductCost);
            if (used[14]) command.Parameters.AddWithValue("@minTCEntry", request.minTotalCost);
            if (used[15]) command.Parameters.AddWithValue("@maxTCEntry", request.maxTotalCost);
            if (used[16]) command.Parameters.AddWithValue("@minQEntry", request.minQuantity);
            if (used[17]) command.Parameters.AddWithValue("@maxQEntry", request.maxQuantity);
            if (used[18]) command.Parameters.AddWithValue("@OIDEntry", request.orderID);
            if (used[19]) command.Parameters.AddWithValue("@ComNameEntry", request.CompanyName); 

            return command;
        }

        public List<ClientReportResponseModel> GetCompletedReport (ClientReportRequestModel request)
        {
            (string, bool[]) data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data.Item1, data.Item2);
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
            (string, bool[]) data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data.Item1, data.Item2);
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
            (string, bool[]) data = this.GenerateString(request);
            var commandForQuery = this.GenerateCommand(request, data.Item1, data.Item2);
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
