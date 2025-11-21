using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Unit.Helpers;

public static class TestHelper
{
    public static (TContext context, TRepository repository) CreateContextAndRepository<TContext, TRepository>()
        where TContext : DbContext
        where TRepository : class
    {
        var options = new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = (TContext)Activator.CreateInstance(typeof(TContext), options)!;
        context.Database.EnsureCreated();

        var repository = (TRepository)Activator.CreateInstance(typeof(TRepository), context)!;

        return (context, repository);
    }

    public static async Task SeedDataAsync<TEntity>(DbContext context, IEnumerable<TEntity> entities) where TEntity : class
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
    }
}