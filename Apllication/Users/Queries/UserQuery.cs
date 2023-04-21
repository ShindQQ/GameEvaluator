using Domain.Entities.Users;
using MediatR;

namespace Aplliction.Users.Queries;

public record UserQuery(UserId Id) : IRequest<List<User>>;
