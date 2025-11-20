using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500);
    }
}