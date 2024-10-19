namespace backend.Models
{
    public class AddDeliveryModel
    {
        public int ProductID { get; set; }
        public int BatchNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ReservedUnits { get; set; }

    }
}
