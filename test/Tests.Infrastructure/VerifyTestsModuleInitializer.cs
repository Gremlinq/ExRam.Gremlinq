using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

using Argon;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Process.Traversal;

internal static class VerifyTestsModuleInitializer
{
    private sealed class TreeConverter : JsonConverter
    {
        public override bool CanConvert(Type type) => typeof(ITree).IsAssignableFrom(type);

        public override object? ReadJson(JsonReader reader, Type type, object? existingValue, JsonSerializer serializer) => throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => WriteJsonEx(writer, (dynamic)value, serializer);

        private static void WriteJsonEx<TKey, TValue>(JsonWriter writer, Tree<TKey, TValue> value, JsonSerializer serializer)
            where TKey : notnull
            where TValue : ITree => serializer.Serialize(writer, value.Select(kvp => new { kvp.Key, kvp.Value }).ToArray());

        private static void WriteJsonEx<TKey>(JsonWriter writer, Tree<TKey> value, JsonSerializer serializer)
            where TKey : notnull => serializer.Serialize(writer, value.Select(kvp => new { kvp.Key, kvp.Value }).ToArray());
    }

    private sealed class KeyConverter : WriteOnlyJsonConverter<Key>
    {
        public override void Write(VerifyJsonWriter writer, Key value) => writer.WriteValue(ToText(value));
    }

    private sealed class KeyTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) => value is Key key && destinationType == typeof(string)
            ? ToText(key)
            : base.ConvertTo(context, culture, value, destinationType);
    }

    [ModuleInitializer]
    internal static void Init()
    {
        TypeDescriptor.AddAttributes(typeof(Key), new TypeConverterAttribute(typeof(KeyTypeConverter)));

        VerifierSettings
            .AddExtraSettings(settings =>
            {
                settings.Converters.Add(new TreeConverter());
                settings.Converters.Add(new KeyConverter());
            });
    }

    private static string ToText(Key key) => key.RawKey is T t
        ? $"{t.EnumName}.{t.EnumValue}"
        : (string)key.RawKey;
}
