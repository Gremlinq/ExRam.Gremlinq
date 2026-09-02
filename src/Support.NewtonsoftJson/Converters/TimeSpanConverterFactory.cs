using Newtonsoft.Json.Linq;
using System.Xml;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TimeSpanConverterFactory : FixedTypeConverterFactory<TimeSpan>
    {
        protected override TimeSpan? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse) => jValue switch
        {
            { Type: JTokenType.String } => FromXmlDuration(jValue.Value<string>()!),
            { Type: JTokenType.Float } => TimeSpan.FromMilliseconds(jValue.Value<double>()),
            { Type: JTokenType.Integer } => TimeSpan.FromMilliseconds(jValue.Value<long>()),
            _ => null
        };

        // XmlConvert has no Try-variant, and a string that isn't an ISO 8601 duration must make
        // this converter decline, not throw. An exception escaping here fails the whole result set.
        private static TimeSpan? FromXmlDuration(string value)
        {
            try
            {
                return XmlConvert.ToTimeSpan(value);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
