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
            return jValue switch
            {
                { Type: JTokenType.String } => XmlConvert.ToTimeSpan(jValue.Value<string>()!),
                { Type: JTokenType.Float } => TimeSpan.FromMilliseconds(jValue.Value<double>()),
                { Type: JTokenType.Integer } => TimeSpan.FromMilliseconds(jValue.Value<long>()),
                _ => default(TimeSpan?)
            };
        }
    }
}
