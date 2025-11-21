using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class CategoryRepositoryTests
{
    [Fact(DisplayName = "Given new category, when CreateAsync called, then category is added and saved")]
    public async Task Given_NewCategory_When_CreateAsync_Then_CategoryIsAddedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();

        var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
        await repository.CreateAsync(category);

        var saved = await context.Set<Category>().FindAsync(category.Id);
        Assert.NotNull(saved);
        Assert.Equal(category.Name, saved.Name);
    }

    [Fact(DisplayName = "Given existing category ID, when GetByIdAsync called, then category is returned")]
    public async Task Given_ExistingCategoryId_When_GetByIdAsync_Then_CategoryIsReturned()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();

        var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
        await TestHelper.SeedDataAsync(context, [category]);

        var result = await repository.GetByIdAsync(category.Id);

        Assert.NotNull(result);
        Assert.Equal(category.Name, result.Name);
    }

    [Fact(DisplayName = "Given category, when UpdateAsync called, then category is updated and saved")]
    public async Task Given_Category_When_UpdateAsync_Then_CategoryIsUpdatedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, CategoryRepository>();

        var category = new Category { Id = Guid.NewGuid(), Name = "OldName" };
        await TestHelper.SeedDataAsync(context, [category]);

        category.Name = "NewName";

        var updated = await repository.UpdateAsync(category);

        var saved = await context.Set<Category>().FindAsync(category.Id);

        Assert.Equal(category.Name, saved!.Name);
        Assert.Equal(category, updated);
    }
}