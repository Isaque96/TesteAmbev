using Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateCartHandlerTests
{
    private static (DefaultContext context, UpdateCartHandler handler, UpdateCartCommandValidator validator) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();
        var mapper = Helper.CreateMapper();
        var validator = new UpdateCartCommandValidator();
        var handler = new UpdateCartHandler(repository, mapper);
        return (context, handler, validator);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then cart is updated")]
    public async Task Given_ValidCommand_When_Handle_Then_CartIsUpdated()
    {
        var (context, handler, validator) = CreateHandler();

        var user = UserTestData.GenerateValidUser();
        var category = new Category
        {
            Name = "Teste",
            Description = "TESTE",
            Id = Guid.NewGuid()
        };
        var product = new Product
        {
            Description = "Produto",
            Id = Guid.NewGuid(),
            CategoryId =  category.Id,
            Image = "test.jpg",
            Price = 123.45M,
            Rating = new Rating
            {
                Rate = 10,
                Count = 1
            },
            Title = "PRODUTO"
        };
        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId =  user.Id,
            Date =  DateTime.Now
        };
        cart.AddProduct(product.Id, 1);
        await TestHelper.SeedDataAsync(context, [user]);
        await TestHelper.SeedDataAsync(context, [category]);
        await TestHelper.SeedDataAsync(context, [product]);
        await TestHelper.SeedDataAsync(context, [cart]);

        var command = new UpdateCartCommand
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Date = cart.Date.AddDays(1),
            Products = [
                new UpdateCartProductItem
                {
                    ProductId = cart.CartItems.First().ProductId,
                    Quantity = cart.CartItems.First().Quantity
                }
            ]
        };

        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        var updatedCart = await context.Set<Cart>().FindAsync(cart.Id);
        Assert.NotNull(updatedCart);
        Assert.Equal(command.UserId, updatedCart.UserId);
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation fails")]
    public async Task Given_InvalidCommand_When_Handle_Then_ValidationFails()
    {
        var (_, _, validator) = CreateHandler();

        var command = new UpdateCartCommand
        {
            Id = Guid.Empty,
            UserId = Guid.Empty
        };

        var validationResult = await validator.ValidateAsync(command);
        Assert.False(validationResult.IsValid);
    }
}