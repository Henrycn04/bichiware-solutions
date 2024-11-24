using System.ComponentModel.Design;
using System.Data.SqlClient;
using backend.Models;
namespace backend.Handlers
{
    public class DeleteFromCompanyHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public DeleteFromCompanyHandler() {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("SetType");
            _connection = new SqlConnection(_routeConnection);
        }

        public void deleteUserCompany(DeleteFromCompanyModel data)
        {
            var query =
                @"DELETE FROM [dbo].[CompanyMembers]
                OUTPUT DELETED.CompanyID				
                WHERE [UserID] = @UID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UID", data.userID);
            
            _connection.Open();
            List<int> deletedIDs = new List<int>();

            using (var reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    deletedIDs.Add(reader.GetInt32(0));  // Assuming CompanyID is an integer
                }
            }
            _connection.Close();

            deleteCompany(deletedIDs);
            deleteProfileCompany(data);
        }

        private void deleteCompany(List<int> deletedIDs)
        {
            var deleter =
                @"DELETE FROM [dbo].[Company]				
                WHERE [CompanyID] = @CID AND Deleted = 0";
            var getter = 
                @"Select [UserID] 
                FROM [dbo].[CompanyMembers]				
                WHERE [CompanyID] = @CID";
            var commandDeleter = new SqlCommand(deleter,_connection);
            var commandGetter = new SqlCommand(getter, _connection);
            bool empty = true;
            while (deletedIDs.Count > 0)
            {
                commandGetter.Parameters.Clear();
                commandGetter.Parameters.AddWithValue("@CID", deletedIDs[0]);
                empty = true;

                _connection.Open();

                using (var reader = commandGetter.ExecuteReader())
                {
                    if (reader.Read()) empty = false;
                }

                if (empty) {
                    commandDeleter.Parameters.Clear();
                    commandDeleter.Parameters.AddWithValue("@CID", deletedIDs[0]);
                    commandDeleter.ExecuteScalar();
                }

                _connection.Close();
                deletedIDs.RemoveAt(0);
            }
        }

        private void deleteProfileCompany(DeleteFromCompanyModel data)
        {
            var query =
                @"DELETE FROM [dbo].[CompanyProfiles]				
                WHERE [UserID] = @UID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UID", data.userID);

            _connection.Open();
            commandForQuery.ExecuteScalar();
            _connection.Close();
        }

    }
}
