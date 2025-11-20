namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;

/// <summary>
/// Response model for create category operation
/// </summary>
public class CreateCategoryResponse
{
    /// <summary>
    /// CategoryName ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// CategoryName name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CategoryName description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
}