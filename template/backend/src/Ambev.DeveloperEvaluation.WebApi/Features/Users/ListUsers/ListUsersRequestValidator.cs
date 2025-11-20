using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersRequestValidator : AbstractValidator<ListUsersRequest>
{
    public ListUsersRequestValidator()
    {
        // Paginação – segue o padrão que tínhamos falado
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.Order)
            .Must(o => o is "ASC" or "DESC")
            .WithMessage("Order must be 'ASC' or 'DESC'.");

        // Filtros específicos
        RuleFor(x => x.Name)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Email)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}