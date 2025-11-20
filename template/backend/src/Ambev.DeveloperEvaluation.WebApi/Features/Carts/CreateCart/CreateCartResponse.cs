using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Response model for create cart operation
/// </summary>
public class CreateCartResponse
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
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
}