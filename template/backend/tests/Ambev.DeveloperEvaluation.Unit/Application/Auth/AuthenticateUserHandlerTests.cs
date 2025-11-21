using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth;

public class AuthenticateUserHandlerTests
{
    private static (AuthenticateUserHandler handler,
                   IUserRepository userRepository,
                   IPasswordHasher passwordHasher,
                   IJwtTokenGenerator jwtTokenGenerator)
        CreateHandler()
    {
        var userRepository = Substitute.For<IUserRepository>();
        var passwordHasher = Substitute.For<IPasswordHasher>();
        var jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

        var handler = new AuthenticateUserHandler(
            userRepository,
            passwordHasher,
            jwtTokenGenerator
        );

        return (handler, userRepository, passwordHasher, jwtTokenGenerator);
    }

    [Fact(DisplayName = "Given valid credentials and active user, when Handle called, then token is returned")]
    public async Task Given_ValidCredentials_And_ActiveUser_When_Handle_Then_ReturnsToken()
    {
        var (handler, userRepo, passwordHasher, jwtGenerator) = CreateHandler();

        var email = "user@example.com";
        var password = "Pwd@123";

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "john.doe",
            Password = "hashed",
            Status = UserStatus.Active,
            Role = UserRole.Admin
        };

        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher.VerifyPassword(password, user.Password)
            .Returns(true);

        jwtGenerator.GenerateToken(user)
            .Returns("fake-jwt-token");

        var command = new AuthenticateUserCommand
        {
            Email = email,
            Password = password
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal(email, result.Email);
        Assert.Equal("john.doe", result.Name);
        Assert.Equal(nameof(UserRole.Admin), result.Role);
    }

    [Fact(DisplayName = "Given non existing user, when Handle called, then UnauthorizedAccessException")]
    public async Task Given_NonExistingUser_When_Handle_Then_ThrowsUnauthorizedAccess()
    {
        var (handler, userRepo, passwordHasher, _) = CreateHandler();

        var email = "user@example.com";
        var password = "Pwd@123";

        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns((DeveloperEvaluation.Domain.Entities.User?)null);

        var command = new AuthenticateUserCommand
        {
            Email = email,
            Password = password
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(command, CancellationToken.None));

        // passwordHasher não deve nem ser chamado
        passwordHasher.DidNotReceive()
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact(DisplayName = "Given invalid password, when Handle called, then UnauthorizedAccessException")]
    public async Task Given_InvalidPassword_When_Handle_Then_ThrowsUnauthorizedAccess()
    {
        var (handler, userRepo, passwordHasher, _) = CreateHandler();

        var email = "user@example.com";
        var password = "wrong";

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "john.doe",
            Password = "hashed",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher.VerifyPassword(password, user.Password)
            .Returns(false);

        var command = new AuthenticateUserCommand
        {
            Email = email,
            Password = password
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given inactive user, when Handle called, then UnauthorizedAccessException")]
    public async Task Given_InactiveUser_When_Handle_Then_ThrowsUnauthorizedAccess()
    {
        var (handler, userRepo, passwordHasher, _) = CreateHandler();

        var email = "user@example.com";
        var password = "Pwd@123";

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "john.doe",
            Password = "hashed",
            Status = UserStatus.Inactive,
            Role = UserRole.Customer
        };

        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher.VerifyPassword(password, user.Password)
            .Returns(true);

        var command = new AuthenticateUserCommand
        {
            Email = email,
            Password = password
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given suspended user, when Handle called, then UnauthorizedAccessException")]
    public async Task Given_SuspendedUser_When_Handle_Then_ThrowsUnauthorizedAccess()
    {
        var (handler, userRepo, passwordHasher, _) = CreateHandler();

        var email = "user@example.com";
        var password = "Pwd@123";

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "john.doe",
            Password = "hashed",
            Status = UserStatus.Suspended,
            Role = UserRole.Customer
        };

        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(user);

        passwordHasher.VerifyPassword(password, user.Password)
            .Returns(true);

        var command = new AuthenticateUserCommand
        {
            Email = email,
            Password = password
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}