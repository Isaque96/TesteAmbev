using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository(DefaultContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<(IEnumerable<Product>, int Count)> GetProductsByCategoryAsync(
        string categoryName,
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.Where(c => c.Category.Name == categoryName)
            .Include(p => p.Category)
            .AsQueryable();

        // Ordenação reaproveitando a lógica da listagem geral
        query = ApplyOrdering(query, order);

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return  (items, totalItems);
    }
    
    public async Task<IEnumerable<Product>> GetByCategoryAsync(
        string category,
        int page,
        int size,
        string? order,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable().Where(p => p.Category.Name == category);

        query = ApplyOrdering(query, order);

        return await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product> Items, int Count)> GetProductsPaginatedAsync(
        int page,
        int size,
        string? order,
        string? title,
        string? category,
        decimal? maxPrice,
        decimal? minPrice,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        // Filtro por title (suportando prefix/sufix via "*")
        if (!string.IsNullOrWhiteSpace(title))
        {
            if (title.StartsWith('*') && title.EndsWith('*'))
            {
                var value = title.Trim('*');
                query = query.Where(p => EF.Functions.ILike(p.Title, $"%{value}%"));
            }
            else if (title.StartsWith('*'))
            {
                var value = title.TrimStart('*');
                query = query.Where(p => EF.Functions.ILike(p.Title, $"%{value}"));
            }
            else if (title.EndsWith('*'))
            {
                var value = title.TrimEnd('*');
                query = query.Where(p => EF.Functions.ILike(p.Title, $"{value}%"));
            }
            else
            {
                query = query.Where(p => p.Title == title);
            }
        }

        // Filtro por categoria
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category.Name == category);
        }

        // Filtro por faixa de preço (_minPrice/_maxPrice)
        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        // Total antes da paginação
        var totalItems = await query.CountAsync(cancellationToken);

        // Ordenação
        query = ApplyOrdering(query, order);

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return  (items, totalItems);
    }
}