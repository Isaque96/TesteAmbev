using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class NameValidator : AbstractValidator<Name>
{
    public NameValidator()
    {
        RuleFor(n => n.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("First name cannot be longer than 100 characters.");

        RuleFor(n => n.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Last name cannot be longer than 100 characters.");
    }
}