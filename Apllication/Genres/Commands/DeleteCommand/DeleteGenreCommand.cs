using Domain.Entities.Genres;
using MediatR;

namespace Apllication.Genres.Commands.DeleteCommand;

public record DeleteGenreCommand(GenreId Id) : IRequest;
