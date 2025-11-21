using System.Globalization;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class GeolocationValidator : AbstractValidator<Geolocation>
{
    public GeolocationValidator()
    {
        RuleFor(g => g.Lat)
            .NotEmpty().WithMessage("Latitude is required.")
            .Must(BeAValidLatitude).WithMessage("Latitude must be a valid decimal number between -90 and 90 with up to 6 decimal places.");

        RuleFor(g => g.Long)
            .NotEmpty().WithMessage("Longitude is required.")
            .Must(BeAValidLongitude).WithMessage("Longitude must be a valid decimal number between -180 and 180 with up to 6 decimal places.");
    }

    private static bool BeAValidLatitude(string lat)
    {
        if (!decimal.TryParse(lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return false;
        
        if (value is < -90 or > 90) return false;

        // Verifica até 6 casas decimais
        int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        return decimalPlaces <= 6;
    }

    private static bool BeAValidLongitude(string lon)
    {
        if (!decimal.TryParse(lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return false;
        
        if (value is < -180 or > 180) return false;

        // Verifica até 6 casas decimais
        int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        return decimalPlaces <= 6;
    }
}