using Basket.Basket.Dtos;
using Basket.Basket.Features.CreateBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Basket.Features.AddItemInBasket
{
    public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);
    public record AddItemInBasketResponse(Guid Id);

    public sealed class AddItemIntoBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket{userName}/items", async ([FromRoute] string userName,
                                                          [FromBody] AddItemIntoBasketRequest request,
                                                          ISender sender) =>
            {
                var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
                var result = await sender.Send(command);

                var response = result.Adapt<AddItemInBasketResponse>();

                return Results.Created($"/basket/{response.Id}", response);
            })
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Item into Basket")
        .WithDescription("Add Item into Basket");
        }
    }
}
