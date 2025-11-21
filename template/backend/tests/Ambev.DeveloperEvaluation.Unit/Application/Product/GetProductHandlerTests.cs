using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Product;

public class GetProductHandlerTests
{
    private static (DefaultContext context, GetProductHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new GetProductHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing product, when Handle called, then product is returned")]
    public async Task Given_ExistingProduct_When_Handle_Then_ProductIsReturned()
    {
        var (context, handler) = CreateHandler();

        var product = new DeveloperEvaluation.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Title = "Existing",
            Price = 100,
            Description = "Desc",
            Category = new DeveloperEvaluation.Domain.Entities.Category
            {
               Name = "category",
               Description = "desc",
            },
            Image = "img",
            Rating = new Rating { Rate = 4, Count = 20 }
        };

        context.Set<DeveloperEvaluation.Domain.Entities.Product>().Add(product);
        await context.SaveChangesAsync();

        var query = new GetProductQuery { Id = product.Id };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Title, result.Title);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.Category.Name, result.CategoryName);
        Assert.Equal(product.Image, result.Image);
        Assert.Equal(product.Rating.Rate, result.Rate);
        Assert.Equal(product.Rating.Count, result.Count);
    }

    [Fact(DisplayName = "Given not existing product, when Handle called, then key not found exception")]
    public async Task Given_NotExistingProduct_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var query = new GetProductQuery { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
    }

    [Fact(DisplayName = "Given empty guid, when Handle called, then key not found exception")]
    public async Task Given_EmptyGuid_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var query = new GetProductQuery { Id = Guid.Empty };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
    }
}
