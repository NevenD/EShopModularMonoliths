using Shared.DDD;

namespace Basket.Basket.Modules
{
    public class ShoppingCartItem : Entity<Guid>
    {
        public Guid ShoppingCartId { get; private set; } = default!;
        public Guid ProductId { get; private set; } = default!;
        public int Quantity { get; internal set; } = default;
        public string Color { get; private set; } = string.Empty;


        // will come from catalog module
        public decimal Price { get; private set; } = default;
        public string ProductName { get; private set; } = string.Empty;


        internal ShoppingCartItem(Guid shoppingCartId,
                                  Guid productId,
                                  int quantity,
                                  string color,
                                  decimal price,
                                  string productName)
        {
            ShoppingCartId = shoppingCartId;
            ProductId = productId;
            Quantity = quantity;
            Color = color;
            Price = price;
            ProductName = productName;
        }
    }
}
