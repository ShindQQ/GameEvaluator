using Domain.Entities.Games;
using MediatR;

namespace Application.Games.Commands.DeleteCommand;

public record DeleteGameCommand(GameId Id) : IRequest;
