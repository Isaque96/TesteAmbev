using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth;

public class AuthenticateUserValidatorTests
{
    private readonly AuthenticateUserValidator _validator = new();

    [Fact(DisplayName = "Given valid command, when validate, then validation succeeds")]
    public async Task Given_ValidCommand_When_Validate_Then_ValidationSucceeds()
    {
        var command = new AuthenticateUserCommand
        {
            Email = "user@test.com",
            Password = "123456"
        };

        var result = await _validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Given empty email, when validate, then validation fails")]
    public async Task Given_EmptyEmail_When_Validate_Then_ValidationFails()
    {
        var command = new AuthenticateUserCommand
        {
            Email = string.Empty,
            Password = "123456"
        };

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "Given invalid email format, when validate, then validation fails")]
    public async Task Given_InvalidEmailFormat_When_Validate_Then_ValidationFails()
    {
        var command = new AuthenticateUserCommand
        {
            Email = "invalid-email",
            Password = "123456"
        };

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "Given empty password, when validate, then validation fails")]
    public async Task Given_EmptyPassword_When_Validate_Then_ValidationFails()
    {
        var command = new AuthenticateUserCommand
        {
            Email = "user@test.com",
            Password = string.Empty
        };

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact(DisplayName = "Given short password, when validate, then validation fails")]
    public async Task Given_ShortPassword_When_Validate_Then_ValidationFails()
    {
        var command = new AuthenticateUserCommand
        {
            Email = "user@test.com",
            Password = "12345" // 5 chars, less than minimum 6
        };

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact(DisplayName = "Given invalid email and password, when validate, then multiple errors returned")]
    public async Task Given_InvalidEmailAndPassword_When_Validate_Then_MultipleErrors()
    {
        var command = new AuthenticateUserCommand
        {
            Email = "",
            Password = "123" // too short
        };

        var result = await _validator.ValidateAsync(command);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 2);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }
}