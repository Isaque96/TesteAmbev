using Ambev.DeveloperEvaluation.Application.Cart.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateCartHandlerTests
{
    private static (DefaultContext context, CreateCartHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new CreateCartHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then cart is created")]
    public async Task Given_ValidCommand_When_Handle_Then_CartIsCreated()
    {
        var (context, handler) = CreateHandler();

        var command = new CreateCartCommand
        {
            UserId = Guid.NewGuid(),
            Date =  DateTime.Now,
            Products = [
                new CreateCartProductItem
                {
                    ProductId =  Guid.NewGuid(),
                    Quantity = 5
                }
            ]
        };

        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        var savedCart = await context.Set<Cart>().FindAsync(result.Id);
        Assert.NotNull(savedCart);
        Assert.Equal(command.UserId, savedCart.UserId);
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation fails")]
    public async Task Given_InvalidCommand_When_Handle_Then_ValidationFails()
    {
        var command = new CreateCartCommand
        {
            UserId = Guid.Empty
        };

        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.False(validationResult.IsValid);
    }
}