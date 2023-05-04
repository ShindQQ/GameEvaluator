using Domain.Entities.Genres;
using MediatR;

namespace Application.Genres.Commands.DeleteCommand;

public record DeleteGenreCommand(GenreId Id) : IRequest;
