using backend.Application.Orders;
using backend.Domain;
using backend.Models;
using System.Data;
using GeoCoordinatePortable;

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
                                    where dbo.Company.CompanyName = 'Bichiware Solutions'
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
    }
}
