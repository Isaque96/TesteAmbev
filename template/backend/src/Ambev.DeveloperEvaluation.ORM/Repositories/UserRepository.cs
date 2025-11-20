using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(DefaultContext context) : base(context) { }

    public async Task<(IEnumerable<User> Items, int Count)> GetUsersPaginatedAsync(
        int page,
        int size,
        string? order,
        string? username = null,
        string? email = null,
        string? status = null,
        string? role = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        // Filtros básicos (igual doc de produtos, mas pra user)
        if (!string.IsNullOrWhiteSpace(username))
        {
            if (username.StartsWith('*') && username.EndsWith('*'))
            {
                var value = username.Trim('*');
                query = query.Where(u => EF.Functions.ILike(u.Username, $"%{value}%"));
            }
            else if (username.StartsWith('*'))
            {
                var value = username.TrimStart('*');
                query = query.Where(u => EF.Functions.ILike(u.Username, $"%{value}"));
            }
            else if (username.EndsWith('*'))
            {
                var value = username.TrimEnd('*');
                query = query.Where(u => EF.Functions.ILike(u.Username, $"{value}%"));
            }
            else
            {
                query = query.Where(u => u.Username == username);
            }
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(u => u.Email == email);
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse(status, out UserStatus statusEnum))
        {
            query = query.Where(u => u.Status == statusEnum);
        }

        if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse(role, out UserRole roleEnum))
        {
            query = query.Where(u => u.Role == roleEnum);
        }

        // Total antes da paginação
        var totalItems = await query.CountAsync(cancellationToken);

        // Ordenação: "username asc, email desc"
        query = ApplyOrdering(query, order);

        var users = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return (users, totalItems);
    }
    
    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}