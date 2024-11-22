namespace backend.Domain
{
    public class PendingOrderReport
    {
        public int      Number          { get; set; }
        public string   Companies       { get; set; }
        public int      Quantity        { get; set; }
        public string   CreationDate    { get; set; }
        public string   ShippingDate    { get; set; }
        public string   State           { get; set; }
        public double   Subtotal        { get; set; }
        public double   ShippingCost    { get; set; }
        public double   Total           { get; set; }
    }
}
