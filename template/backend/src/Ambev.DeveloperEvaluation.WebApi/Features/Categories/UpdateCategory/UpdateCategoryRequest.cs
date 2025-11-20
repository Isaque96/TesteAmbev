namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;

/// <summary>
/// Request model for updating a category
/// </summary>
public class UpdateCategoryRequest
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
}