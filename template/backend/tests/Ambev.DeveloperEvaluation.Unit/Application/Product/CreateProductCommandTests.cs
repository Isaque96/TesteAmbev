using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Product
{
    public class CreateProductCommandTests
    {
        [Fact(DisplayName = "Given valid product data, when Validate called, then result is valid")]
        public void Given_ValidProduct_When_Validate_Then_ResultIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Title        = "Test Product",
                Price        = 19.99m,
                Description  = "A valid product description",
                CategoryName = "Category A",
                Image        = "https://example.com/image.png",
                Rate         = 4.5m,
                Count        = 100
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact(DisplayName = "Given empty title, when Validate called, then result is invalid")]
        public void Given_EmptyTitle_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Title        = string.Empty,
                Price        = 19.99m,
                Description  = "Description",
                CategoryName = "Category A",
                Image        = "https://example.com/image.png",
                Rate         = 4.5m,
                Count        = 10
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given non-positive price, when Validate called, then result is invalid")]
        public void Given_NonPositivePrice_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Title        = "Valid title",
                Price        = 0m,
                Description  = "Description",
                CategoryName = "Category A",
                Image        = "https://example.com/image.png",
                Rate         = 4.5m,
                Count        = 10
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty category and image, when Validate called, then result is invalid with multiple errors")]
        public void Given_EmptyCategoryAndImage_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Title        = "Valid title",
                Price        = 10m,
                Description  = "Description",
                CategoryName = string.Empty,
                Image        = string.Empty,
                Rate         = 4.5m,
                Count        = 10
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);

            var errors = result.Errors.ToList();
            Assert.True(errors.Count >= 1);
        }

        [Fact(DisplayName = "Given invalid rate or count, when Validate called, then result is invalid")]
        public void Given_InvalidRateOrCount_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Title        = "Valid title",
                Price        = 10m,
                Description  = "Description",
                CategoryName = "Category A",
                Image        = "https://example.com/image.png",
                Rate         = -1m, // inválido
                Count        = -5   // inválido
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }
    }
}