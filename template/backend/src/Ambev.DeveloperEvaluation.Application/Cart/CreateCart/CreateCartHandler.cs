using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.CreateCart;

public class CreateCartHandler(ICartRepository cartRepository, IMapper mapper)
    : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = mapper.Map<Domain.Entities.Cart>(command);

        // garante Date + CreatedAt se não vier nada
        if (cart.Date == default)
            cart.Date = DateTime.UtcNow;

        var created = await cartRepository.CreateAsync(cart, cancellationToken);

        return mapper.Map<CreateCartResult>(created);
    }
}