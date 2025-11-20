using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty();

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Username)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(u => u.Number)
            .GreaterThan(0);

        RuleFor(u => u.Zipcode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(u => u.Lat)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Long)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(u => u.Status)
            .NotEmpty();

        RuleFor(u => u.Role)
            .NotEmpty();
    }
}