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
        switch (quantity)
        {
            case <= 0:
                throw new ArgumentException("Quantity must be greater than zero.");
            case > 20:
                throw new InvalidOperationException("Cannot add more than 20 identical items.");
        }

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
        if (item == null) return;
        CartItems.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearCart()
    {
        CartItems.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total price with discounts applied.
    /// </summary>
    public decimal CalculateTotal()
    {
        return (from item in CartItems let itemTotal = item.Product.Price * item.Quantity let discount = item.CalculateDiscount() select itemTotal - discount).Sum();
    }
}