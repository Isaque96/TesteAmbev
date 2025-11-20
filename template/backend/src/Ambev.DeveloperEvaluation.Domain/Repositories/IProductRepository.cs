using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<(IEnumerable<Product>, int Count)> GetProductsByCategoryAsync(
        string categoryName,
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    );
    
    Task<IEnumerable<Product>> GetByCategoryAsync(
        string category,
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    );

    Task<(IEnumerable<Product> Items, int Count)> GetProductsPaginatedAsync(
        int page,
        int size,
        string? order,
        string? title,
        string? category,
        decimal? maxPrice,
        decimal? minPrice,
        CancellationToken cancellationToken = default
    );
}