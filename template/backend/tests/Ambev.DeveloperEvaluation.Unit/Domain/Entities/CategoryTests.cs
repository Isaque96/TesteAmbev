using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CategoryTests
{
    [Fact(DisplayName = "CategoryName properties should be set and retrieved correctly")]
    public void Given_CategoryProperties_When_Set_Then_PropertiesAreCorrect()
    {
        // Arrange
        var category = new Category
        {
            // Act
            Name = "Electronics",
            Description = "Electronic devices and gadgets"
        };

        // Assert
        Assert.Equal("Electronics", category.Name);
        Assert.Equal("Electronic devices and gadgets", category.Description);
    }

    [Fact(DisplayName = "Validate should return valid result for valid category")]
    public void Given_ValidCategory_When_Validate_Then_ReturnsValid()
    {
        // Arrange
        var category = new Category
        {
            Name = "Books",
            Description = "All kinds of books"
        };

        // Act
        var result = category.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}