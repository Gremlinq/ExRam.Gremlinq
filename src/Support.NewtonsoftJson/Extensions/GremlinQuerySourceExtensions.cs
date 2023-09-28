namespace ExRam.Gremlinq.Core
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
