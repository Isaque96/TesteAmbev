using Ambev.DeveloperEvaluation.Application.Categories.GetCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetCategoryHandlerTests
{
    private static (DefaultContext context, GetCategoryHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new GetCategoryHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given existing category, when Handle called, then category is returned")]
    public async Task Given_ExistingCategory_When_Handle_Then_ReturnsCategory()
    {
        var (context, handler) = CreateHandler();

        var category = new Category
        {
            Name = "Existing",
            Description = "Desc"
        };
        context.Set<Category>().Add(category);
        await context.SaveChangesAsync();

        var query = new GetCategoryQuery { Id = category.Id };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal(category.Name, result.Name);
        Assert.Equal(category.Description, result.Description);
    }

    [Fact(DisplayName = "Given not existing category, when Handle called, then key not found exception")]
    public async Task Given_NotExistingCategory_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var query = new GetCategoryQuery { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
    }
}