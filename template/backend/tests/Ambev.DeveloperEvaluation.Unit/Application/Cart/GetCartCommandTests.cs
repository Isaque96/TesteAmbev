using Ambev.DeveloperEvaluation.Application.Cart.CreateCart;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class CreateCartCommandTests
{
    [Fact(DisplayName = "Given valid command, when Validate called, then result is valid and has no errors")]
    public void Given_ValidCommand_When_Validate_Then_ResultIsValid()
    {
        // Arrange
        var command = new CreateCartCommand
        {
            UserId = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            Products = new List<CreateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2
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

    [Fact(DisplayName = "Given invalid command, when Validate called, then result is invalid and contains errors")]
    public void Given_InvalidCommand_When_Validate_Then_ResultIsInvalid()
    {
        // Arrange
        var command = new CreateCartCommand
        {
            UserId = Guid.Empty,                 // inválido
            Date = default,                      // inválido
            Products = new List<CreateCartProductItem>
            {
                new()
                {
                    ProductId = Guid.Empty,      // inválido
                    Quantity = 0                 // inválido
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

        // Se quiser ser mais específico:
        var errorList = result.Errors.ToList();
        Assert.True(errorList.Count >= 3);
    }
}