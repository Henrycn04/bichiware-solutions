using backend.Models;

namespace backend.Application.Orders
{
    public interface IShippingCostCalculator
    {
        public double CalculateShippingCost(PhysicalAddress destination, double orderKgMass);
    }
}
