using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City cannot be longer than 100 characters.");

        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(200).WithMessage("Street cannot be longer than 200 characters.");

        RuleFor(a => a.Number)
            .GreaterThan(0)
            .WithMessage("Number must be greater than zero.");

        RuleFor(a => a.ZipCode)
            .NotEmpty().WithMessage("ZipCode is required.")
            .MaximumLength(20).WithMessage("ZipCode cannot be longer than 20 characters.");

        RuleFor(a => a.Geolocation)
            .SetValidator(new GeolocationValidator());
    }
}