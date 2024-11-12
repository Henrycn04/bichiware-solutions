namespace backend.Models
{
    public class DeleteCartProductModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public bool IsPerishable { get; set; }
    }

}