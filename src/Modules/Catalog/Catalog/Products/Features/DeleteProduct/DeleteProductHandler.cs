using Catalog.Data;
using Catalog.Products.Exceptions;
using Shared.Contracts.CQRS;

namespace Catalog.Contracts.Features.DeleteProduct
{

    public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    internal sealed class DeleteProductHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public DeleteProductHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }


        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _catalogDbContext.Products.FindAsync([request.ProductId], cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }

            _catalogDbContext.Products.Remove(product);
            await _catalogDbContext.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
