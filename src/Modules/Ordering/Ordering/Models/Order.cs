using Ordering.Events;
using Ordering.ValueObjects;
using Shared.DDD;

namespace Ordering.Models
{
    public class Order : Aggregate<Guid>
    {

        private readonly List<OrderItem> _items = [];

        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();


        public Guid CustomerId { get; private set; }
        public string OrderName { get; private set; } = string.Empty;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);



        public static Order Create(Guid id, Guid customerId, string orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment
            };

            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }

    }
}
