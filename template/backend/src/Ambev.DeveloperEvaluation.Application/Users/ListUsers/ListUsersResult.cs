namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersResult
{
    public IEnumerable<ListUsersItem> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}