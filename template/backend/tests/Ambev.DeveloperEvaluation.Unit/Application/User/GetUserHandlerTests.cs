using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.User;

public class GetUserHandlerTests
{
    private static (DefaultContext context, GetUserHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new GetUserHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command and existing user, when Handle called, then user is returned")]
    public async Task Given_ValidCommand_And_ExistingUser_When_Handle_Then_UserIsReturned()
    {
        var (context, handler) = CreateHandler();

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = "john.doe",
            Email = "john@example.com",
            Phone = "999999999",
            Password = "Pwd@123",
            Name =
            {
                FirstName = "John",
                LastName = "Doe"
            },
            Address =
            {
                City = "City",
                Street = "Street",
                Number = 123,
                ZipCode = "12345-678",
                Geolocation =
                {
                    Lat = "10",
                    Long = "20"
                }
            }
        };

        context.Set<DeveloperEvaluation.Domain.Entities.User>().Add(user);
        await context.SaveChangesAsync();

        var command = new GetUserCommand { Id = user.Id };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Phone, result.Phone);
    }

    [Fact(DisplayName = "Given invalid command (empty id), when Handle called, then validation exception")]
    public async Task Given_InvalidCommand_EmptyId_When_Handle_Then_ThrowsValidationException()
    {
        var (_, handler) = CreateHandler();

        var command = new GetUserCommand { Id = Guid.Empty };

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given non existing user, when Handle called, then key not found exception")]
    public async Task Given_NonExistingUser_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new GetUserCommand { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}