using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

/// <summary>
/// Response model for list carts operation
/// </summary>
public class ListCartsResponse
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

    /// <summary>
    /// Total price with discounts applied
    /// </summary>
    public decimal TotalPrice { get; set; }
}