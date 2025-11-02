using Catalog.Data;
using Catalog.Products.Dtos;
using Shared.CQRS;

namespace Catalog.Products.Features.UpdateProduct
{

    public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {

        private readonly CatalogDbContext _catalogDbContext;

        public UpdateProductCommandHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }


        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            // FindAsync is a better approach since it is optimized. This method is optimzed to look up a single entity
            // You cannot call related data, then we need FirstOrDefault
            var product = await _catalogDbContext.Products.FindAsync(request.Product.Id, cancellationToken);

            if (product is null)
            {
                throw new Exception($"Product not  found: {request.Product.Id}");
            }

            UpdateProductWithNewValues(product, request.Product);

            _catalogDbContext.Products.Update(product);
            await _catalogDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }

        private void UpdateProductWithNewValues(Product product, ProductDto dto)
        {
            product.Update(dto.Name, dto.Category, dto.Description, dto.ImageFile, dto.Price);
        }
    }
}
