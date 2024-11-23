
namespace backend.Models
{
    public class CancelledOrdersModel
    {
        public int OrderID { get; set; }
        public string? AllCompanies { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string? CancelledBy { get; set; }
        public decimal? ProductCost { get; set; }
        public decimal? ShippingCost { get; set; }
        public decimal Total { get; set; }

    }
}
