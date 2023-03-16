namespace Domain.Entities
{
    public class Order
    {
        public Order(OrderStatus status, int cartId)
        {
            Id = Guid.NewGuid();
            Status = status;
            CartId = cartId;
        }

        public Guid Id { get; private set; } 
        //public decimal TotalCost { get; private set; }
        public OrderStatus Status { get; private set; }
        public int CartId { get; private set; }

        public void CancelOrder()
        {
            Status = OrderStatus.Canceled;
        }
        //public void CalculateTotalCost()
        //{
        //    TotalCost = Cart.CalculateTotal();
        //}
    }
}
