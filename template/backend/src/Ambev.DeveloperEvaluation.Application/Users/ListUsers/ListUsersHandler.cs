using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<ListUsersQuery, ListUsersResult>
{
    public async Task<ListUsersResult> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalItems) = await userRepository.GetUsersPaginatedAsync(
            request.Page,
            request.Size,
            request.Order,
            request.Username,
            request.Email,
            request.Status,
            request.Role,
            cancellationToken
        );

        var data = mapper.Map<IEnumerable<ListUsersItem>>(users);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);

        return new ListUsersResult
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }
}