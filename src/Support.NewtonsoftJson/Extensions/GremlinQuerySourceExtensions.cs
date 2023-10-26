using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource UseNewtonsoftJson(this IGremlinQuerySource source)
        {
            return source
                .ConfigureEnvironment(env => env
                    .ConfigureDeserializer(deserializer => deserializer
                        .UseNewtonsoftJson()));
        }
    }
}
