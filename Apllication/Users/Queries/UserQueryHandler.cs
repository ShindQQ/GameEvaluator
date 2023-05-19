using Aplliction.Users.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed class UserQueryHandler : IRequestHandler<UserQuery, PaginatedList<UserDto>>
{
    private readonly IUserRepository _repository;

    private readonly IMapper _mapper;

    public UserQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserDto>> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var queryable = (await _repository.GetAsync())
             .Include(user => user.UserGames)
                 .ThenInclude(game => game.Game)
                    .ThenInclude(game => game.Platforms)
             .Include(user => user.UserGames)
                 .ThenInclude(game => game.Game)
                    .ThenInclude(game => game.Genres)
             .Include(user => user.UserGames)
                 .ThenInclude(game => game.Game)
                    .ThenInclude(game => game.Companies)
             .Include(user => user.UserGames)
                 .ThenInclude(game => game.Game)
                    .ThenInclude(game => game.Comments)
                        .ThenInclude(comment => comment.ChildrenComments)
                           .ThenInclude(comment => comment.User)
             .Include(user => user.UserGames)
                 .ThenInclude(game => game.Game)
                    .ThenInclude(game => game.Comments)
                           .ThenInclude(comment => comment.User)
             .Include(user => user.Roles)
             .Include(user => user.Company)
             .Include(user => user.BanState)
             .Include(user => user.Comments)
                .ThenInclude(comment => comment.ChildrenComments)
                    .ThenInclude(comment => comment.User)
             .Include(user => user.Comments)
             .AsQueryable();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        var d = queryable.ToArray(); // shit, but it works

        var dtos = queryable.Select(user => new UserDto
        {
            Comments = user.Comments.Select(comment => CommentResolver.MapCommentToCommentDto(comment)!).ToList(),
            Games = GameResolver.IEnumerableMapGamesToGameDtos(user.UserGames.Select(userGame => userGame.Game))!.ToList(),
            Id = user.Id.Value,
            Roles = user.Roles.Select(genre => genre.Name).ToList(),
            Banned = user.BanState == null ? false : user.BanState.IsBaned,
            BannedAt = user.BanState == null ? default : user.BanState.BannedTo,
            BannedTo = user.BanState == null ? default : user.BanState.BannedAt,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Company = user.Company == null ? string.Empty : user.Company.Name,
        });

        return await dtos
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
