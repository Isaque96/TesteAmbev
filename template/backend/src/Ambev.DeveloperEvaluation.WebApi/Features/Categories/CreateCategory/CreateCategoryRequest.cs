namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;

/// <summary>
/// Request model for creating a new category
/// </summary>
public class CreateCategoryRequest
{
    /// <summary>
    /// CategoryName name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CategoryName description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}