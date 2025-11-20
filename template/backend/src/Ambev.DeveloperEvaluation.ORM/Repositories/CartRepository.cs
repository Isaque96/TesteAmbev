using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository(DefaultContext context) : Repository<Cart>(context), ICartRepository
{
    public async Task<(IEnumerable<Cart> Items, int Count)> GetCartPaginatedAsync(
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet
            .Include(c => c.CartItems)
            .AsQueryable();

        query = ApplyOrdering(query, order);

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);       
        
        return (items, totalItems);
    }
    
    public override Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}