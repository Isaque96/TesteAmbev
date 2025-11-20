using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product image URL.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category ID.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Navigation property to Category.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product rating.
    /// </summary>
    public Rating Rating { get; set; } = new();

    /// <summary>
    /// Navigation property to CartItems.
    /// </summary>
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the product entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Updates the product price.
    /// </summary>
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.");
        
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}