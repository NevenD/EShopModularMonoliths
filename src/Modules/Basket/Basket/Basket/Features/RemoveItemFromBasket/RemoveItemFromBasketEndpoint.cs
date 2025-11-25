using Basket.Basket.Features.CreateBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.RemoveItemFromBasket
{
    public record RemoveItemFromBasketResponse(Guid Id);
    public sealed class RemoveItemFromBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket", async ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender) =>
            {
                var command = new RemoveItemFromBasketCommand(userName, productId);

                var result = await sender.Send(command);

                var response = result.Adapt<RemoveItemFromBasketResponse>();

                return Results.Ok(response);
            })
             .Produces<CreateBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Remove item from Basket")
             .WithDescription("Remove item from Basket");
        }
    }
}
