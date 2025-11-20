using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersRequest : PaginatedRequest
{
    public string? Name  { get; set; }
    public string? Email { get; set; }
}