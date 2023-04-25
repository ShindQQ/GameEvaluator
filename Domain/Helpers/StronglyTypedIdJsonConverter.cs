using Newtonsoft.Json;

namespace Domain.Helpers;

public sealed class StronglyTypedIdJsonConverter<T> : JsonConverter
    where T : IStronglyTypedId
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == typeof(T);

    public override void WriteJson(
        JsonWriter writer,
        object value,
        JsonSerializer serializer)
    {
        var id = (T)value;
        serializer.Serialize(writer, id.Value);
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<Guid>(reader);

        return (T)Activator.CreateInstance(typeof(T), new object[] { guid })!;
    }
}
