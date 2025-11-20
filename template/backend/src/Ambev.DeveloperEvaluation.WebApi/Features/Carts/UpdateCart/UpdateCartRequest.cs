using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Request model for updating a cart
/// </summary>
public class UpdateCartRequest
{
    /// <summary>
    /// Cart ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User ID who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Cart date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// List of products in the cart
    /// </summary>
    public List<CartItemDto> Products { get; set; } = new();
}