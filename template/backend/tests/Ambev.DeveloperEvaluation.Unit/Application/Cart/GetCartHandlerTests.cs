using Ambev.DeveloperEvaluation.Application.Cart.GetCart;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class GetCartHandleTests
{
    private static (DefaultContext context, GetCartHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new GetCartHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing cart ID, when Handle called, then cart is returned")]
    public async Task Given_ExistingCartId_When_Handle_Then_CartIsReturned()
    {
        var (context, handler) = CreateHandler();

        var cart = new DeveloperEvaluation.Domain.Entities.Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        await TestHelper.SeedDataAsync(context, [cart]);

        var result = await handler.Handle(new GetCartQuery { Id = cart.Id }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(cart.Id, result.Id);
    }

    [Fact(DisplayName = "Given non-existing cart ID, when Handle called, then null is returned")]
    public async Task Given_NonExistingCartId_When_Handle_Then_NullReturned()
    {
        var (_, handler) = CreateHandler();
        var id = Guid.NewGuid();

        var result = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(new GetCartQuery { Id = id }, CancellationToken.None));

        Assert.NotEmpty(result.Message);
        Assert.Contains(id.ToString(), result.Message);
    }
}