using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Rebus.Bus;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace Ambev.DeveloperEvaluation.Unit.Application.Product;

public class CreateProductHandlerTests
{
    private static (DefaultContext context, CreateProductHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, ProductRepository>();
        var mapper = Helper.CreateMapper();
        var bus = Substitute.For<IBus>();
        var handler = new CreateProductHandler(new CategoryRepository(context), repository, mapper, bus);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then product is created")]
    public async Task Given_ValidCommand_When_Handle_Then_ProductIsCreated()
    {
        var (context, handler) = CreateHandler();
        const string electronics = "electronics";
        context.Categories.Add(new DeveloperEvaluation.Domain.Entities.Category
        {
            Name = electronics,
            Description = "electronics description"
        });
        await context.SaveChangesAsync();

        var command = new CreateProductCommand
        {
            Title = "Laptop Gamer",
            Price = 5999.90m,
            Description = "High-end gaming laptop",
            CategoryName = electronics,
            Image = "https://example.com/laptop.png",
            Rate = 4.5m,
            Count = 10
        };

        var validator = new CreateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        var saved = await context.Products
            .Include(c => c.Category)
            .FirstAsync(p => p.Id == result.Id);
        Assert.NotNull(saved);
        Assert.Equal(command.Title, saved.Title);
        Assert.Equal(command.Price, saved.Price);
        Assert.Equal(command.Description, saved.Description);
        Assert.Equal(command.CategoryName, saved.Category.Name);
        Assert.Equal(command.Image, saved.Image);
        Assert.Equal(command.Rate, saved.Rating.Rate);
        Assert.Equal(command.Count, saved.Rating.Count);
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation exception is thrown")]
    public async Task Given_InvalidCommand_When_Handle_Then_ThrowsValidationException()
    {
        var (_, handler) = CreateHandler();

        var command = new CreateProductCommand
        {
            Title = "",             // inválido
            Price = 0,              // inválido
            Description = "",       // inválido
            CategoryName = "",          // inválido
            Image = "",             // inválido
            Rate = 6,               // inválido > 5
            Count = -1              // inválido < 0
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given boundary values in command, when Handle called, then product is created")]
    public async Task Given_BoundaryValues_When_Handle_Then_ProductIsCreated()
    {
        var (context, handler) = CreateHandler();
        var category = new string('C', 100);
        context.Categories.Add(new DeveloperEvaluation.Domain.Entities.Category
        {
            Name =  category,
            Description = "category description"
        });
        await  context.SaveChangesAsync();

        var command = new CreateProductCommand
        {
            Title = new string('A', 150),
            Price = 0.01m,
            Description = new string('D', 1000),
            CategoryName = category,
            Image = new string('I', 500),
            Rate = 0,   // limite inferior válido
            Count = 0   // limite inferior válido
        };

        var validator = new CreateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        var saved = await context.Products
            .Include(c => c.Category)
            .FirstAsync(p => p.Id == result.Id);
        Assert.NotNull(saved);
        Assert.Equal(150, saved.Title.Length);
        Assert.Equal(1000, saved.Description.Length);
        Assert.Equal(100, saved.Category.Name.Length);
        Assert.Equal(500, saved.Image.Length);
        Assert.Equal(0, saved.Rating.Rate);
        Assert.Equal(0, saved.Rating.Count);
    }
}