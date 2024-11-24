using Org.BouncyCastle.Utilities.Zlib;
using System.Collections.Generic;

namespace backend.Domain
{
    public class ClientReportRequestModel
    {
        public int UserID { get; set; }
        public int RequestType { get; set; }
        public string CreationStartDate { get; set; }
        public string CreationEndDate { get; set; }
        public string SentStartDate { get; set; }
        public string SentEndDate { get; set; }
        public string DeliveredStartDate { get; set; }
        public string DeliveredEndDate { get; set; }
        public string CancelledStartDate { get; set; }
        public string CancelledEndDate { get; set; }
        public int CancelledBy { get; set; }
        public double minShippingCost { get; set; }
        public double maxShippingCost { get; set; }
        public double minProductCost { get; set; }
        public double maxProductCost { get; set; }
        public double minTotalCost { get; set; }
        public double maxTotalCost { get; set; } 
        public int minQuantity { get; set; }
        public int maxQuantity { get; set; }
        public int orderID { get; set; }
        public string CompanyName { get; set; }
    }
}