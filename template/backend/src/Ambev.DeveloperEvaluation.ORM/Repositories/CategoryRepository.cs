using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CategoryRepository(DefaultContext context) : Repository<Category>(context), ICategoryRepository
{
    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(c => c.Name == name, cancellationToken);
    }

    public Task<bool> ExistsByNameExceptIdAsync(string name, Guid id, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(c => c.Name == name && c.Id != id, cancellationToken);
    }
    
    public async Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Select(p => p.Name)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
    }
    
    public Task<Category> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        return DbSet.FirstAsync(c => c.Name == categoryName, cancellationToken);
    }
}