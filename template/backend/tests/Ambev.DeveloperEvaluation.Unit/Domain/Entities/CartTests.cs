using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact(DisplayName = "AddProduct should add new item when product not in cart")]
    public void Given_EmptyCart_When_AddProduct_Then_ItemIsAdded()
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();
        int quantity = 5;

        // Act
        cart.AddProduct(productId, quantity);

        // Assert
        Assert.Single(cart.CartItems);
        var item = cart.CartItems.First();
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
    }

    [Fact(DisplayName = "AddProduct should update quantity when product already in cart")]
    public void Given_CartWithProduct_When_AddProduct_Then_QuantityIsUpdated()
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();
        cart.AddProduct(productId, 3);

        // Act
        cart.AddProduct(productId, 2);

        // Assert
        Assert.Single(cart.CartItems);
        var item = cart.CartItems.First();
        Assert.Equal(5, item.Quantity);
    }

    [Theory(DisplayName = "AddProduct should throw exception for invalid quantity")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Given_InvalidQuantity_When_AddProduct_Then_ThrowsArgumentException(int quantity)
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cart.AddProduct(productId, quantity));
    }

    [Fact(DisplayName = "AddProduct should throw exception when quantity exceeds 20")]
    public void Given_QuantityGreaterThan20_When_AddProduct_Then_ThrowsInvalidOperationException()
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cart.AddProduct(productId, 21));
    }

    [Fact(DisplayName = "RemoveProduct should remove item if exists")]
    public void Given_CartWithProduct_When_RemoveProduct_Then_ItemIsRemoved()
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();
        cart.AddProduct(productId, 1);

        // Act
        cart.RemoveProduct(productId);

        // Assert
        Assert.Empty(cart.CartItems);
    }

    [Fact(DisplayName = "RemoveProduct should do nothing if product not in cart")]
    public void Given_CartWithoutProduct_When_RemoveProduct_Then_NoChange()
    {
        // Arrange
        var cart = new Cart();
        var productId = Guid.NewGuid();

        // Act
        cart.RemoveProduct(productId);

        // Assert
        Assert.Empty(cart.CartItems);
    }

    [Fact(DisplayName = "CalculateTotal should return sum of items with discounts")]
    public void Given_CartWithItems_When_CalculateTotal_Then_ReturnsCorrectTotal()
    {
        // Arrange
        var cart = new Cart();

        var product1 = new Product { Id = Guid.NewGuid(), Price = 10m };
        var product2 = new Product { Id = Guid.NewGuid(), Price = 20m };

        var item1 = new CartItem { Product = product1, ProductId = product1.Id, Quantity = 2 };
        var item2 = new CartItem { Product = product2, ProductId = product2.Id, Quantity = 1 };

        cart.CartItems.Add(item1);
        cart.CartItems.Add(item2);

        // Mock discount calculation by overriding CalculateDiscount method if possible
        // Otherwise, assume no discount for this test

        // Act
        var total = cart.CalculateTotal();

        // Assert
        var expectedTotal = (product1.Price * item1.Quantity) + (product2.Price * item2.Quantity);
        Assert.Equal(expectedTotal, total);
    }

    [Fact(DisplayName = "Validate should return valid result for valid cart")]
    public void Given_ValidCart_When_Validate_Then_ReturnsValid()
    {
        // Arrange
        var cart = new Cart
        {
            UserId = Guid.NewGuid(),
            Date = DateTime.UtcNow,
        };
        cart.AddProduct(Guid.NewGuid(), 1);

        // Act
        var result = cart.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}