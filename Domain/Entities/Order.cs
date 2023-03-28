namespace Domain.Entities
{
    public class Order
    {
        public Order(OrderStatus status, Cart cart)
        {
            Id = Guid.NewGuid().ToString();
            Status = status;
            TotalCost = cart.CalculateTotalCost();
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem(item.ProductId, item.Quantity, item.Price);
                _orderItems.Add(orderItem);
            }
        }

        public string Id { get; private set; } 
        public decimal TotalCost { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void CancelOrder()
        {
            Status = OrderStatus.Canceled;
        }
    }
}
