using Domain.Entities.Games;
using MediatR;

namespace Apllication.Games.Commands.DeleteCommand;

public record DeleteGameCommand(GameId Id) : IRequest;
