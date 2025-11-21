using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Fact(DisplayName = "UpdateQuantity should update quantity when valid")]
    public void Given_ValidQuantity_When_UpdateQuantity_Then_QuantityIsUpdated()
    {
        // Arrange
        var cartItem = new CartItem { Quantity = 1 };

        // Act
        cartItem.UpdateQuantity(5);

        // Assert
        Assert.Equal(5, cartItem.Quantity);
    }

    [Theory(DisplayName = "UpdateQuantity should throw ArgumentException for zero or negative quantity")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_InvalidQuantity_When_UpdateQuantity_Then_ThrowsArgumentException(int quantity)
    {
        // Arrange
        var cartItem = new CartItem { Quantity = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cartItem.UpdateQuantity(quantity));
    }

    [Fact(DisplayName = "UpdateQuantity should throw InvalidOperationException for quantity greater than 20")]
    public void Given_QuantityGreaterThan20_When_UpdateQuantity_Then_ThrowsInvalidOperationException()
    {
        // Arrange
        var cartItem = new CartItem { Quantity = 1 };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cartItem.UpdateQuantity(21));
    }

    [Theory(DisplayName = "CalculateDiscount should return correct discount based on quantity")]
    [InlineData(3, 0)]
    [InlineData(4, 0.10)]
    [InlineData(9, 0.10)]
    [InlineData(10, 0.20)]
    [InlineData(20, 0.20)]
    public void Given_Quantity_When_CalculateDiscount_Then_ReturnsExpectedDiscount(int quantity, decimal expectedDiscountRate)
    {
        // Arrange
        var productPrice = 100m;
        var cartItem = new CartItem
        {
            Quantity = quantity,
            Product = new Product { Price = productPrice }
        };

        var expectedDiscount = productPrice * quantity * expectedDiscountRate;

        // Act
        var discount = cartItem.CalculateDiscount();

        // Assert
        Assert.Equal(expectedDiscount, discount);
    }

    [Fact(DisplayName = "Validate should return valid result for valid cart item")]
    public void Given_ValidCartItem_When_Validate_Then_ReturnsValid()
    {
        // Arrange
        var cartItem = new CartItem
        {
            CartId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        // Act
        var result = cartItem.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}