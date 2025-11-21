using Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateCategoryHandlerTests
{
    private static (DefaultContext context, UpdateCategoryHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new UpdateCategoryHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command, when Handle called, then category is updated")]
    public async Task Given_ValidCommand_When_Handle_Then_CategoryIsUpdated()
    {
        var (context, handler) = CreateHandler();

        var category = new Category
        {
            Name = "Old Name",
            Description = "Old description"
        };
        context.Set<Category>().Add(category);
        await context.SaveChangesAsync();

        var command = new UpdateCategoryCommand
        {
            Id = category.Id,
            Name = "New Name",
            Description = "New description"
        };

        var validator = new UpdateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);

        var updated = await context.Set<Category>().FindAsync(category.Id);
        Assert.NotNull(updated);
        Assert.Equal(command.Name, updated.Name);
        Assert.Equal(command.Description, updated.Description);
    }

    [Fact(DisplayName = "Given non existing category, when Handle called, then key not found exception")]
    public async Task Given_NonExistingCategory_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            Name = "Any",
            Description = "Any"
        };

        var validator = new UpdateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given duplicate name for different category, when Handle called, then validation exception")]
    public async Task Given_DuplicateNameForDifferentCategory_When_Handle_Then_ThrowsValidationException()
    {
        var (context, handler) = CreateHandler();

        var existing1 = new Category
        {
            Name = "Books",
            Description = "First"
        };
        var existing2 = new Category
        {
            Name = "Movies",
            Description = "Second"
        };
        context.Set<Category>().AddRange(existing1, existing2);
        await context.SaveChangesAsync();

        var command = new UpdateCategoryCommand
        {
            Id = existing2.Id,
            Name = "Books",
            Description = "Updated desc"
        };

        var validator = new UpdateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.True(validationResult.IsValid);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation exception")]
    public async Task Given_InvalidCommand_When_Handle_Then_ThrowsValidationException()
    {
        var (context, handler) = CreateHandler();

        var category = new Category
        {
            Name = "Name",
            Description = "Desc"
        };
        context.Set<Category>().Add(category);
        await context.SaveChangesAsync();

        var command = new UpdateCategoryCommand
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = new string('x', 501)
        };

        var validator = new UpdateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command);
        Assert.False(validationResult.IsValid);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}