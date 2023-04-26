using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Platforms;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<PlatformId>))]
public record PlatformId(Guid Value) : IStronglyTypedId;
