using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Comments;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<CommentId>))]
public record CommentId(Guid Value) : IStronglyTypedId;
