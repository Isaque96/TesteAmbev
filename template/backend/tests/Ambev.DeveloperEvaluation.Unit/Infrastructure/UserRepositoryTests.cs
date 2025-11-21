using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class UserRepositoryTests
{
    [Fact(DisplayName = "Given new user, when CreateAsync called, then user is added and saved")]
    public async Task Given_NewUser_When_CreateAsync_Then_UserIsAddedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();

        var user = new User { Id = Guid.NewGuid(), Username = "user1" };
        await repository.CreateAsync(user);

        var saved = await context.Set<User>().FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal(user.Username, saved.Username);
    }

    [Fact(DisplayName = "Given existing user ID, when GetByIdAsync called, then user is returned")]
    public async Task Given_ExistingUserId_When_GetByIdAsync_Then_UserIsReturned()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();

        var user = new User { Id = Guid.NewGuid(), Username = "user1" };
        await TestHelper.SeedDataAsync(context, [user]);

        var result = await repository.GetByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact(DisplayName = "Given user, when UpdateAsync called, then user is updated and saved")]
    public async Task Given_User_When_UpdateAsync_Then_UserIsUpdatedAndSaved()
    {
        var (context, repository) = TestHelper.CreateContextAndRepository<DefaultContext, UserRepository>();

        var user = new User { Id = Guid.NewGuid(), Username = "oldUser" };
        await TestHelper.SeedDataAsync(context, [user]);

        user.Username = "newUser";

        var updated = await repository.UpdateAsync(user);

        var saved = await context.Set<User>().FindAsync(user.Id);

        Assert.Equal(user.Username, saved!.Username);
        Assert.Equal(user, updated);
    }
}