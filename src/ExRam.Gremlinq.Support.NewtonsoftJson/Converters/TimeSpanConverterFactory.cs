using Newtonsoft.Json.Linq;
using System.Xml;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TimeSpanConverterFactory : FixedTypeConverterFactory<TimeSpan>
    {
        protected override TimeSpan? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue.Type == JTokenType.String
                ? XmlConvert.ToTimeSpan(jValue.Value<string>()!)
                : jValue.Type == JTokenType.Float
                    ? TimeSpan.FromMilliseconds(jValue.Value<double>())
                    : jValue.Type == JTokenType.Integer
                        ? TimeSpan.FromMilliseconds(jValue.Value<long>())
                        : default(TimeSpan?);
        }
    }
}
