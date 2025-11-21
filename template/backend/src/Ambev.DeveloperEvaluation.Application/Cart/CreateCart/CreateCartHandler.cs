using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Rebus.Bus;

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

        if (cart.Date == default)
            cart.Date = DateTime.UtcNow;

        var created = await cartRepository.CreateAsync(cart, cancellationToken);

        return mapper.Map<CreateCartResult>(created);
    }
}