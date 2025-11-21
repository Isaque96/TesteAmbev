using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.User;

public class UpdateUserHandlerTests
{
    private static (DefaultContext context, UpdateUserHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new UpdateUserHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command and existing user, when Handle called, then user is updated")]
    public async Task Given_ValidCommand_And_ExistingUser_When_Handle_Then_UserIsUpdated()
    {
        var (context, handler) = CreateHandler();

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = "old.user",
            Email = "old@example.com",
            Phone = "111111111",
            Password = "OldPwd@123",
            Status = UserStatus.Inactive,
            Role = UserRole.Customer,
            Name =
            {
                FirstName = "Old",
                LastName = "Name"
            },
            Address =
            {
                City = "Old City",
                Street = "Old Street",
                Number = 1,
                ZipCode = "00000-000",
                Geolocation =
                {
                    Lat = "0",
                    Long = "0"
                }
            }
        };

        context.Set<DeveloperEvaluation.Domain.Entities.User>().Add(user);
        await context.SaveChangesAsync();

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            Email = "new@example.com",
            Username = "new.user",
            Password = "NewPwd@123",
            FirstName = "New",
            LastName = "Name",
            City = "New City",
            Street = "New Street",
            Number = 123,
            Zipcode = "12345-678",
            Lat = "10",
            Long = "20",
            Phone = "999999999",
            Status = nameof(UserStatus.Active),
            Role = nameof(UserRole.Admin)
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);

        var updated = await context.Set<DeveloperEvaluation.Domain.Entities.User>().FindAsync(user.Id);
        Assert.NotNull(updated);
        Assert.Equal("new@example.com", updated.Email);
        Assert.Equal("new.user", updated.Username);
        Assert.Equal("NewPwd@123", updated.Password);
        Assert.Equal("New", updated.Name.FirstName);
        Assert.Equal("Name", updated.Name.LastName);
        Assert.Equal("New City", updated.Address.City);
        Assert.Equal("New Street", updated.Address.Street);
        Assert.Equal(123, updated.Address.Number);
        Assert.Equal("12345-678", updated.Address.ZipCode);
        Assert.Equal("10", updated.Address.Geolocation.Lat);
        Assert.Equal("20", updated.Address.Geolocation.Long);
        Assert.Equal("999999999", updated.Phone);
        Assert.Equal(UserStatus.Active, updated.Status);
        Assert.Equal(UserRole.Admin, updated.Role);
    }

    [Fact(DisplayName = "Given non existing user, when Handle called, then key not found exception")]
    public async Task Given_NonExistingUser_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Email = "new@example.com",
            Username = "new.user",
            Password = "NewPwd@123",
            FirstName = "New",
            LastName = "Name",
            City = "New City",
            Street = "New Street",
            Number = 123,
            Zipcode = "12345-678",
            Lat = "10",
            Long = "20",
            Phone = "999999999",
            Status = nameof(UserStatus.Active),
            Role = nameof(UserRole.Admin)
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given invalid command, when Handle called, then validation exception")]
    public async Task Given_InvalidCommand_When_Handle_Then_ThrowsValidationException()
    {
        var (context, handler) = CreateHandler();

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = "valid.user",
            Email = "valid@example.com",
            Phone = "111111111",
            Password = "Pwd@123",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };
        context.Set<DeveloperEvaluation.Domain.Entities.User>().Add(user);
        await context.SaveChangesAsync();

        var command = new UpdateUserCommand
        {
            Id = Guid.Empty,
            Email = "invalid-email",
            Username = "ab",          // < 3
            Password = "123",         // < 6
            FirstName = "",           // required
            LastName = "",            // required
            City = "",                // required
            Street = "",              // required
            Number = 0,               // <= 0
            Zipcode = "",             // required
            Lat = "",                 // required
            Long = "",                // required
            Phone = "",               // required
            Status = "",              // required
            Role = ""                 // required
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given invalid status and role strings, when Handle called, then user stays with previous enums")]
    public async Task Given_InvalidStatusAndRoleStrings_When_Handle_Then_EnumsNotChanged()
    {
        var (context, handler) = CreateHandler();

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = "user",
            Email = "user@example.com",
            Phone = "111111111",
            Password = "Pwd@123",
            Status = UserStatus.Active,
            Role = UserRole.Admin
        };

        context.Set<DeveloperEvaluation.Domain.Entities.User>().Add(user);
        await context.SaveChangesAsync();

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            Email = "user2@example.com",
            Username = "user2",
            Password = "Pwd@1234",
            FirstName = "User",
            LastName = "Two",
            City = "City",
            Street = "Street",
            Number = 10,
            Zipcode = "12345-000",
            Lat = "1",
            Long = "2",
            Phone = "222222222",
            Status = "INVALID_STATUS",
            Role = "INVALID_ROLE"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);

        var updated = await context.Set<DeveloperEvaluation.Domain.Entities.User>().FindAsync(user.Id);
        Assert.NotNull(updated);
        Assert.Equal(UserStatus.Active, updated.Status);
        Assert.Equal(UserRole.Admin, updated.Role);
    }
}