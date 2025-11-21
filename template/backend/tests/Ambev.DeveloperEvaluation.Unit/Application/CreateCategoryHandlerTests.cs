using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateCategoryHandlerTests
{
    private static (DefaultContext context, CreateCategoryHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new CreateCategoryHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then category is created")]
    public async Task Given_ValidCommand_When_Handle_Then_CategoryIsCreated()
    {
        var (context, handler) = CreateHandler();

        var command = new CreateCategoryCommand
        {
            Name = "Electronics",
            Description = "Electronic products"
        };

        var validator = new CreateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        var savedCategory = await context.Set<Category>().FindAsync(result.Id);
        Assert.NotNull(savedCategory);
        Assert.Equal(command.Name, savedCategory.Name);
        Assert.Equal(command.Description, savedCategory.Description);
        Assert.NotEqual(default, savedCategory.CreatedAt);
    }

    [Fact(DisplayName = "Given duplicate name, when Handle called, then validation exception is thrown")]
    public async Task Given_DuplicateName_When_Handle_Then_ThrowsValidationException()
    {
        var (context, handler) = CreateHandler();

        var existing = new Category
        {
            Name = "Books",
            Description = "All kinds of books"
        };
        context.Set<Category>().Add(existing);
        await context.SaveChangesAsync();

        var command = new CreateCategoryCommand
        {
            Name = "Books",
            Description = "Another description"
        };

        var validator = new CreateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid); // validator não conhece duplicidade

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation exception is thrown")]
    public async Task Given_InvalidCommand_When_Handle_Then_ThrowsValidationException()
    {
        var (_, handler) = CreateHandler();

        var command = new CreateCategoryCommand
        {
            Name = string.Empty, // inválido
            Description = new string('x', 501) // maior que 500
        };

        var validator = new CreateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.False(validationResult.IsValid);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}