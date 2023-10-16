using System.Xml;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TimeSpanConverterFactory : FixedTypeConverterFactory<TimeSpan>
    {
        protected override TimeSpan? Convert(object jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            return jValue switch
            {
                string stringValue => XmlConvert.ToTimeSpan(stringValue),
                double doubleValue => TimeSpan.FromMilliseconds(doubleValue),
                int intValue => TimeSpan.FromMilliseconds(intValue),
                long longValue => TimeSpan.FromMilliseconds(longValue),
                _ => default(TimeSpan?)
            };
        }
    }
}
