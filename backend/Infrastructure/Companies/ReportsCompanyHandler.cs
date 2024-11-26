using System.Data;
using System.Data.SqlClient;
using backend.Domain;
using backend.Models;
using MailKit.Search;

namespace backend.Infrastructure
{
    public class ReportsCompanyHandler : IReportsCompanyHandler
    {
        DatabaseQuery db;

        public ReportsCompanyHandler()
        {
            db = new DatabaseQuery();
        }

        public List<PendingOrderReport> GeneratePendingOrdersReport(FiltersCompletedOrdersModel filter)
        {
            if (filter.CompanyID != null)
            {
                return this.GetPendingOrdersSpecific(filter.CompanyID.Value);
            }
            else
            {
                return this.GetPendingOrders(filter.UserID);
            }
        }

        private List<PendingOrderReport> GetPendingOrders(int userId)
        {
            List<PendingOrderReport> report = new List<PendingOrderReport>();
            SqlCommand cmd = new SqlCommand("GetPendingReportOfAllUserCompanies", db.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId);
            DataTable result = db.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);
                double subtotal = Convert.ToDouble(row["ProductCost"]);
                double shipping = Convert.ToDouble(row["ShippingCost"]);
                report.Add(new PendingOrderReport()
                {
                    Number = orderId,
                    Companies = this.GetCompaniesNames(orderId),
                    Quantity = this.GetQuantityOfProducts(orderId),
                    CreationDate = Convert.ToString(row["CreationDate"]),
                    ShippingDate = Convert.ToString(row["DeliveryDate"]),
                    State = "Pending",
                    Subtotal = subtotal,
                    ShippingCost = shipping,
                    Total = subtotal + shipping + Convert.ToDouble(row["Tax"]) * subtotal
                });
            }
            return report;
        }

        private List<PendingOrderReport> GetPendingOrdersSpecific(int companyId)
        {
            List<PendingOrderReport> report = new List<PendingOrderReport>();
            SqlCommand cmd = new SqlCommand("GetPendingOrderReport", db.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", companyId);
            DataTable result = db.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);
                double subtotal = Convert.ToDouble(row["ProductCost"]);
                double shipping = Convert.ToDouble(row["ShippingCost"]);
                double tax = Convert.ToDouble(row["Tax"]);
                string state = "";
                switch (Convert.ToInt32(row["OrderStatus"]))
                {
                    case 1: state = "Creado"; break;
                    case 2: state = "Confirmado"; break;
                    case 4: state = "Enviado"; break;
                }

                report.Add( new PendingOrderReport()
                {
                    Number = orderId,
                    Companies = this.GetCompaniesNames(orderId),
                    Quantity = this.GetQuantityOfProducts(orderId),
                    CreationDate = Convert.ToString(row["CreationDate"]),
                    ShippingDate = Convert.ToString(row["DeliveryDate"]),
                    State = state,
                    Subtotal = subtotal,
                    ShippingCost = shipping,
                    Total = Math.Round(subtotal + shipping + tax, 2)
                });
            }
            return report;
        }

        public bool CheckUserType(int userId)
        {
            string request = @" select UserType from UserData where UserID = @userId";
            SqlCommand cmd = new SqlCommand(request, db.GetConnection());
            cmd.Parameters.AddWithValue("@userId", userId);
            DataTable result = db.ReadFromDatabase(cmd);

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["UserType"]) > 1;
            }
            return false;
        }

        private string GetCompaniesNames(int orderId)
        {
            string companiesNames = "";
            SqlCommand cmd = new SqlCommand("GetCompaniesNamesOfOrder", db.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderId);
            DataTable result = db.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                companiesNames += Convert.ToString(row["CompanyName"]) + "\n";
            }
            return companiesNames;
        }

        private int GetQuantityOfProducts(int orderId)
        {
            int quantity = 0;
            string request = @"select sum(Quantity) as Quantity from OrderedNonPerishable where OrderID = @orderId
                               union
                               select sum(Quantity) as Quantity from OrderedPerishable where OrderID = @orderId";
            SqlCommand cmd = new SqlCommand(request, db.GetConnection());
            cmd.Parameters.AddWithValue("@orderId", orderId);
            DataTable result = db.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                if (row["Quantity"] != DBNull.Value)
                {
                    quantity += Convert.ToInt32(row["Quantity"]);
                }
            }
            return quantity;
        }
    }
}
