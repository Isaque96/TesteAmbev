using Ambev.DeveloperEvaluation.Application.Categories.DeleteCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteCategoryHandlerTests
{
    private static (DefaultContext context, DeleteCategoryHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();
        var handler = new DeleteCategoryHandler(repository);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing category, when Handle called, then category is deleted")]
    public async Task Given_ExistingCategory_When_Handle_Then_CategoryIsDeleted()
    {
        var (context, handler) = CreateHandler();

        var category = new Category
        {
            Name = "To Delete",
            Description = "Desc"
        };
        context.Set<Category>().Add(category);
        await context.SaveChangesAsync();

        var command = new DeleteCategoryCommand { Id = category.Id };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Deleted);

        var deleted = await context.Set<Category>().FindAsync(category.Id);
        Assert.Null(deleted);
    }

    [Fact(DisplayName = "Given not existing category, when Handle called, then key not found exception")]
    public async Task Given_NotExistingCategory_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new DeleteCategoryCommand { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}