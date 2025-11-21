using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Events;

public class UserRegisteredEventTests
{
    [Fact(DisplayName = "Placeholder test for UserRegisteredEvent")]
    public void PlaceholderTest()
    {
        var userEvent = new UserRegisteredEvent(new User());

        Assert.NotNull(userEvent);
        Assert.NotNull(userEvent.User);
    }
}