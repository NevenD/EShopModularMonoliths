using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductsResult>;

    public record GetProductsResult(IEnumerable<ProductDto> Products);

    internal sealed class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public GetProductsHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {


            var products = await _catalogDbContext.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(productDtos);
        }
    }
}
