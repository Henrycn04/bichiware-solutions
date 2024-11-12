namespace backend.Models
{
    public class UpdateCartProductModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public bool IsPerishable { get; set; }
        public int CurrentCartQuantity { get; set; }
    }

}