using Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Category;

public class UpdateCategoryCommandTests
    {
        [Fact(DisplayName = "Given valid category data, when Validate called, then result is valid")]
        public void Given_ValidCategory_When_Validate_Then_ResultIsValid()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
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

        [Fact(DisplayName = "Given empty id, when Validate called, then result is invalid")]
        public void Given_EmptyId_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                Id = Guid.Empty,
                Name = "Beers",
                Description = "Desc"
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty name, when Validate called, then result is invalid")]
        public void Given_EmptyName_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Description = "Valid description"
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty id and name, when Validate called, then result is invalid with multiple errors")]
        public void Given_EmptyIdAndName_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                Id = Guid.Empty,
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
            Assert.True(errors.Count >= 1);
        }
    }