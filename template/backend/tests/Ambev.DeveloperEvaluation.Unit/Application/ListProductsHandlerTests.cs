using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListProductsHandlerTests
{
    private static (DefaultContext context, ListProductsHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new ListProductsHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given products exist, when Handle called, then paged result is returned")]
    public async Task Given_ProductsExist_When_Handle_Then_PagedResultReturned()
    {
        var (context, handler) = CreateHandler();

        for (int i = 0; i < 25; i++)
        {
            context.Set<Product>().Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = $"Product {i:D2}",
                Price = 10 + i,
                Description = "Desc",
                Category = i % 2 == 0 ? 
                    new Category
                    {
                        Name = "electronics",
                        Description = "Electronics"
                    } : new Category
                    {
                        Name = "books",
                        Description = "Books"
                    },
                Image = "img",
                Rating = new Rating { Rate = 4, Count = 10 }
            });
        }

        await context.SaveChangesAsync();

        var query = new ListProductsQuery
        {
            Page = 2,
            Size = 10,
            Order = "title",
            Title = null,
            Category = null,
            MaxPrice = null,
            MinPrice = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(25, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.CurrentPage);

        var data = result.Data.ToList();
        Assert.Equal(10, data.Count);
    }

    [Fact(DisplayName = "Given no products, when Handle called, then empty result is returned")]
    public async Task Given_NoProducts_When_Handle_Then_EmptyResult()
    {
        var (_, handler) = CreateHandler();

        var query = new ListProductsQuery
        {
            Page = 1,
            Size = 10,
            Order = "title",
            Title = null,
            Category = null,
            MaxPrice = null,
            MinPrice = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
        Assert.Empty(result.Data);
    }

    [Fact(DisplayName = "Given filters, when Handle called, then only filtered products are returned")]
    public async Task Given_Filters_When_Handle_Then_FilteredProductsReturned()
    {
        var (context, handler) = CreateHandler();

        context.Set<Product>().AddRange(
            new Product
            {
                Id = Guid.NewGuid(),
                Title = "Laptop",
                Price = 5000,
                Description = "High-end",
                Category = new Category {Name = "electronics", Description = "Electronics"},
                Image = "img",
                Rating = new Rating { Rate = 4.5m, Count = 100 }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Title = "Book C#",
                Price = 150,
                Description = "Programming",
                Category = new Category {Name = "books", Description = "Books"},
                Image = "img",
                Rating = new Rating { Rate = 4.0m, Count = 10 }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Title = "Mouse",
                Price = 100,
                Description = "Peripheral",
                Category = new Category {Name = "electronics", Description = "Electronics"},
                Image = "img",
                Rating = new Rating { Rate = 3.8m, Count = 5 }
            }
        );

        await context.SaveChangesAsync();

        var query = new ListProductsQuery
        {
            Page = 1,
            Size = 10,
            Order = "price",
            Title = "Book C#",
            Category = "books",
            MinPrice = 100,
            MaxPrice = 200
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalItems);
        var data = result.Data.ToList();
        Assert.Single(data);
        Assert.Equal("Book C#",
            data[0].Title);
    }

    [Fact(DisplayName = "Given exact page size products, when Handle called, then total pages is correct")]
    public async Task Given_ExactPageSizeProducts_When_Handle_Then_TotalPagesCorrect()
    {
        var (context, handler) = CreateHandler();

        for (int i = 0; i < 20; i++)
        {
            context.Set<Product>().Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = $"Product {i}",
                Price = 10 + i,
                Description = "Desc",
                Category = new Category{Name = "category", Description = "category"},
                Image = "img",
                Rating = new Rating { Rate = 4, Count = 10 }
            });
        }

        await context.SaveChangesAsync();

        var query = new ListProductsQuery
        {
            Page = 1,
            Size = 10,
            Order = "title",
            Title = null,
            Category = null,
            MaxPrice = null,
            MinPrice = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(20, result.TotalItems);
        Assert.Equal(2, result.TotalPages);
    }
}