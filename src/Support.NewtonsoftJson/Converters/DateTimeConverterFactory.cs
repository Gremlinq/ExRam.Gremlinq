using System.Globalization;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeConverterFactory : FixedTypeConverterFactory<DateTime>
    {
        protected override DateTime? Convert(object jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue switch
            {
                DateTime dateTime => dateTime,
                DateTimeOffset dateTimeOffset => dateTimeOffset.UtcDateTime,
                string dateTimeString when DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parseResult) => parseResult,
                int intValue => new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(intValue).Ticks, DateTimeKind.Utc),
                long longValue => new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(longValue).Ticks, DateTimeKind.Utc),
                double doubleValue => new DateTime(DateTimeOffset.FromUnixTimeMilliseconds((long)doubleValue).Ticks, DateTimeKind.Utc),
                _ => default(DateTime?)
            };
        }
    }
}
