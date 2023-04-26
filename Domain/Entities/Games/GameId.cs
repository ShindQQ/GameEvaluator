using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Games;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<GameId>))]
public record GameId(Guid Value) : IStronglyTypedId;
