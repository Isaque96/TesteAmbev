using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class CartRepositoryTests
{
    [Fact(DisplayName = "Given new cart, when CreateAsync called, then cart is added and saved")]
    public async Task Given_NewCart_When_CreateAsync_Then_CartIsAddedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();

        var cart = new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        await repository.CreateAsync(cart);

        var saved = await context.Set<Cart>().FindAsync(cart.Id);
        Assert.NotNull(saved);
        Assert.Equal(cart.UserId, saved.UserId);
    }

    [Fact(DisplayName = "Given existing cart ID, when GetByIdAsync called, then cart is returned")]
    public async Task Given_ExistingCartId_When_GetByIdAsync_Then_CartIsReturned()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();

        var cart = new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        await TestHelper.SeedDataAsync(context, [cart]);

        var result = await repository.GetByIdAsync(cart.Id);

        Assert.NotNull(result);
        Assert.Equal(cart.UserId, result.UserId);
    }

    [Fact(DisplayName = "Given cart, when UpdateAsync called, then cart is updated and saved")]
    public async Task Given_Cart_When_UpdateAsync_Then_CartIsUpdatedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CartRepository>();

        var cart = new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        await TestHelper.SeedDataAsync(context, [cart]);

        var newUserId = Guid.NewGuid();
        cart.UserId = newUserId;

        var updated = await repository.UpdateAsync(cart);

        var saved = await context.Set<Cart>().FindAsync(cart.Id);

        Assert.Equal(newUserId, saved!.UserId);
        Assert.Equal(cart, updated);
    }
}