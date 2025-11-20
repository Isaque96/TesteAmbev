using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;

public class UpdateCategoryCommand : IRequest<UpdateCategoryResult>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ValidationResultDetail Validate()
    {
        var validator = new UpdateCategoryCommandValidator();
        var result    = validator.Validate(this);

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors  = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}