using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class DatabaseQuery
    {
        private SqlConnection connection;
        private string connectionPath;

        public DatabaseQuery()
        {
            var builder = WebApplication.CreateBuilder();
            this.connectionPath = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            if (this.connectionPath == null )
            {
                throw new NullReferenceException("Connection path was not found");
            }
            this.connection = new SqlConnection(this.connectionPath);
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public DataTable ReadFromDatabase(string request)
        {
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable result = new DataTable();

            this.connection.Open();
            adapter.Fill(result);
            this.connection.Close();

            try
            {
                this.ValidateReadQuery(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public DataTable ReadFromDatabase(SqlCommand command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable result = new DataTable();

            this.connection.Open();
            adapter.Fill(result);
            this.connection.Close();

            try
            {
                this.ValidateReadQuery(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public bool WriteToDatabase(string request)
        {
            SqlCommand command = new SqlCommand(request, connection);
            this.connection.Open();
            bool success = command.ExecuteNonQuery() >= 1;
            this.connection.Close();
            return success;
        }

        private DataTable ValidateReadQuery(DataTable result)
        {
            if (result.Rows.Count <= 0)
            {
                // throw new Exception("Empty result on select query");
                return result;
            }
            return result;
        }
    }
}
