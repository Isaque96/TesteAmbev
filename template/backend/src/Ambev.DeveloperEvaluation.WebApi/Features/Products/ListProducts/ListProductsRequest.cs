using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Request model for listing products with pagination
/// </summary>
public class ListProductsRequest : PaginatedRequest
{
    /// <summary>
    /// Filter by category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Filter by title (partial match)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }
}