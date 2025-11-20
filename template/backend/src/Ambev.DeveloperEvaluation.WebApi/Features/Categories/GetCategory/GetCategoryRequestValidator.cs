using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;

/// <summary>
/// Validator for GetCategoryRequest
/// </summary>
public class GetCategoryRequestValidator : AbstractValidator<GetCategoryRequest>
{
    /// <summary>
    /// Initializes validation rules for GetCategoryRequest
    /// </summary>
    public GetCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("CategoryName ID is required");
    }
}