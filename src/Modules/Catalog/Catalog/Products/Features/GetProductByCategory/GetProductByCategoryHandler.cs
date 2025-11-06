using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);


    internal sealed class GetProductByCategoryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public GetProductByCategoryHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await _catalogDbContext.Products
                .AsNoTracking()
                .Where(p => p.Category.Contains(query.Category))
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductByCategoryResult(productDtos);
        }
    }
}
