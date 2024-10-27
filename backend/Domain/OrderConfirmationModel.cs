namespace backend.Models
{
    public class OrderConfirmationModel
    {
        public int UserID { get; set; }
        public float tax { get; set; }
        public float delivery { get; set; }
        public float productCost { get; set; }
    }
}
