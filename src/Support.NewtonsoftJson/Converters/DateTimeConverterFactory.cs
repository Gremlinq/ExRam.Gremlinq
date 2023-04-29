using Newtonsoft.Json.Linq;
using System.Globalization;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeConverterFactory : FixedTypeConverterFactory<DateTime>
    {
        protected override DateTime? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue.Value switch
            {
                DateTime dateTime => dateTime,
                DateTimeOffset dateTimeOffset => dateTimeOffset.UtcDateTime,
                string dateTimeString when DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parseResult) => parseResult,
                _ when jValue.Type == JTokenType.Integer => new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()).Ticks, DateTimeKind.Utc),
                _ => default(DateTime?)
            };
        }
    }
}
