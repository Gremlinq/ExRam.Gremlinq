using System.Runtime.CompilerServices;

using Argon;

using ExRam.Gremlinq.Core;

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

    [ModuleInitializer]
    internal static void Init() => VerifierSettings
        .AddExtraSettings(settings =>
        {
            settings.Converters.Add(new TreeConverter());
        });
}
