using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;

public class UpdateCartHandler(ICartRepository cartRepository, IMapper mapper)
    : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await cartRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existing == null)
            throw new KeyNotFoundException($"Cart with id {command.Id} was not found.");

        existing.UserId = command.UserId;
        existing.Date = command.Date;

        // Estratégia simples: limpa e recria os itens
        existing.ClearCart();
        foreach (var item in command.Products)
            existing.AddProduct(item.ProductId, item.Quantity);

        var updated = await cartRepository.UpdateAsync(existing, cancellationToken);
        return mapper.Map<UpdateCartResult>(updated);
    }
}