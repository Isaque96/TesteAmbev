using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameExceptIdAsync(string name, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Category> GetCategoryByNameAsync(string categoryName, CancellationToken cancellationToken = default);
}