using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Category;

public class CreateCategoryCommandValidatorTests
{
    [Fact(DisplayName = "Given valid command, when validated, then it is valid")]
    public async Task Given_ValidCommand_When_Validate_Then_Valid()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Books",
            Description = "Some description"
        };

        var validator = new CreateCategoryCommandValidator();

        var result = await validator.ValidateAsync(command);

        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Given empty name, when validated, then it is invalid")]
    public async Task Given_EmptyName_When_Validate_Then_Invalid()
    {
        var command = new CreateCategoryCommand
        {
            Name = string.Empty,
            Description = "Some description"
        };

        var validator = new CreateCategoryCommandValidator();

        var result = await validator.ValidateAsync(command);

        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Given too long fields, when validated, then it is invalid")]
    public async Task Given_TooLongFields_When_Validate_Then_Invalid()
    {
        var command = new CreateCategoryCommand
        {
            Name = new string('x', 101),
            Description = new string('x', 501)
        };

        var validator = new CreateCategoryCommandValidator();

        var result = await validator.ValidateAsync(command);

        Assert.False(result.IsValid);
    }
}