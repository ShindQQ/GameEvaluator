using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Genres.Queries;

public sealed class GenreQueryHandler : IRequestHandler<GenreQuery, PaginatedList<GenreDto>>
{
    private readonly IGenreRepository _repository;

    private readonly IMapper _mapper;

    public GenreQueryHandler(IGenreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GenreDto>> Handle(GenreQuery request, CancellationToken cancellationToken)
    {
        var queryable = await _repository.GetAsync();

        if (request.Id is not null)
            queryable = queryable.Where(entity => entity.Id == request.Id);

        return await queryable
            .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
