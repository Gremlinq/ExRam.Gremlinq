using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Globalization;
using static System.Globalization.DateTimeStyles;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeOffsetConverterFactory : FixedTypeConverterFactory<DateTimeOffset>
    {
        protected override DateTimeOffset? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue switch
            {
                { Value: DateTimeOffset dateTimeOffset } => dateTimeOffset,
                { Value: DateTime dateTime } => new DateTimeOffset(dateTime),
                { Value: string dateTimeString } when DateTimeOffset.TryParse(dateTimeString, CultureInfo.InvariantCulture, AdjustToUniversal | AssumeLocal, out var parseResult) => parseResult,
                { Type: JTokenType.Integer } => DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()),
                _ => default(DateTimeOffset?)
            };
        }
    }
}

