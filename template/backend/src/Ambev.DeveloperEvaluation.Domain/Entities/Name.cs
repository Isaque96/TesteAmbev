namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a user's name.
/// </summary>
public class Name
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
}