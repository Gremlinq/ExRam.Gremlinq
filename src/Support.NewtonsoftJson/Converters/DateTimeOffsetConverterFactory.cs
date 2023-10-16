using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeOffsetConverterFactory : FixedTypeConverterFactory<DateTimeOffset>
    {
        protected override DateTimeOffset? Convert(object jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue switch
            {
                DateTime dateTime => new DateTimeOffset(dateTime),
                DateTimeOffset dateTimeOffset => dateTimeOffset,
                int intValue => DateTimeOffset.FromUnixTimeMilliseconds(intValue),
                long longValue => DateTimeOffset.FromUnixTimeMilliseconds(longValue),
                double doubleValue => DateTimeOffset.FromUnixTimeMilliseconds((long)doubleValue),
                _ => default(DateTimeOffset?)
            };
        }
    }
}
