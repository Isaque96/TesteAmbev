using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with id {command.Id} was not found.");

        // Campos simples
        user.Email = command.Email;
        user.Username = command.Username;
        user.Password = command.Password;
        user.Phone = command.Phone;
        if (Enum.TryParse(command.Status, out UserStatus status))
            user.Status = status;
        if  (Enum.TryParse(command.Role, out UserRole role))
            user.Role = role;

        // Nome
        user.Name.FirstName = command.FirstName;
        user.Name.LastName  = command.LastName;

        // Endereço
        user.Address.City = command.City;
        user.Address.Street = command.Street;
        user.Address.Number = command.Number;
        user.Address.ZipCode = command.Zipcode;
        user.Address.Geolocation.Lat = command.Lat;
        user.Address.Geolocation.Long = command.Long;

        var updated = await userRepository.UpdateAsync(user, cancellationToken);

        return mapper.Map<UpdateUserResult>(updated);
    }
}