namespace backend.Domain
{
    public class ClientReportResponseModel
    {
        public int OrderID { get; set; }
        public string Companies { get; set; }
        public int Quantity { get; set; }
        public string CreationDate { get; set; }
        public string SentDate { get; set; }
        public string DeliveredDate { get; set; }
        public string CancelledDate { get; set; }
        public int CancelledBy { get; set; }
        public int Status { get; set; }
        public double ProductCost { get; set; }
        public double DeliveryCost { get; set; }
        public double TotalCost { get; set; }
    }
}
