using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListUsersHandlerTests
{
    private static (DefaultContext context, ListUsersHandler handler) CreateHandler()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();
        var mapper = Helper.CreateMapper();
        var handler = new ListUsersHandler(repository, mapper);
        return (context, handler);
    }

    [Fact(DisplayName = "Given users exist, when Handle called, then paged result is returned")]
    public async Task Given_UsersExist_When_Handle_Then_PagedResultReturned()
    {
        var (context, handler) = CreateHandler();

        for (int i = 0; i < 25; i++)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = $"user{i:D2}",
                Email = $"user{i}@example.com",
                Phone = "999999999",
                Password = "Pwd@123",
                Status = i % 2 == 0 ? UserStatus.Active : UserStatus.Inactive,
                Role = i % 2 == 0 ? UserRole.Admin : UserRole.Customer
            };
            context.Set<User>().Add(user);
        }

        await context.SaveChangesAsync();

        var query = new ListUsersQuery
        {
            Page = 2,
            Size = 10,
            Order = "username",
            Username = null,
            Email = null,
            Status = null,
            Role = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(25, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.CurrentPage);

        var data = result.Data.ToList();
        Assert.Equal(10, data.Count);
    }

    [Fact(DisplayName = "Given no users, when Handle called, then empty result is returned")]
    public async Task Given_NoUsers_When_Handle_Then_EmptyResult()
    {
        var (_, handler) = CreateHandler();

        var query = new ListUsersQuery
        {
            Page = 1,
            Size = 10,
            Order = "username",
            Username = null,
            Email = null,
            Status = null,
            Role = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
        Assert.Empty(result.Data);
    }

    [Fact(DisplayName = "Given filters, when Handle called, then only filtered users are returned")]
    public async Task Given_Filters_When_Handle_Then_FilteredUsersReturned()
    {
        var (context, handler) = CreateHandler();

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Username = "john.doe",
            Email = "john@example.com",
            Phone = "999999999",
            Password = "Pwd@123",
            Status = UserStatus.Active,
            Role = UserRole.Admin
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Username = "jane.doe",
            Email = "jane@example.com",
            Phone = "999999998",
            Password = "Pwd@123",
            Status = UserStatus.Inactive,
            Role = UserRole.Customer
        };
        context.Set<User>().AddRange(user1, user2);
        await context.SaveChangesAsync();

        var query = new ListUsersQuery
        {
            Page = 1,
            Size = 10,
            Order = "username",
            Username = "john.doe",
            Email = "john@example.com",
            Status = nameof(UserStatus.Active),
            Role = nameof(UserRole.Admin)
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalItems);
        var data = result.Data.ToList();
        Assert.Single(data);
        Assert.Equal("john.doe", data[0].Username);
    }

    [Fact(DisplayName = "Given exact page size users, when Handle called, then total pages is correct")]
    public async Task Given_ExactPageSizeUsers_When_Handle_Then_TotalPagesCorrect()
    {
        var (context, handler) = CreateHandler();

        for (int i = 0; i < 20; i++)
        {
            context.Set<User>().Add(new User
            {
                Id = Guid.NewGuid(),
                Username = $"user{i}",
                Email = $"user{i}@example.com",
                Phone = "999999999",
                Password = "Pwd@123",
                Status = UserStatus.Active,
                Role = UserRole.Customer
            });
        }

        await context.SaveChangesAsync();

        var query = new ListUsersQuery
        {
            Page = 1,
            Size = 10,
            Order = "username",
            Username = null,
            Email = null,
            Status = null,
            Role = null
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(20, result.TotalItems);
        Assert.Equal(2, result.TotalPages);
    }
}