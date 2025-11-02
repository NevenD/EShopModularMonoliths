using Catalog.Data;
using Catalog.Products.Dtos;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;


    public record CreateProductResult(Guid Id);

    internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public CreateProductCommandHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = CreateNewProduct(command.Product);
            _catalogDbContext.Products.Add(product);
            await _catalogDbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }

        private Product CreateNewProduct(ProductDto productDto)
        {
            var product = Product.Create(

                Guid.NewGuid(),
                productDto.Name,
                productDto.Category,
                productDto.Description,
                productDto.ImageFile,
                productDto.Price
                );

            return product;
        }
    }

}
