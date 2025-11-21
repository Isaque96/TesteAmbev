using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using AutoMapper.Configuration.Annotations;

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
    [Ignore]
    public Cart Cart { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Navigation property to Product.
    /// </summary>
    [Ignore]
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

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
        switch (newQuantity)
        {
            case <= 0:
                throw new ArgumentException("Quantity must be greater than zero.");
            case > 20:
                throw new InvalidOperationException("Cannot have more than 20 identical items.");
            default:
                Quantity = newQuantity;
                UpdatedAt = DateTime.UtcNow;
                break;
        }
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

        var itemTotal = Product.Price * Quantity;

        return Quantity switch
        {
            >= 10 and <= 20 => itemTotal * 0.20m,
            >= 4 and <= 9 => itemTotal * 0.10m,
            _ => 0
        };
    }
}