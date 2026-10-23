using System.Globalization;

using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class TinkerPop38Serializers
    {
        public static readonly IGraphSONSerializer OffsetDateTime = new OffsetDateTimeSerializer();

        // Writes a DateTimeOffset in the form of TinkerPop's IO reference sample, 2007-12-03T10:15:30+01:00:
        // seconds always, a fraction of a second only when there is one, and the offset, +00:00 for UTC.
        private sealed class OffsetDateTimeSerializer : IGraphSONSerializer
        {
            private const string Format = "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFzzz";

            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer) => GraphSONUtil.ToTypedValue("OffsetDateTime", ((DateTimeOffset)objectData).ToString(Format, CultureInfo.InvariantCulture), "gx");
        }
    }
}
