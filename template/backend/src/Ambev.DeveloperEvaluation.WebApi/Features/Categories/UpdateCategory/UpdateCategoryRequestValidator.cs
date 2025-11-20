using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;

/// <summary>
/// Validator for UpdateCategoryRequest
/// </summary>
public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateCategoryRequest
    /// </summary>
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("CategoryName ID is required");

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