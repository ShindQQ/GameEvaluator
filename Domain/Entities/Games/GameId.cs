using Domain.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Domain.Entities.Games;

[JsonConverter(typeof(StronglyTypedIdJsonConverter<GameId>))]
[TypeConverter(typeof(StronglyTypedIdTypeConverter<GameId>))]
public record GameId(Guid Value) : IStronglyTypedId;
