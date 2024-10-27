namespace backend.Domain
{
    public class ShippingFee
    {
        public double distanceKmMin { get; set; }
        public double distanceKmMax { get; set; }
        public double costFirstKg { get; set; }
        public double costAdditionalKg { get; set; }
    }
}
