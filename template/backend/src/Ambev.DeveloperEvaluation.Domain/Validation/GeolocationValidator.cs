using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class GeolocationValidator : AbstractValidator<Geolocation>
{
    public GeolocationValidator()
    {
        RuleFor(g => g.Lat)
            .NotEmpty().WithMessage("Latitude is required.");

        RuleFor(g => g.Long)
            .NotEmpty().WithMessage("Longitude is required.");
    }
}