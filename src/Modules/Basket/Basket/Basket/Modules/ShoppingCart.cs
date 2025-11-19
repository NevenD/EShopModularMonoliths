using Shared.DDD;

namespace Basket.Basket.Modules
{
    public class ShoppingCart : Aggregate<Guid>
    {
        //Only methods inside the class can change it; external code can only read it.
        public string UserName { get; private set; } = string.Empty;

        private readonly List<ShoppingCartItem> _items = [];

        public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public static ShoppingCart Create(Guid id, string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);

            var shoppingCart = new ShoppingCart { Id = id, UserName = userName };

            return shoppingCart;
        }

        public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {

                var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
                _items.Add(newItem);
            }
        }

        public void RemoveItem(Guid productId)
        {
            var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is not null)
            {
                _items.Remove(existingItem);
            }
        }

    }
}
