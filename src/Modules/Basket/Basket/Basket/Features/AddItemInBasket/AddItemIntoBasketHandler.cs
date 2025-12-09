using Basket.Basket.Dtos;
using Basket.Data.Repository;
using Catalog.Contracts.Products.Features.GetProductById;
using MediatR;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.AddItemInBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemInBasketResult>;
    public record AddItemInBasketResult(Guid Id);

    public sealed class AddItemIntoBasketHandler : ICommandHandler<AddItemIntoBasketCommand, AddItemInBasketResult>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ISender _sender;

        public AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender)
        {
            _basketRepository = basketRepository;
            _sender = sender;
        }

        public async Task<AddItemInBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasket(request.UserName, false, cancellationToken);

            var result = await _sender.Send(new GetProductByIdQuery(request.ShoppingCartItem.ProductId));

            shoppingCart.AddItem(
                request.ShoppingCartItem.ProductId,
                request.ShoppingCartItem.Quantity,
                request.ShoppingCartItem.Color,
                result.Product.Price,
                result.Product.Name);

            await _basketRepository.SaveChangesAsync(request.UserName, cancellationToken);

            return new AddItemInBasketResult(shoppingCart.Id);
        }
    }
}
