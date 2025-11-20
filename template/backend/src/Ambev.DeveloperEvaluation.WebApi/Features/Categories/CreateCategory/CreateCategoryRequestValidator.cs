using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;

/// <summary>
/// Validator for CreateCategoryRequest
/// </summary>
public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateCategoryRequest
    /// </summary>
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("CategoryName name is required")
            .MaximumLength(100)
            .WithMessage("CategoryName name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("CategoryName description is required")
            .MaximumLength(500)
            .WithMessage("CategoryName description must not exceed 500 characters");
    }
}