namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;

/// <summary>
/// Response model for update category operation
/// </summary>
public class UpdateCategoryResponse
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
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}