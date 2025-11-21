using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;

/// <summary>
/// Request model for listing categories with pagination
/// </summary>
public class ListCategoriesRequest
{
    /// <summary>
    /// Filter by name (partial match)
    /// </summary>
    public string? Name { get; set; }
}