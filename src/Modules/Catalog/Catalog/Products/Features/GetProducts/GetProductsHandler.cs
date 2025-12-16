using Catalog.Contracts.Dtos;
using Catalog.Data;
using Mapster;
using Shared.Contracts.CQRS;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductDto> Products);

    internal sealed class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly CatalogDbContext _catalogDbContext;

        public GetProductsHandler(CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await _catalogDbContext.Products.LongCountAsync(cancellationToken);

            var products = await _catalogDbContext.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(
                new PaginatedResult<ProductDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    productDtos
                    )
                );
        }
    }
}
