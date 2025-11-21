using Ambev.DeveloperEvaluation.Application.Categories.ListCategories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListCategoriesHandlerTests
{
    private static (DefaultContext context, ListCategoriesHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();
        var handler = new ListCategoriesHandler(repository);
        return (context, handler);
    }

    [Fact(DisplayName = "Given categories exist, when Handle called, then all category names are returned")]
    public async Task Given_CategoriesExist_When_Handle_Then_ReturnsNames()
    {
        var (context, handler) = CreateHandler();

        context.Set<Category>().AddRange(
            new Category
            {
                Name = "Books",
                Description = "Desc"
            },
            new Category
            {
                Name = "Movies",
                Description = "Desc"
            });
        await context.SaveChangesAsync();

        var query = new ListCategoriesQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        var list = result.ToList();
        Assert.Contains("Books", list);
        Assert.Contains("Movies", list);
    }

    [Fact(DisplayName = "Given no categories, when Handle called, then returns empty list")]
    public async Task Given_NoCategories_When_Handle_Then_ReturnsEmpty()
    {
        var (_, handler) = CreateHandler();

        var query = new ListCategoriesQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}