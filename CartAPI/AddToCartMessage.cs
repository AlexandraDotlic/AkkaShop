namespace CartAPI
{
    public class AddToCartMessage
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}