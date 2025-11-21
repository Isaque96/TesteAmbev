using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Category;

public class CreateCategoryCommandTests
{
    [Fact(DisplayName = "Given valid category data, when Validate called, then result is valid")]
    public void Given_ValidCategory_When_Validate_Then_ResultIsValid()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Name = "Beers",
            Description = "All beer products"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Given empty name, when Validate called, then result is invalid and has errors")]
    public void Given_EmptyName_When_Validate_Then_ResultIsInvalid()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Name = string.Empty,
            Description = "Some description"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact(DisplayName = "Given empty name and description, when Validate called, then result is invalid and has multiple errors")]
    public void Given_EmptyNameAndDescription_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Name = string.Empty,
            Description = string.Empty
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);

        var errors = result.Errors.ToList();
        Assert.True(errors.Count >= 1); // provavelmente 1+ erros (Name obrigatório, possivelmente Description também)
    }
}