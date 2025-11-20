namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

public class UpdateUserRequest
{
    public Guid Id { get; set; }

    public string? Name  { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}