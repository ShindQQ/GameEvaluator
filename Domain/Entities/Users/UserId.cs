using Domain.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Domain.Entities.Users;

[JsonConverter(typeof(StronglyTypedIdJsonConverter<UserId>))]
[TypeConverter(typeof(StronglyTypedIdTypeConverter<UserId>))]
public record UserId(Guid Value) : IStronglyTypedId;
