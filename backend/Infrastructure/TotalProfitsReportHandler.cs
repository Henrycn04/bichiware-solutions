using backend.Domain;
using backend.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace backend.Handlers
{
    public interface ITotalProfitsHandler
    {
        Task<List<TotalProfitsResponseModel>> GetTotalProfits(TotalProftsRequestModel request);
    }

    public class TotalProfitsHandler : ITotalProfitsHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;

        public TotalProfitsHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
        }

        public async Task<List<TotalProfitsResponseModel>> GetTotalProfits(TotalProftsRequestModel request)
        {
            var result = new List<TotalProfitsResponseModel>();

            try
            {

                using (var command = new SqlCommand("TotalProfits", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Years", JsonConvert.SerializeObject(request.Years));
                    command.Parameters.AddWithValue("@CompanyIDs", JsonConvert.SerializeObject(request.CompanyIDs));

                    await _connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var profitResponse = new TotalProfitsResponseModel
                            {
                                CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                TotalShippingCost = reader.GetDecimal(reader.GetOrdinal("TotalShippingCost")),
                                TotalOrderPrice = reader.GetDecimal(reader.GetOrdinal("TotalOrderPrice"))
                            };

                            result.Add(profitResponse);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                // Cerrar la conexión
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return result;
        }
    }
}