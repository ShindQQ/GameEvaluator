using Domain.Helpers;
using System.ComponentModel;

namespace Domain.Entities.Companies;

[TypeConverter(typeof(StronglyTypedIdTypeConverter<CompanyId>))]
public record CompanyId(Guid Value) : IStronglyTypedId;
