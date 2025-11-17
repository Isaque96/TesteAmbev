using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class RatingValidator : AbstractValidator<Rating>
{
    public RatingValidator()
    {
        RuleFor(r => r.Rate)
            .InclusiveBetween(0, 5)
            .WithMessage("Rate must be between 0 and 5.");

        RuleFor(r => r.Count)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Count cannot be negative.");
    }
}