using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Product
{
    public class UpdateProductCommandTests
    {
        [Fact(DisplayName = "Given valid product data, when Validate called, then result is valid")]
        public void Given_ValidProduct_When_Validate_Then_ResultIsValid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id           = Guid.NewGuid(),
                Title        = "Updated Product",
                Price        = 25.50m,
                Description  = "Updated description",
                CategoryName = "Updated Category",
                Image        = "https://example.com/image.png",
                Rate         = 4.8m,
                Count        = 50
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
            var command = new UpdateProductCommand
            {
                Id           = Guid.Empty,
                Title        = "Title",
                Price        = 10m,
                Description  = "Desc",
                CategoryName = "Category",
                Image        = "https://example.com/image.png",
                Rate         = 4m,
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

        [Fact(DisplayName = "Given empty title, when Validate called, then result is invalid")]
        public void Given_EmptyTitle_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id           = Guid.NewGuid(),
                Title        = string.Empty,
                Price        = 10m,
                Description  = "Desc",
                CategoryName = "Category",
                Image        = "https://example.com/image.png",
                Rate         = 4m,
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
            var command = new UpdateProductCommand
            {
                Id           = Guid.NewGuid(),
                Title        = "Valid title",
                Price        = 0m,
                Description  = "Desc",
                CategoryName = "Category",
                Image        = "https://example.com/image.png",
                Rate         = 4m,
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

        [Fact(DisplayName = "Given invalid rate or count, when Validate called, then result is invalid")]
        public void Given_InvalidRateOrCount_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id           = Guid.NewGuid(),
                Title        = "Valid title",
                Price        = 10m,
                Description  = "Desc",
                CategoryName = "Category",
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

        [Fact(DisplayName = "Given empty required fields, when Validate called, then result is invalid with multiple errors")]
        public void Given_EmptyRequiredFields_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id           = Guid.Empty,
                Title        = string.Empty,
                Price        = 0m,
                Description  = string.Empty,
                CategoryName = string.Empty,
                Image        = string.Empty,
                Rate         = -1m,
                Count        = -1
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
    }
}