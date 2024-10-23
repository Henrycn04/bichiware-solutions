namespace backend.Commands
{
    public class AddProductToCartCommand
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public bool IsPerishable { get; set; }

        public int CurrentStock { get; set; }
        public int CurrentCartQuantity { get; set; }

        public bool IsValid()
        {
            // Check if IDs and Quantity are positive
            if (UserID <= 0 || ProductID <= 0 || Quantity <= 0)
                return false;

            // Check if the ProductID parity matches the IsPerishable flag
            if (IsPerishable && ProductID % 2 != 0)
                return false;
            if (!IsPerishable && ProductID % 2 == 0)
                return false;

            if (!IsPerishable && Quantity + CurrentCartQuantity > CurrentStock)
                return false;

            return true;
        }
    }

}