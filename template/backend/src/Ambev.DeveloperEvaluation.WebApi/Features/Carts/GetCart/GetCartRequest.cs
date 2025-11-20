namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Request model for getting a cart by ID
/// </summary>
public class GetCartRequest
{
    /// <summary>
    /// Cart ID
    /// </summary>
    public Guid Id { get; set; }
}