namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;

/// <summary>
/// Response model for list categories operation
/// </summary>
public class ListCategoriesResponse
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