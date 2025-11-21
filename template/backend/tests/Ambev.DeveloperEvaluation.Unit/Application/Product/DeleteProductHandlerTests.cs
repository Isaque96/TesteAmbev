using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using NSubstitute;
using Rebus.Bus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Product;

public class DeleteProductHandlerTests
{
    private static (DefaultContext context, DeleteProductHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var bus = Substitute.For<IBus>();
        var handler = new DeleteProductHandler(repository, bus);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing product, when Handle called, then product is deleted")]
    public async Task Given_ExistingProduct_When_Handle_Then_ProductIsDeleted()
    {
        var (context, handler) = CreateHandler();

        var product = new DeveloperEvaluation.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Title = "To delete",
            Price = 10,
            Description = "Desc",
            Category = new DeveloperEvaluation.Domain.Entities.Category { Name = "category", Description =  "category description" },
            Image = "img",
            Rating = new Rating { Rate = 3, Count = 1 }
        };

        context.Set<DeveloperEvaluation.Domain.Entities.Product>().Add(product);
        await context.SaveChangesAsync();

        var command = new DeleteProductCommand { Id = product.Id };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Deleted);

        var deleted = await context.Set<DeveloperEvaluation.Domain.Entities.Product>().FindAsync(product.Id);
        Assert.Null(deleted);
    }

    [Fact(DisplayName = "Given not existing product, when Handle called, then key not found exception")]
    public async Task Given_NotExistingProduct_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new DeleteProductCommand { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given empty guid, when Handle called, then key not found exception")]
    public async Task Given_EmptyGuid_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new DeleteProductCommand { Id = Guid.Empty };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}