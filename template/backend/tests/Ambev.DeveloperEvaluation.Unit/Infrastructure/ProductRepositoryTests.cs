using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class ProductRepositoryTests
{
    [Fact(DisplayName = "Given new product, when CreateAsync called, then product is added and saved")]
    public async Task Given_NewProduct_When_CreateAsync_Then_ProductIsAddedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();

        var product = new Product { Id = Guid.NewGuid(), Title = "Product1" };
        await repository.CreateAsync(product);

        var saved = await context.Set<Product>().FindAsync(product.Id);
        Assert.NotNull(saved);
        Assert.Equal(product.Title, saved.Title);
    }

    [Fact(DisplayName = "Given existing product ID, when GetByIdAsync called, then product is returned")]
    public async Task Given_ExistingProductId_When_GetByIdAsync_Then_ProductIsReturned()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();

        var product = new Product { Id = Guid.NewGuid(), Title = "Product1" };
        await TestHelper.SeedDataAsync(context, [product]);

        var result = await repository.GetByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal(product.Title, result.Title);
    }

    [Fact(DisplayName = "Given product, when UpdateAsync called, then product is updated and saved")]
    public async Task Given_Product_When_UpdateAsync_Then_ProductIsUpdatedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();

        var product = new Product { Id = Guid.NewGuid(), Title = "OldTitle" };
        await TestHelper.SeedDataAsync(context, [product]);

        product.Title = "NewTitle";

        var updated = await repository.UpdateAsync(product);

        var saved = await context.Set<Product>().FindAsync(product.Id);

        Assert.Equal(product.Title, saved!.Title);
        Assert.Equal(product, updated);
    }
}