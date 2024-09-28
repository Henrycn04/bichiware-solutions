using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Services
{
    public class DeliveryCleanUpService : IHostedService, IDisposable
    {   
        private Timer _timer; // mantain the time
        private readonly ILogger<DeliveryCleanUpService> _logger; // write log messages 
        private readonly string _connectionString; // context of database

        public DeliveryCleanUpService(ILogger<DeliveryCleanUpService> logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delivery Cleanup Service is starting.");
            // Cleanup evry day at an specific hour
            _timer = new Timer(CleanupExpiredDeliveries, null, GetNextRunTime(), TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private TimeSpan GetNextRunTime()
        {
            var now = DateTime.Now; // actual time
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 00, 00, 0); // 00:00 AM
            if (now > nextRun) 
            {
                nextRun = nextRun.AddDays(1); // Plan it to the next day if it was already cleaned
            }
            return nextRun - now;// time until clean again
        }

        private void CleanupExpiredDeliveries(object state)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    // SQL command to delete expired deliveries
                    var command = new SqlCommand("DELETE FROM Delivery WHERE ExpirationDate < @currentDate", connection);
                    command.Parameters.AddWithValue("@currentDate", DateTime.Now);

                    var rowsAffected = command.ExecuteNonQuery();
                    _logger.LogInformation($"{rowsAffected} expired deliveries cleaned up successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cleaning up expired deliveries.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {   // when the service is stopped the cleanup method and timer stop working
            _logger.LogInformation("Delivery Cleanup Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {   // free resources
            _timer?.Dispose();
        }
    }
}