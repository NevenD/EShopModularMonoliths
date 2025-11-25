using Basket.Basket.Dtos;
using Basket.Basket.Features.CreateBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketResponse(ShoppingCartDto ShoppingCart);
    public sealed class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
             .Produces<CreateBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get Basket")
             .WithDescription("Get Basket");
        }
    }
}
