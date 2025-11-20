namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersResponse
{
    public Guid Id { get; set; }
    public string? Name  { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}