namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Request model for deleting a product
/// </summary>
public class DeleteProductRequest
{
    /// <summary>
    /// Product ID to delete
    /// </summary>
    public Guid Id { get; set; }
}