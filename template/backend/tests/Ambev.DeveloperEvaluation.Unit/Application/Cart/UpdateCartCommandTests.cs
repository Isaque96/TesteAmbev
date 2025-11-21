using Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class UpdateCartCommandTests
{
    [Fact(DisplayName = "Given valid cart data, when Validate called, then result is valid")]
    public void Given_ValidCart_When_Validate_Then_ResultIsValid()
    {
        // Arrange
        var command = new UpdateCartCommand
        {
            Id     = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date   = DateTime.UtcNow,
            Products = new List<UpdateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity  = 2
                }
            }
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Given empty id, when Validate called, then result is invalid")]
    public void Given_EmptyId_When_Validate_Then_ResultIsInvalid()
    {
        // Arrange
        var command = new UpdateCartCommand
        {
            Id     = Guid.Empty,
            UserId = Guid.NewGuid(),
            Date   = DateTime.UtcNow,
            Products = new List<UpdateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity  = 1
                }
            }
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Given empty user id, when Validate called, then result is invalid")]
    public void Given_EmptyUserId_When_Validate_Then_ResultIsInvalid()
    {
        // Arrange
        var command = new UpdateCartCommand
        {
            Id     = Guid.NewGuid(),
            UserId = Guid.Empty,
            Date   = DateTime.UtcNow,
            Products = new List<UpdateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity  = 1
                }
            }
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Given empty products, when Validate called, then result is invalid")]
    public void Given_EmptyProducts_When_Validate_Then_ResultIsInvalid()
    {
        // Arrange
        var command = new UpdateCartCommand
        {
            Id        = Guid.NewGuid(),
            UserId    = Guid.NewGuid(),
            Date      = DateTime.UtcNow,
            Products  = new List<UpdateCartProductItem>() // vazio
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Given invalid product items, when Validate called, then result is invalid with multiple errors")]
    public void Given_InvalidProducts_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
    {
        // Arrange
        var command = new UpdateCartCommand
        {
            Id     = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date   = DateTime.UtcNow,
            Products = new List<UpdateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.Empty, // inválido
                    Quantity  = 0          // inválido
                }
            }
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);

        var errors = result.Errors.ToList();
        Assert.True(errors.Count >= 1); // provavelmente vários erros (ProductId, Quantity, etc.)
    }
}