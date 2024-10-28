using backend.Application;
using backend.Domain;
using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class AddOrderHandler
    {
        private readonly string _connectionString;
        private ShippingCostCalculator shippingCalculator;
        private MailHandler mailHandler;
        private AdminOrderBodyBuilder adminBody;
        private CustomerOrderBodyBuilder customerBody;
        private DatabaseQuery query;

        public AddOrderHandler(IMailService service)
        {
            var builder                 = WebApplication.CreateBuilder();
            this._connectionString      = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            this.query                  = new DatabaseQuery();
            this.mailHandler            = new MailHandler(service);
            this.adminBody              = new AdminOrderBodyBuilder();
            this.customerBody           = new CustomerOrderBodyBuilder();
            this.shippingCalculator     = new ShippingCostCalculator();
        }

        public async Task<int> InsertOrder(AddOrderModel order)
        {
            string query = @"
                INSERT INTO Orders (UserID, AddressID, FeeID, Tax, ShippingCost, ProductCost, DeliveryDate)
                OUTPUT INSERTED.OrderID
                VALUES (@UserID, @AddressID, @FeeID, @Tax, @ShippingCost, @ProductCost, @DeliveryDate);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand(query, connection))
                { 
                    command.Parameters.AddWithValue("@UserID",          order.UserID);
                    command.Parameters.AddWithValue("@AddressID",       order.AddressID);
                    command.Parameters.AddWithValue("@FeeID",           order.FeeID);
                    command.Parameters.AddWithValue("@Tax",             order.Tax);
                    command.Parameters.AddWithValue("@ShippingCost",    0);
                    command.Parameters.AddWithValue("@ProductCost",     order.ProductCost);
                    command.Parameters.AddWithValue("@DeliveryDate",    order.DeliveryDate);

                    return (int) await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<bool> InsertPerishableProduct(AddPerishableProductToOrderModel product)
        {
            string query = @"
                INSERT INTO OrderedPerishable (ProductID, OrderID, BatchNumber, ProductName, Quantity, ProductPrice)
                VALUES (@ProductID, @OrderID, @BatchNumber, @ProductName, @Quantity, @ProductPrice);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@OrderID", product.OrderID);
                        command.Parameters.AddWithValue("@BatchNumber", product.BatchNumber);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar producto perecedero: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertNonPerishableProduct(AddNonPerishableProductToOrderModel product)
        {
            string query = @"
                INSERT INTO OrderedNonPerishable (ProductID, OrderID, ProductName, Quantity, ProductPrice)
                VALUES (@ProductID, @OrderID, @ProductName, @Quantity, @ProductPrice);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@OrderID", product.OrderID);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar producto no perecedero: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> HasSufficientPerishableStock(AddPerishableProductToOrderModel product)
        {
            string query = @"
                SELECT (pp.ProductionLimit - d.ReservedUnits) AS AvailableStock
                FROM Delivery d
                INNER JOIN PerishableProduct pp ON d.ProductID = pp.ProductID
                WHERE d.ProductID = @ProductID AND d.BatchNumber = @BatchNumber;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Open the connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@BatchNumber", product.BatchNumber);

                    int availableStock = (int)(await command.ExecuteScalarAsync() ?? 0);
                    return availableStock >= product.Quantity;
                }
            }
        }

        public async Task<bool> HasSufficientNonPerishableStock(AddNonPerishableProductToOrderModel product)
        {
            string query = "SELECT Stock FROM NonPerishableProduct WHERE ProductID = @ProductID;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);

                    int stock = (int)(await command.ExecuteScalarAsync() ?? 0);
                    return stock >= product.Quantity;
                }
            }
        }

        public async Task<bool> SendRealizationEmails(OrderEmailModel order)
        {
            PhysicalAddress destination         = this.shippingCalculator.GetOrderDestination(order.addressId);
            double weight                       = this.shippingCalculator.SumOrderProductsWeight(order.orderId);
            double shippingCost                 = this.shippingCalculator.CalculateShippingCost(destination, weight);
            List<OrderProductModel> products    = this.shippingCalculator.GetOrderProducts(order.orderId);
            MailMessageModel customerMail       = this.GetUserEmailData(order.userId);

            this.customerBody.SetState(OrderStates.Pending);
            this.customerBody.SetOrderDetails(products, order.tax, shippingCost);
            this.mailHandler.SetBodyBuilder(this.customerBody);
            bool customerEmailSuccess = this.mailHandler.SendMail(customerMail);
            return customerEmailSuccess && this.SendAdminEmails(order, shippingCost, products, customerMail.ReceiverMailName);
        }

        private MailMessageModel GetUserEmailData(int userId)
        {
            MailMessageModel mail;
            string request = @"SELECT ProfileName,Email FROM dbo.Profile WHERE UserID = @userId ";

            SqlCommand cmd = new SqlCommand(request, this.query.GetConnection());
            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = this.query.ReadFromDatabase(cmd);

            if (result.Rows.Count == 1)
            {
                mail = new MailMessageModel()
                {
                    ReceiverMailAddress = Convert.ToString(result.Rows[0]["Email"]),
                    ReceiverMailName = Convert.ToString(result.Rows[0]["ProfileName"])
                };
            }
            else
            {
                mail = new MailMessageModel();
            }
            return mail;
        }

        private bool SendAdminEmails(OrderEmailModel order, double shippingCost, List<OrderProductModel> products, string customerName)
        {
            bool success = true;
            string request = @"select Profile.UserID as UserID from Profile inner join UserData on Profile.UserID = UserData.UserID
                                where UserData.UserType = 3";
            DataTable result = query.ReadFromDatabase(request);

            this.adminBody.SetCustomerDetails(customerName, this.GetAddressString(order.userId));
            this.adminBody.SetOrderDetails(products, order.tax, shippingCost);
            this.mailHandler.SetBodyBuilder(this.adminBody);

            foreach (DataRow row in result.Rows)
            {
                MailMessageModel mail = this.GetUserEmailData(Convert.ToInt32(row["UserID"]));
                Console.WriteLine("Hola");
                if (this.mailHandler.SendMail(mail) == false)
                {
                    throw new Exception("One email for one of the administrators was not sent properly");
                }
            }
            return success;
        }


        private string GetAddressString(int userId)
        {
            string address = "";
            string request = @" select Province, Canton, District, ExactAddress from Address inner join UserAddress
                                on Address.AddressID = UserAddress.AddressID
                                where UserAddress.UserID = @userId ";
            SqlCommand command = new SqlCommand(request, this.query.GetConnection());

            command.Parameters.AddWithValue("@userId", userId);
            DataTable result = query.ReadFromDatabase(command);

            if (result.Rows.Count == 1)
            {
                DataRow row = result.Rows[0];
                address += Convert.ToString(row["Province"]) + " ";
                address += Convert.ToString(row["Canton"]) + " ";
                address += Convert.ToString(row["District"]) + " ";
                address += Convert.ToString(row["ExactAddress"]);
            }

            return address;
        }

        public double CalculateShipping(OrderEmailModel order)
        {
            double shipping = 0;

            PhysicalAddress destination     = this.shippingCalculator.GetOrderDestination(order.addressId);
            double weight                   = this.shippingCalculator.SumOrderProductsWeight(order.orderId);
            shipping                        = this.shippingCalculator.CalculateShippingCost(destination, weight);

            return shipping;
        }
    }
}