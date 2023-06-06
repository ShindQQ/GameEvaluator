using System.ComponentModel;
using System.Globalization;

namespace Domain.Helpers;

public sealed class StronglyTypedIdTypeConverter<T> : TypeConverter
    where T : IStronglyTypedId
{
    public override bool CanConvertFrom(
        ITypeDescriptorContext? context,
        Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value)
    {
        var stringValue = value as string;

        if (Guid.TryParse(stringValue, out var guid))
            return (T)Activator.CreateInstance(typeof(T), new object[] { guid })!;

        return base.ConvertFrom(context, culture, value)!;
    }
}
