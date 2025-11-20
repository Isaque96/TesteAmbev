namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;

/// <summary>
/// Response model for get category operation
/// </summary>
public class GetCategoryResponse
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

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}