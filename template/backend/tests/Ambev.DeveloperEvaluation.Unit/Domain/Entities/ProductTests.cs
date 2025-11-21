using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductTests
{
    [Fact(DisplayName = "UpdatePrice should update price when valid")]
    public void Given_ValidPrice_When_UpdatePrice_Then_PriceIsUpdated()
    {
        // Arrange
        var product = new Product { Price = 10m };

        // Act
        product.UpdatePrice(20m);

        // Assert
        Assert.Equal(20m, product.Price);
    }

    [Fact(DisplayName = "UpdatePrice should throw ArgumentException for negative price")]
    public void Given_NegativePrice_When_UpdatePrice_Then_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product { Price = 10m };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.UpdatePrice(-5m));
    }

    [Fact(DisplayName = "Validate should return valid result for valid product")]
    public void Given_ValidProduct_When_Validate_Then_ReturnsValid()
    {
        // Arrange
        var product = new Product
        {
            Title = "Sample Product",
            Price = 10m,
            Description = "Sample Description",
            Image = "https://example.com/image.jpg",
            CategoryId = Guid.NewGuid()
        };

        // Act
        var result = product.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}