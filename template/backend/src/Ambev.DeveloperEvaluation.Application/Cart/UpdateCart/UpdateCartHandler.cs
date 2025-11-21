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

        DealingWithCartItems(command, existing);

        var updated = await cartRepository.UpdateAsync(existing, cancellationToken);
        return mapper.Map<UpdateCartResult>(updated);
    }

    private static void DealingWithCartItems(UpdateCartCommand command, Domain.Entities.Cart existing)
    {
        var productIdsInCommand = command.Products.Select(p => p.ProductId).ToHashSet();
        var itemsToRemove = existing.CartItems
            .Where(ci => !productIdsInCommand.Contains(ci.ProductId))
            .ToList();

        foreach (var item in itemsToRemove)
            existing.RemoveProduct(item.ProductId);

        foreach (var itemCommand in command.Products)
            existing.AddProduct(itemCommand.ProductId, itemCommand.Quantity);
    }
}