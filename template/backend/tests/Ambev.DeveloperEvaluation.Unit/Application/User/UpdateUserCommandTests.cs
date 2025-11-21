using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.User
{
    public class UpdateUserCommandTests
    {
        [Fact(DisplayName = "Given valid user data, when Validate called, then result is valid")]
        public void Given_ValidUser_When_Validate_Then_ResultIsValid()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id        = Guid.NewGuid(),
                Email     = "user@example.com",
                Username  = "valid_user",
                Password  = "StrongP@ssw0rd!",
                FirstName = "John",
                LastName  = "Doe",
                City      = "São Paulo",
                Street    = "Rua X",
                Number    = 123,
                Zipcode   = "01234-567",
                Lat       = "-23.5505",
                Long      = "-46.6333",
                Phone     = "+55 11 99999-9999",
                Status    = "Active",
                Role      = "Admin"
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
            var command = new UpdateUserCommand
            {
                Id        = Guid.Empty,
                Email     = "user@example.com",
                Username  = "valid_user",
                Password  = "StrongP@ssw0rd!",
                FirstName = "John",
                LastName  = "Doe",
                City      = "São Paulo",
                Street    = "Rua X",
                Number    = 123,
                Zipcode   = "01234-567",
                Lat       = "-23.5505",
                Long      = "-46.6333",
                Phone     = "+55 11 99999-9999",
                Status    = "Active",
                Role      = "Admin"
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given empty email and username, when Validate called, then result is invalid")]
        public void Given_EmptyEmailAndUsername_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id        = Guid.NewGuid(),
                Email     = string.Empty,
                Username  = string.Empty,
                Password  = "StrongP@ssw0rd!",
                FirstName = "John",
                LastName  = "Doe",
                City      = "São Paulo",
                Street    = "Rua X",
                Number    = 123,
                Zipcode   = "01234-567",
                Lat       = "-23.5505",
                Long      = "-46.6333",
                Phone     = "+55 11 99999-9999",
                Status    = "Active",
                Role      = "Admin"
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given invalid address fields, when Validate called, then result is invalid")]
        public void Given_InvalidAddress_When_Validate_Then_ResultIsInvalid()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id        = Guid.NewGuid(),
                Email     = "user@example.com",
                Username  = "valid_user",
                Password  = "StrongP@ssw0rd!",
                FirstName = "John",
                LastName  = "Doe",
                City      = string.Empty,
                Street    = string.Empty,
                Number    = 0,
                Zipcode   = string.Empty,
                Lat       = string.Empty,
                Long      = string.Empty,
                Phone     = "invalid_phone",
                Status    = string.Empty,
                Role      = string.Empty
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);

            var errors = result.Errors.ToList();
            Assert.True(errors.Count >= 1); // vários erros esperados (City, Street, Number, Zipcode, Phone, Status, Role, etc.)
        }

        [Fact(DisplayName = "Given empty required fields, when Validate called, then result is invalid with multiple errors")]
        public void Given_AllEmptyRequiredFields_When_Validate_Then_ResultIsInvalidWithMultipleErrors()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id        = Guid.Empty,
                Email     = string.Empty,
                Username  = string.Empty,
                Password  = string.Empty,
                FirstName = string.Empty,
                LastName  = string.Empty,
                City      = string.Empty,
                Street    = string.Empty,
                Number    = 0,
                Zipcode   = string.Empty,
                Lat       = string.Empty,
                Long      = string.Empty,
                Phone     = string.Empty,
                Status    = string.Empty,
                Role      = string.Empty
            };

            // Act
            var result = command.Validate();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(result.Errors);

            var errors = result.Errors.ToList();
            Assert.True(errors.Count >= 1); // monte de erro, como esperado
        }
    }
}