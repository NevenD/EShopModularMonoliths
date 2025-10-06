
namespace Catalog.Products.Models
{
    public class Product : Aggregate<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public List<string> Category { get; private set; } = [];
        public string Description { get; private set; } = string.Empty;
        public string ImageFile { get; private set; } = string.Empty;
        public decimal Price { get; private set; }

        public static Product Create(Guid id,
                                     string name,
                                     List<string> category,
                                     string description,
                                     string imageFile,
                                     decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);


            var product = new Product
            {
                Id = id,
                Category = category,
                Name = name,
                Description = description,
                ImageFile = imageFile,
                Price = price
            };

            product.AdDomainEvent(new ProductCreatedEvent(product));

            return product;
        }


        public void Update(string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            Name = name;
            Category = category;
            Description = description;
            ImageFile = imageFile;

            if (Price != price)
            {
                Price = price;
                AdDomainEvent(new ProductPriceChangedEvent(this));
            }

        }

    }
}
