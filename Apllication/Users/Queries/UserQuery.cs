using Apllication.Common.Models;
using Domain.Entities.Users;
using MediatR;

namespace Aplliction.Users.Queries;

public record UserQuery : IRequest<PaginatedList<UserDto>>
{
    public UserId? Id { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
