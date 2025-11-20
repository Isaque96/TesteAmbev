namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Request model for getting a product by ID
/// </summary>
public class GetProductRequest
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid Id { get; set; }
}