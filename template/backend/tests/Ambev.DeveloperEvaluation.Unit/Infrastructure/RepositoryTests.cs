using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class RepositoryTests
{
    private static (DefaultContext context, Repository<Product> repository) CreateContextAndRepository()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco único por teste
            .Options;

        var context = new DefaultContext(options);
        context.Database.EnsureCreated();

        var repository = new Repository<Product>(context);
        return (context, repository);
    }

    // Helper para popular dados no contexto
    private static async Task SeedDataAsync(DefaultContext context, IEnumerable<Product> entities)
    {
        await context.Set<Product>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    [Fact(DisplayName = "Given new entity, when CreateAsync called, then entity is added and saved")]
    public async Task Given_NewEntity_When_CreateAsync_Then_EntityIsAddedAndSaved()
    {
        var (context, repository) = CreateContextAndRepository();

        var entity = new Product { Id = Guid.NewGuid(), Title = "Test" };

        await repository.CreateAsync(entity);

        var savedEntity = await context.Set<Product>().FindAsync(entity.Id);
        Assert.NotNull(savedEntity);
        Assert.Equal(entity.Title, savedEntity.Title);
    }

    [Fact(DisplayName = "Given existing entity ID, when GetByIdAsync called, then entity is returned")]
    public async Task Given_ExistingEntityId_When_GetByIdAsync_Then_EntityIsReturned()
    {
        var (context, repository) = CreateContextAndRepository();

        var id = Guid.NewGuid();
        var entity = new Product { Id = id, Title = "Existing" };
        await SeedDataAsync(context, [entity]);

        var result = await repository.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact(DisplayName = "Given non-existing entity ID, when GetByIdAsync called, then null is returned")]
    public async Task Given_NonExistingEntityId_When_GetByIdAsync_Then_NullIsReturned()
    {
        var (_, repository) = CreateContextAndRepository();

        var id = Guid.NewGuid();

        var result = await repository.GetByIdAsync(id);

        Assert.Null(result);
    }

    [Fact(DisplayName = "Given entities in DbSet, when GetAllAsync called, then all entities are returned")]
    public async Task Given_EntitiesInDbSet_When_GetAllAsync_Then_AllEntitiesAreReturned()
    {
        var (context, repository) = CreateContextAndRepository();

        var data = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Title = "A" },
            new() { Id = Guid.NewGuid(), Title = "B" }
        };
        await SeedDataAsync(context, data);

        var result = await repository.GetAllAsync();

        Assert.Equal(data.Count, result.Count());
    }

    [Fact(DisplayName = "Given predicate, when FindAsync called, then filtered entities are returned")]
    public async Task Given_Predicate_When_FindAsync_Then_FilteredEntitiesAreReturned()
    {
        var (context, repository) = CreateContextAndRepository();

        var data = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Title = "John" },
            new() { Id = Guid.NewGuid(), Title = "Jane" }
        };
        await SeedDataAsync(context, data);

        Expression<Func<Product, bool>> predicate = x => x.Title == "John";

        var result = (await repository.FindAsync(predicate)).ToList();

        Assert.Single(result);
        Assert.Equal("John", result[0].Title);
    }

    [Fact(DisplayName = "Given entity, when UpdateAsync called, then entity is updated and saved")]
    public async Task Given_Entity_When_UpdateAsync_Then_EntityIsUpdatedAndSaved()
    {
        var (context, repository) = CreateContextAndRepository();

        var entity = new Product { Id = Guid.NewGuid(), Title = "OldName" };
        await SeedDataAsync(context, [entity]);

        entity.Title = "NewName";

        var updatedEntity = await repository.UpdateAsync(entity);

        var savedEntity = await context.Set<Product>().FindAsync(entity.Id);

        Assert.Equal(entity.Title, savedEntity!.Title);
        Assert.Equal(entity, updatedEntity);
    }

    [Fact(DisplayName = "Given existing entity ID, when DeleteAsync called, then entity is removed and true returned")]
    public async Task Given_ExistingEntityId_When_DeleteAsync_Then_EntityIsRemovedAndTrueReturned()
    {
        var (context, repository) = CreateContextAndRepository();

        var id = Guid.NewGuid();
        var entity = new Product { Id = id, Title = "ToDelete" };
        await SeedDataAsync(context, [entity]);

        var result = await repository.DeleteAsync(id);

        var deletedEntity = await context.Set<Product>().FindAsync(id);

        Assert.True(result);
        Assert.Null(deletedEntity);
    }

    [Fact(DisplayName = "Given non-existing entity ID, when DeleteAsync called, then false is returned")]
    public async Task Given_NonExistingEntityId_When_DeleteAsync_Then_FalseIsReturned()
    {
        var (_, repository) = CreateContextAndRepository();

        var id = Guid.NewGuid();

        var result = await repository.DeleteAsync(id);

        Assert.False(result);
    }

    [Fact(DisplayName = "Given predicate, when ExistsAsync called and any entity matches, then true is returned")]
    public async Task Given_Predicate_When_ExistsAsync_And_AnyEntityMatches_Then_TrueIsReturned()
    {
        var (context, repository) = CreateContextAndRepository();

        var entity = new Product { Id = Guid.NewGuid(), Title = "Exists" };
        await SeedDataAsync(context, [entity]);

        Expression<Func<Product, bool>> predicate = x => x.Title == "Exists";

        var result = await repository.ExistsAsync(predicate);

        Assert.True(result);
    }

    [Fact(DisplayName = "Given predicate, when ExistsAsync called and no entity matches, then false is returned")]
    public async Task Given_Predicate_When_ExistsAsync_And_NoEntityMatches_Then_FalseIsReturned()
    {
        var (_, repository) = CreateContextAndRepository();

        Expression<Func<Product, bool>> predicate = x => x.Title == "NotExists";

        var result = await repository.ExistsAsync(predicate);

        Assert.False(result);
    }

    [Fact(DisplayName = "Given query and order 'Title asc', when ApplyOrdering called, then entities are ordered ascending by Title")]
    public void Given_QueryAndOrderNameAsc_When_ApplyOrdering_Then_EntitiesAreOrderedAscendingByName()
    {
        var data = new List<Product>
        {
            new() { Title = "B" },
            new() { Title = "A" }
        }.AsQueryable();

        var ordered = Repository<Product>.ApplyOrdering(data, "Title asc").ToList();

        Assert.Equal("A", ordered[0].Title);
        Assert.Equal("B", ordered[1].Title);
    }

    [Fact(DisplayName = "Given query and order 'Title desc', when ApplyOrdering called, then entities are ordered descending by Title")]
    public void Given_QueryAndOrderNameDesc_When_ApplyOrdering_Then_EntitiesAreOrderedDescendingByTitle()
    {
        var data = new List<Product>
        {
            new() { Title = "A" },
            new() { Title = "B" }
        }.AsQueryable();

        var ordered = Repository<Product>.ApplyOrdering(data, "Title desc").ToList();

        Assert.Equal("B", ordered[0].Title);
        Assert.Equal("A", ordered[1].Title);
    }

    [Fact(DisplayName = "Given query and order 'Title asc, Price desc', when ApplyOrdering called, then entities are ordered by Title ascending and Price descending")]
    public void Given_QueryAndOrderTitleAscAgeDesc_When_ApplyOrdering_Then_EntitiesAreOrderedByTitleAscAgeDesc()
    {
        var data = new List<Product>
        {
            new() { Title = "A", Price = 2 },
            new() { Title = "A", Price = 1 },
            new() { Title = "B", Price = 3 }
        }.AsQueryable();

        var ordered = Repository<Product>.ApplyOrdering(data, "Title asc, Price desc").ToList();

        Assert.Equal(2, ordered[0].Price);
        Assert.Equal(1, ordered[1].Price);
        Assert.Equal("B", ordered[2].Title);
    }

    [Fact(DisplayName = "Given query and null or empty order, when ApplyOrdering called, then original query is returned")]
    public void Given_QueryAndNullOrEmptyOrder_When_ApplyOrdering_Then_OriginalQueryIsReturned()
    {
        var data = new List<Product>
        {
            new() { Title = "A" },
            new() { Title = "B" }
        }.AsQueryable();

        var orderedNull = Repository<Product>.ApplyOrdering(data, null);
        var orderedEmpty = Repository<Product>.ApplyOrdering(data, "");

        Assert.Equal(data, orderedNull);
        Assert.Equal(data, orderedEmpty);
    }

    [Fact(DisplayName = "Given valid order string, when GetKeysForOrderBy called, then dictionary is parsed correctly")]
    public void Given_ValidOrderString_When_GetKeysForOrderBy_Then_DictionaryIsParsedCorrectly()
    {
        const string order = "name asc, age desc";

        var dict = Repository<Product>.GetKeysForOrderBy(order);

        Assert.Equal(2, dict.Count);
        Assert.Equal("asc", dict["name"]);
        Assert.Equal("desc", dict["age"]);
    }

    [Fact(DisplayName = "Given null or whitespace order string, when GetKeysForOrderBy called, then empty dictionary is returned")]
    public void Given_NullOrWhitespaceOrderString_When_GetKeysForOrderBy_Then_EmptyDictionaryIsReturned()
    {
        var dict = Repository<Product>.GetKeysForOrderBy(null);
        Assert.Empty(dict);

        dict = Repository<Product>.GetKeysForOrderBy("   ");
        Assert.Empty(dict);
    }

    [Fact(DisplayName = "Given invalid order string, when GetKeysForOrderBy called, then BadFormatException is thrown")]
    public void Given_InvalidOrderString_When_GetKeysForOrderBy_Then_BadFormatExceptionIsThrown()
    {
        const string invalidOrder = "name asc desc";

        Assert.Throws<BadFormatException>(() => Repository<Product>.GetKeysForOrderBy(invalidOrder));
    }
}