using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersQuery : IRequest<ListUsersResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; } // "username asc, email desc"

    // Filtros básicos seguindo a doc (você pode expandir depois)
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; } // Active, Inactive, Suspended
    public string? Role { get; set; }   // Customer, Manager, Admin
}