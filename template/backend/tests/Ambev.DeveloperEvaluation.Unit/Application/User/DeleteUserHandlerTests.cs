using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using FluentValidation;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.User;

public class DeleteUserHandlerTests
{
    private static (DefaultContext context, DeleteUserHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();
        var handler = new DeleteUserHandler(repository);
        return (context, handler);
    }

    [Fact(DisplayName = "Given valid command for existing user, when Handle called, then user is deleted")]
    public async Task Given_ValidCommand_And_ExistingUser_When_Handle_Then_UserIsDeleted()
    {
        var (context, handler) = CreateHandler();

        var user = new DeveloperEvaluation.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = "john.doe",
            Email = "john@example.com",
            Phone = "999999999",
            Password = "Pwd@123",
        };

        context.Set<DeveloperEvaluation.Domain.Entities.User>().Add(user);
        await context.SaveChangesAsync();

        var command = new DeleteUserCommand { Id = user.Id };

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.Success);

        var deleted = await context.Set<DeveloperEvaluation.Domain.Entities.User>().FindAsync(user.Id);
        Assert.Null(deleted);
    }

    [Fact(DisplayName = "Given invalid command (empty id), when Handle called, then validation exception")]
    public async Task Given_InvalidCommand_EmptyId_When_Handle_Then_ThrowsValidationException()
    {
        var (_, handler) = CreateHandler();

        var command = new DeleteUserCommand { Id = Guid.Empty };

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Given non existing user, when Handle called, then key not found exception")]
    public async Task Given_NonExistingUser_When_Handle_Then_ThrowsKeyNotFoundException()
    {
        var (_, handler) = CreateHandler();

        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}