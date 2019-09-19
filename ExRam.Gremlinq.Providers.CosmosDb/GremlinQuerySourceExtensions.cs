namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IConfigurableGremlinQuerySource UseCosmosDb(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443)
        {
            return source
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .UseCosmosDbSerializer()
                    .UseCosmosDbExecutor(hostname, database, graphName, authKey, source.Logger, port)
                    .UseCosmosDbDeserializer());
        }

        public static IConfigurableGremlinQuerySource UseCosmosDbEmulator(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 8901)
        {
            return source
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .UseCosmosDbSerializer()
                    .UseCosmosDbEmulatorExecutor(hostname, database, graphName, authKey, source.Logger, port)
                    .UseCosmosDbDeserializer());
        }
    }
}
