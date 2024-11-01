using backend.Application.Orders;
using backend.Domain;
using backend.Models;
using System.Data;
using GeoCoordinatePortable;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class ShippingCostCalculator : IShippingCostCalculator
    {
        private ShippingFee                 broadFee;
        private ShippingFee                 gamFee;
        private DatabaseQuery               databaseQuery;
        private PhysicalAddress             headquartersAddress;

        public ShippingCostCalculator()
        {
            this.databaseQuery = new DatabaseQuery();
            try
            {
                this.GetShippingFees();
                this.GetHeadquartersAddress();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void GetShippingFees()
        {
            string request = @" select KmMin, KmMax, CostNormalKg, CostExtraKg from dbo.Fee ";
            DataTable result = databaseQuery.ReadFromDatabase(request);

            foreach (DataRow row in result.Rows)
            {
                double distanceMin = Convert.ToDouble(row["KmMin"]);
                if (distanceMin < 60)
                {
                    this.gamFee = new ShippingFee
                    {
                        distanceKmMin       = distanceMin,
                        distanceKmMax       = Convert.ToDouble(row["KmMax"]),
                        costFirstKg         = Convert.ToDouble(row["CostNormalKg"]),
                        costAdditionalKg    = Convert.ToDouble(row["CostExtraKg"])
                    };
                }
                else
                {
                    this.broadFee = new ShippingFee
                    {
                        distanceKmMin       = distanceMin,
                        distanceKmMax       = Convert.ToDouble(row["KmMax"]),
                        costFirstKg         = Convert.ToDouble(row["CostNormalKg"]),
                        costAdditionalKg    = Convert.ToDouble(row["CostExtraKg"])
                    };
                }
            }
        }

        private void GetHeadquartersAddress()
        {
            string request = @" select Province, Canton, District, ExactAddress, Latitude, Longitude
                                from dbo.Address inner join dbo.CompanyAddress on dbo.CompanyAddress.AddressID = dbo.Address.AddressID
                                where dbo.CompanyAddress.CompanyID = (
                                    select CompanyID from dbo.Company
                                    where dbo.Company.CompanyName = 'BichiwareSolutions'
                                )";
            DataTable result = databaseQuery.ReadFromDatabase(request);

            if (result.Rows.Count == 1)
            {
                this.InitializeHeadquartersAddress(result.Rows[0]);
            }
            else
            {
                throw new Exception("Multiple companies named Bichiware were found, expecting only one");
            }
        }

        private void InitializeHeadquartersAddress(DataRow row)
        {
            this.headquartersAddress = new PhysicalAddress()
            {
                province                = Convert.ToString(row["Province"]),
                canton                  = Convert.ToString(row["Canton"]),
                district                = Convert.ToString(row["District"]),
                exactAddress            = Convert.ToString(row["ExactAddress"]),
                lat                     = Convert.ToDouble(row["Latitude"]),
                lon                     = Convert.ToDouble(row["Longitude"])
            };
        }

        public double CalculateShippingCost(PhysicalAddress destination, double orderKgMass)
        {
            double shippingCost = 0;
            try
            {
                if (orderKgMass <= 0 )
                {
                    throw new Exception("Mass is negative");
                }

                double kmDistance = CalculateDistance(this.headquartersAddress, destination);

                if (kmDistance < 60)
                {
                    shippingCost = gamFee.costFirstKg;
                    shippingCost += gamFee.costAdditionalKg * (Math.Ceiling(orderKgMass) - 1);
                }
                else
                {
                    shippingCost = broadFee.costFirstKg;
                    shippingCost += broadFee.costAdditionalKg * (Math.Ceiling(orderKgMass) - 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            return shippingCost;
        }

        private double CalculateDistance(PhysicalAddress origin, PhysicalAddress destination)
        {
            GeoCoordinate originCoor = new GeoCoordinate()
            {
                Latitude = origin.lat,
                Longitude = origin.lon,
            };
            GeoCoordinate destCoor = new GeoCoordinate()
            {
                Latitude = destination.lat,
                Longitude = destination.lon
            };
            // Returns in Kms. The formula for this is very complex
            return originCoor.GetDistanceTo(destCoor) / 1000;
        }

        public PhysicalAddress GetOrderDestination(int addressId)
        {
            PhysicalAddress pa;
            string request = @"select Province, Canton, District, ExactAddress, Latitude, Longitude
                               from dbo.Address where dbo.Address.AddressID = @addressId";
            SqlCommand command = new SqlCommand(request, databaseQuery.GetConnection());
            command.Parameters.AddWithValue("@addressId", addressId);

            DataTable result = this.databaseQuery.ReadFromDatabase(command);
            if (result.Rows.Count == 1)
            {
                DataRow row = result.Rows[0];
                pa = new PhysicalAddress()
                {
                    province            = Convert.ToString(row["Province"]),
                    canton              = Convert.ToString(row["Canton"]),
                    district            = Convert.ToString(row["District"]),
                    exactAddress        = Convert.ToString(row["ExactAddress"]),
                    lat                 = Convert.ToDouble(row["Latitude"]),
                    lon                 = Convert.ToDouble(row["Longitude"]),
                };
            }
            else
            {
                throw new Exception("Multiple addresses found for the requested address.");
            }
            return pa;
        }

        public double SumOrderProductsWeight(int orderId)
        {
            double sum = 0;
            string request = @" select Weight from PerishableProduct where ProductID = 
	                            (
		                            select ProductID from OrderedPerishable where OrderID = @orderId
	                            )
	                            UNION
	                            select Weight from NonPerishableProduct where ProductID =
	                            (
		                            select ProductID from OrderedNonPerishable where OrderID = @orderId
	                            ) ";
            SqlCommand command = new SqlCommand(request, databaseQuery.GetConnection());
            command.Parameters.AddWithValue("@orderId", orderId);

            DataTable result = databaseQuery.ReadFromDatabase(command);
            foreach (DataRow row in result.Rows)
            {
                sum += Convert.ToDouble(row["Weight"]);
            }
            return sum;
        }

        public List<OrderProductModel> GetOrderProducts(int orderId)
        {
            List<OrderProductModel> products = new List<OrderProductModel>();
            string request = @" select ProductName,CompanyID,CompanyName,Category,ImageURL,Price,Quantity from dbo.PerishableProduct inner join
                                (
	                                select ProductID, Quantity, OrderID from OrderedPerishable
                                )
                                as t on PerishableProduct.ProductID = t.ProductID
                                where t.OrderID = @orderId
                                UNION
                                select ProductName,CompanyID,CompanyName,Category,ImageURL,Price,Quantity from dbo.NonPerishableProduct inner join
                                (
	                                select ProductID, Quantity, OrderID from OrderedNonPerishable
                                )
                                as k on NonPerishableProduct.ProductID = k.ProductID
                                where k.OrderID = @orderId";
            SqlCommand command = new SqlCommand(request, databaseQuery.GetConnection());
            command.Parameters.AddWithValue("@orderId", orderId);

            DataTable result = databaseQuery.ReadFromDatabase(command);

            foreach (DataRow row in result.Rows)
            {
                products.Add(this.InitOrderProduct(row));
            }
            return products;
        }

        private OrderProductModel InitOrderProduct(DataRow row)
        {
            OrderProductModel model = new OrderProductModel()
            {
                Name = Convert.ToString(row["ProductName"]),
                Company = Convert.ToString(row["CompanyName"]),
                CompanyID = Convert.ToInt32(row["CompanyID"]),
                Category = Convert.ToString(row["Category"]),
                ImageURL = Convert.ToString(row["ImageURL"]),
                PriceInColones = Convert.ToDouble(row["Price"]),
                Amount = Convert.ToInt32(row["Quantity"])
            };
            return model;
        }
    }
}
