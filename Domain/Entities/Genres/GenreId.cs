using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Genres;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<GenreId>))]
public record GenreId(Guid Value) : IStronglyTypedId;
