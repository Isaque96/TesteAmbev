using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the cart ID.
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Navigation property to Cart.
    /// </summary>
    public Cart Cart { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Navigation property to Product.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the date and time when the cart item was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public CartItem()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the cart item entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new CartItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Updates the quantity and validates business rules.
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (newQuantity > 20)
            throw new InvalidOperationException("Cannot have more than 20 identical items.");

        Quantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates discount based on quantity rules:
    /// - 10% for 4-9 items
    /// - 20% for 10-20 items
    /// - No discount for less than 4 items
    /// </summary>
    public decimal CalculateDiscount()
    {
        if (Quantity < 4)
            return 0;

        decimal itemTotal = Product.Price * Quantity;

        if (Quantity >= 10 && Quantity <= 20)
            return itemTotal * 0.20m;

        if (Quantity >= 4 && Quantity <= 9)
            return itemTotal * 0.10m;

        return 0;
    }
}