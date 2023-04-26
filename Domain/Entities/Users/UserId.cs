using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Users;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<UserId>))]
public record UserId(Guid Value) : IStronglyTypedId;
