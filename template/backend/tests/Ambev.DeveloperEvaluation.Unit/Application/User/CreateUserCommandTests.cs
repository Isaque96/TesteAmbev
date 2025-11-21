using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.User
{
    public class CreateUserCommandTests
    {
        [Fact(DisplayName = "Given valid user data, when Validate called, then result is valid")]
        public void Given_ValidUser_When_Validate_Then_ResultIsValid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "valid_user",
                Password = "StrongP@ssw0rd!",
                Phone    = "+5511999999999",
                Email    = "user@example.com",
                Status   = UserStatus.Active,
                Role     = UserRole.Admin
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact(DisplayName = "Given empty username, when Validate called, then result is invalid")]
        public void Given_EmptyUsername_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = string.Empty,
                Password = "StrongP@ssw0rd!",
                Phone    = "+55 11 99999-9999",
                Email    = "user@example.com",
                Status   = UserStatus.Active,
                Role     = UserRole.Admin
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty password, when Validate called, then result is invalid")]
        public void Given_EmptyPassword_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "valid_user",
                Password = string.Empty,
                Phone    = "+55 11 99999-9999",
                Email    = "user@example.com",
                Status   = UserStatus.Active,
                Role     = UserRole.Admin
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given invalid email, when Validate called, then result is invalid")]
        public void Given_InvalidEmail_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "valid_user",
                Password = "StrongP@ssw0rd!",
                Phone    = "+55 11 99999-9999",
                Email    = "not-an-email",
                Status   = UserStatus.Active,
                Role     = UserRole.Admin
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty username and password, when Validate called, then result is invalid with multiple errors")]
        public void Given_EmptyUsernameAndPassword_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = string.Empty,
                Password = string.Empty,
                Phone    = string.Empty,
                Email    = string.Empty,
                Status   = UserStatus.Active,
                Role     = UserRole.Customer
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