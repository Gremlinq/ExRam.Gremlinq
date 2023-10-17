using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DateTimeConverterFactory : FixedTypeConverterFactory<DateTime>
    {
        protected override DateTime? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue switch
            {
                { Value: DateTime dateTime } => dateTime,
                _ when recurse.TryTransform<JToken, DateTimeOffset>(jValue, environment, out var dateTimeOffset) => dateTimeOffset.UtcDateTime,
                _ => default(DateTime?)
            };
        }
    }
}
