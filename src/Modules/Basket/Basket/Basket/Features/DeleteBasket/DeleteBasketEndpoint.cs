using Basket.Basket.Features.CreateBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);

    public sealed class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResult>();

                return Results.Ok(response);
            })
             .Produces<CreateBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Delete Basket")
             .WithDescription("Delete Basket");
        }
    }
}
