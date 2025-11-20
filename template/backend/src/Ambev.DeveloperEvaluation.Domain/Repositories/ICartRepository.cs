using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<(IEnumerable<Cart> Items, int Count)> GetCartPaginatedAsync(
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    );
}