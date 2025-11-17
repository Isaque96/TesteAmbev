namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product rating.
/// </summary>
public class Rating
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}