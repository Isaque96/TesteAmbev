using Ambev.DeveloperEvaluation.Application.Cart.ListCarts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListsCartsHandlerTests
{
    private static (DefaultContext context, ListCartsHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new ListCartsHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "When Handle called, then all carts are returned")]
    public async Task When_Handle_Then_AllCartsAreReturned()
    {
        var (context, handler) = CreateHandler();

        var carts = new List<Cart>
        {
            new() { Id = Guid.NewGuid(), UserId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        await TestHelper.SeedDataAsync(context, carts);

        var result = await handler.Handle(new ListCartsQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(carts.Count, result.Data.Count());
    }
}