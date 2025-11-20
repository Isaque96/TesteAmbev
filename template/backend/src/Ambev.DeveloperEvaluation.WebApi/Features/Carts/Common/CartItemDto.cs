namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public class CartItemDto
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}