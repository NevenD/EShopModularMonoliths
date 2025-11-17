using Shared.Exceptions;

namespace Catalog.Products.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }
    }
}
