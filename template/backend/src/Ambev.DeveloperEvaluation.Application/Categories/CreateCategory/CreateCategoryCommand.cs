using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;

public class CreateCategoryCommand : IRequest<CreateCategoryResult>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ValidationResultDetail Validate()
    {
        var validator = new CreateCategoryCommandValidator();
        var result    = validator.Validate(this);

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors  = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}