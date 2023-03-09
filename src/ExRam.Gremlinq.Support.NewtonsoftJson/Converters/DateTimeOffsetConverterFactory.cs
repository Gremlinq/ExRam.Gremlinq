using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeOffsetConverterFactory : FixedTypeConverterFactory<DateTimeOffset>
    {
        protected override DateTimeOffset? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue.Value switch
            {
                DateTime dateTime => new DateTimeOffset(dateTime),
                DateTimeOffset dateTimeOffset => dateTimeOffset,
                _ when jValue.Type == JTokenType.Integer => DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()),
                _ => default(DateTimeOffset?)
            };
        }
    }
}
