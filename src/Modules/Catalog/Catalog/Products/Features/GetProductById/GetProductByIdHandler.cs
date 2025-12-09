using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Exceptions;
using Mapster;
using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(ProductDto Product);

    internal sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public GetProductByIdHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _catalogDbContext.Products
                           .AsNoTracking()
                           .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(query.Id);
            }

            var productDto = product.Adapt<ProductDto>();

            return new GetProductByIdResult(productDto);
        }
    }
}
