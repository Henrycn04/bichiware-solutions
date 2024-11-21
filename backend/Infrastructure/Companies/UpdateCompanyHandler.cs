using backend.Infrastructure;
using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class UpdateCompanyHandler : IUpdateCompanyHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        private DatabaseQuery databaseQuery;
        private readonly IUpdateProductHandler productHandler;
        private readonly IOrdersHandler ordersHandler;

        public UpdateCompanyHandler(
            IUpdateProductHandler productHandler,
            IOrdersHandler ordersHandler)
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
            
            databaseQuery = new DatabaseQuery();
            this.productHandler = productHandler;
            this.ordersHandler = ordersHandler;
        }

        public void modifyCompanyData(CompanyProfileModel newData)
        {
            SqlCommand commandForQuery = new SqlCommand("UpdateCompanyData", _connection);
            commandForQuery.CommandType = CommandType.StoredProcedure;
            commandForQuery.Parameters.Add(new SqlParameter("@ID", newData.Id));
            commandForQuery.Parameters.Add(new SqlParameter("@CompanyName", newData.Name));
            commandForQuery.Parameters.Add(new SqlParameter("@Email", newData.Email));
            int phoneNumber = Convert.ToInt32(newData.PhoneNumber);
            int legalId = Convert.ToInt32(newData.LegalId);
            commandForQuery.Parameters.Add(new SqlParameter("@PhoneNumber", phoneNumber));
            commandForQuery.Parameters.Add(new SqlParameter("@LegalID", legalId));
            _connection.Open();
            commandForQuery.ExecuteNonQuery();
            _connection.Close();
        }

        public bool DeleteCompany(int companyId)
        {
            Console.WriteLine("Company was successfully deleted");
            if (this.HasOrders(companyId))
            {
                return this.CompanySoftDelete(companyId);
            }
            else
            {
                return this.CompanyHardDelete(companyId);
            }
        }

        public bool CheckCompanyExistence(CompaniesIDModel company)
        {
            string request = @" select CompanyName from Company where CompanyID = @companyId ";
            SqlCommand cmd = new SqlCommand(request, databaseQuery.GetConnection());
            cmd.Parameters.AddWithValue("@companyId", company.CompanyID);
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            var companyName = Convert.ToString(result.Rows[0]["CompanyName"]);
            return company.CompanyName == companyName;
        }

        public bool IsHeadquarters(CompaniesIDModel company)
        {
            string request = @"if exists (select CompanyID from Company where CompanyID = @companyId AND CompanyName = 'Bichiware Solutions')
	                               select 1 as result
                               else
	                               select 0 as result";
            SqlCommand cmd = new SqlCommand(request, databaseQuery.GetConnection());
            cmd.Parameters.AddWithValue("@companyId", company.CompanyID);
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            return Convert.ToBoolean(result.Rows[0]["result"]);         
        }

        public bool HasPendingOrders(CompaniesIDModel company)
        {
            SqlCommand cmd = new SqlCommand("GetPendingOrdersOfCompany", databaseQuery.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", company.CompanyID);
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            return result.Rows.Count > 0;
        }

        private bool HasOrders(int companyId)
        {
            SqlCommand cmd = new SqlCommand("GetOrdersOfCompany", databaseQuery.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", companyId);
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            return result.Rows.Count > 0;
        }

        public List<int> GetCompanyProducts(int companyId)
        {
            SqlCommand cmd = new SqlCommand("GetCompanyProducts", databaseQuery.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", companyId);
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            List<int> companyProductsIds = new List<int>();
            foreach(DataRow row in result.Rows)
            {
                companyProductsIds.Add(Convert.ToInt32(row["ProductID"]));
            }
            return companyProductsIds;
        }

        public void DeleteCompanyProducts(int companyId)
        {
            List<int> listOfCompanyProductsIds = this.GetCompanyProducts(companyId);
            foreach (int productId in listOfCompanyProductsIds)
            {
                if (productId % 2 == 0 /*even => perishable. odd => non-perishable*/)
                {
                    if (this.ordersHandler.PerishableHasRelatedOrders(productId))
                    {
                        this.productHandler.LogicPerishableProductDelete(productId);
                    }
                    else
                    {
                        this.productHandler.PerishableProductDelete(productId);
                    }
                }
                else
                {
                    if (this.ordersHandler.NonPerishableHasRelatedOrders(productId))
                    {
                        this.productHandler.LogicNonPerishableProductDelete(productId);
                    }
                    else
                    {
                        this.productHandler.NonPerishableProductDelete(productId);
                    }
                }
            }
        }

        public bool CompanyHardDelete(int companyId)
        {
            SqlCommand cmd = new SqlCommand("CompanyHardDelete", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", companyId);

            _connection.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            _connection.Close();

            return rowsAffected > 0;
        }

        public bool CompanySoftDelete(int companyId)
        {
            SqlCommand cmd = new SqlCommand("CompanySoftDelete", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", companyId);

            _connection.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            _connection.Close();

            return rowsAffected > 0;
        }
    }
}
