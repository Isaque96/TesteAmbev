using Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListProductsByCategoryHandlerTests
{
    private static (DefaultContext context, ListProductsByCategoryHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new ListProductsByCategoryHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given products in category, when Handle called, then paged result is returned")]
    public async Task Given_ProductsInCategory_When_Handle_Then_ReturnsPagedResult()
    {
        var (context, handler) = CreateHandler();

        const string electronics = "electronics";

        for (var i = 0; i < 15; i++)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = $"Product {i}",
                Price = 10 + i,
                Description = "Desc",
                Category = new Category
                {
                    Name = electronics,
                    Description = "Brabo"
                },
                Image = "img",
                Rating = new Rating { Rate = 4.5m, Count = 10 + i }
            };
            context.Set<Product>().Add(product);
        }

        await context.SaveChangesAsync();

        var query = new ListProductsByCategoryQuery
        {
            CategoryName = electronics,
            Page = 1,
            Size = 10,
            Order = "title"
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(15, result.TotalItems);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(2, result.TotalPages); // 15 / 10 = 1.5 -> 2

        var dataList = result.Data.ToList();
        Assert.Equal(10, dataList.Count);
        Assert.All(dataList, x => Assert.Equal(electronics, x.CategoryName));
    }

    [Fact(DisplayName = "Given no products in category, when Handle called, then empty result is returned")]
    public async Task Given_NoProductsInCategory_When_Handle_Then_ReturnsEmpty()
    {
        var (_, handler) = CreateHandler();

        var query = new ListProductsByCategoryQuery
        {
            CategoryName = "none",
            Page = 1,
            Size = 10,
            Order = "title"
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
        Assert.Empty(result.Data);
    }
}