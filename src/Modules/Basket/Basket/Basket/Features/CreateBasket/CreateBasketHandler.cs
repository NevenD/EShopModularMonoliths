using Basket.Basket.Dtos;
using Basket.Basket.Modules;
using Basket.Data;
using FluentValidation;
using Shared.CQRS;

namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;
    public record CreateBasketResult(Guid Id);


    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public sealed class CreateBasketHandler : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {

        private readonly BasketDbContext _basketDbContext;

        public CreateBasketHandler(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
        {

            var shoppingCart = CreateNewBasket(command.ShoppingCart);

            _basketDbContext.ShoppingCarts.Add(shoppingCart);
            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return new CreateBasketResult(shoppingCart.Id);
        }

        private static ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCart)
        {
            var newBasket = ShoppingCart.Create(Guid.NewGuid(), shoppingCart.UserName);

            shoppingCart.Items.ForEach(item =>
            {
                newBasket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
            });

            return newBasket;
        }
    }
}
