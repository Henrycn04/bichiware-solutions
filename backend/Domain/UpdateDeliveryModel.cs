namespace backend.Domain
{
    public class UpdateDeliveryModel
    {
        public int ProductID { get; set; }
        public int BatchNumber { get; set; }
        public int OldBatchNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
