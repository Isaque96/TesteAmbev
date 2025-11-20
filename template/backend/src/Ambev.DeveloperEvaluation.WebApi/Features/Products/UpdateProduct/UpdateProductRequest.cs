using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Request model for updating a product
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product image URL
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// CategoryName ID
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Product rating
    /// </summary>
    public RatingDto Rating { get; set; } = new();
}