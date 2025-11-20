using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for User entity with specific operations
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Retrieve users with pagination
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="order"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="status"></param>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IEnumerable<User> Items, int Count)> GetUsersPaginatedAsync(
        int page,
        int size,
        string? order,
        string? username = null,
        string? email = null,
        string? status = null,
        string? role = null,
        CancellationToken cancellationToken = default
    );
    
    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
