using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    /// <summary>
    /// Gets or sets the user ID who owns the cart.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the cart date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets the date and time when the cart was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Navigation property to CartItems.
    /// </summary>
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public Cart()
    {
        Date = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the cart entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new CartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Adds a product to the cart with quantity.
    /// Applies discount rules based on quantity.
    /// </summary>
    public void AddProduct(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (quantity > 20)
            throw new InvalidOperationException("Cannot add more than 20 identical items.");

        var existingItem = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = Id,
                ProductId = productId,
                Quantity = quantity
            };
            CartItems.Add(cartItem);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a product from the cart.
    /// </summary>
    public void RemoveProduct(Guid productId)
    {
        var item = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (item != null)
        {
            CartItems.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Calculates the total price with discounts applied.
    /// </summary>
    public decimal CalculateTotal()
    {
        decimal total = 0;

        foreach (var item in CartItems)
        {
            decimal itemTotal = item.Product.Price * item.Quantity;
            decimal discount = item.CalculateDiscount();
            total += itemTotal - discount;
        }

        return total;
    }
}