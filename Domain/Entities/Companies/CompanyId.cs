using Domain.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Domain.Entities.Companies;

[JsonConverter(typeof(StronglyTypedIdJsonConverter<CompanyId>))]
[TypeConverter(typeof(StronglyTypedIdTypeConverter<CompanyId>))]
public record CompanyId(Guid Value) : IStronglyTypedId;
