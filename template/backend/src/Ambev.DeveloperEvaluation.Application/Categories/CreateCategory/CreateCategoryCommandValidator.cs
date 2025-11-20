using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500);
    }
}