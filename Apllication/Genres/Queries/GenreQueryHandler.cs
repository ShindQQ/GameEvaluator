using Apllication.Common.Interfaces.Repositories;
using Apllication.Common.Mappings;
using Apllication.Common.Models;
using Apllication.Common.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Apllication.Genres.Queries;

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
