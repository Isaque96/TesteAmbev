namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;

/// <summary>
/// Request model for deleting a cart
/// </summary>
public class DeleteCartRequest
{
    /// <summary>
    /// Cart ID to delete
    /// </summary>
    public Guid Id { get; set; }
}