using Ambev.DeveloperEvaluation.Application.Cart.DeleteCart;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class DeleteCartHandlerTests
{
    private static (DefaultContext context, DeleteCartHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();
        var handler = new DeleteCartHandler(repository);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing cart ID, when Handle called, then cart is deleted and true returned")]
    public async Task Given_ExistingCartId_When_Handle_Then_CartIsDeletedAndTrueReturned()
    {
        var (context, handler) = CreateHandler();

        var cart = new DeveloperEvaluation.Domain.Entities.Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        await TestHelper.SeedDataAsync(context, [cart]);

        var result = await handler.Handle(new DeleteCartCommand { Id = cart.Id }, CancellationToken.None);

        Assert.True(result.Deleted);
        var deleted = await context.Set<DeveloperEvaluation.Domain.Entities.Cart>().FindAsync(cart.Id);
        Assert.Null(deleted);
    }

    [Fact(DisplayName = "Given non-existing cart ID, when Handle called, then false is returned")]
    public async Task Given_NonExistingCartId_When_Handle_Then_FalseReturned()
    {
        var (_, handler) = CreateHandler();
        var id = Guid.NewGuid();

        var result = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(new DeleteCartCommand { Id = id }, CancellationToken.None)
        );

        Assert.NotEmpty(result.Message);
        Assert.Contains(id.ToString(), result.Message);
    }
}