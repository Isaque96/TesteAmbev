using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

/// <summary>
/// Request model for listing carts with pagination
/// </summary>
public class ListCartsRequest : PaginatedRequest
{
    /// <summary>
    /// Filter by user ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Minimum date filter
    /// </summary>
    public DateTime? MinDate { get; set; }

    /// <summary>
    /// Maximum date filter
    /// </summary>
    public DateTime? MaxDate { get; set; }
}