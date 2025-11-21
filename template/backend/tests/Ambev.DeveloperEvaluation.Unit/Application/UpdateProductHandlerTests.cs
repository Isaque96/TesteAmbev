using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;
public class UpdateProductHandlerTests
{
    private static (DefaultContext context, UpdateProductHandler handler) CreateHandler()
    {
        var (context, productRepository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new UpdateProductHandler(productRepository, new CategoryRepository(context), mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then product is updated")]
    public async Task Given_ValidCommand_When_Handle_Then_ProductIsUpdated()
    {
        var (context, handler) = CreateHandler();
        context.Categories.Add(new Category { Name = "new-category", Description = "new-description" });

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Price = 100,
            Description = "Old Desc",
            Category = new Category{Name = "old-category", Description = "old-description"},
            Image = "old.png",
            Rating = new Rating { Rate = 3, Count = 5 }
        };

        context.Set<Product>().Add(product);
        await context.SaveChangesAsync();

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Title = "New Title",
            Price = 200,
            Description = "New Desc",
            CategoryName = "new-category",
            Image = "new.png",
            Rate = 4.5m,
            Count = 20
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);

        var updated = await context.Products
            .Include(c => c.Category)
            .FirstAsync(p => p.Id == product.Id);
        Assert.NotNull(updated);
        Assert.Equal("New Title", updated.Title);
        Assert.Equal(200, updated.Price);
        Assert.Equal("New Desc", updated.Description);
        Assert.Equal("new-category", updated.Category.Name);
        Assert.Equal("new.png", updated.Image);
        Assert.Equal(4.5m, updated.Rating.Rate);
        Assert.Equal(20, updated.Rating.Count);
    }

    [Fact(DisplayName = "Given non existing product, when Handle called, then key not found exception")]
    public async Task Given_NonExistingProduct_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Price = 100,
            Description = "Desc",
            CategoryName = "category",
            Image = "img",
            Rate = 4,
            Count = 10
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation exception")]
    public async Task Given_InvalidCommand_When_Handle_Then_ThrowsValidationException()
    {
        var (context, handler) = CreateHandler();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Valid",
            Price = 10,
            Description = "Valid",
            Category = new Category {Name = "valid-category", Description = "valid-description"},
            Image = "valid.png",
            Rating = new Rating { Rate = 3, Count = 5 }
        };

        context.Set<Product>().Add(product);
        await context.SaveChangesAsync();

        var command = new UpdateProductCommand
        {
            Id = Guid.Empty,
            Title = "",
            Price = -1,
            Description = "",
            CategoryName = "",
            Image = "",
            Rate = 10,
            Count = -1
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given rating values, when Handle called, then rating is updated correctly")]
    public async Task Given_RatingValues_When_Handle_Then_RatingUpdatedCorrectly()
    {
        var (context, handler) = CreateHandler();

        var category = new Category { Name = "category", Description = "desc-category" };
        context.Set<Category>().Add(category);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Price = 100,
            Description = "Desc",
            Category = category,
            Image = "img",
            Rating = new Rating { Rate = 1, Count = 1 }
        };

        context.Set<Product>().Add(product);
        await context.SaveChangesAsync();

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Title = "Title Updated",
            Price = 150,
            Description = "Desc Updated",
            CategoryName = "category",
            Image = "img2",
            Rate = 5,
            Count = 100
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        var updated = await context.Set<Product>().FindAsync(product.Id);
        Assert.Equal(5, updated!.Rating.Rate);
        Assert.Equal(100, updated.Rating.Count);
    }
}
