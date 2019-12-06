namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource UseCosmosDb(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443)
        {
            return source.ConfigureEnvironment(env => env
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .UseCosmosDbSerializer()
                    .UseCosmosDbExecutor(hostname, database, graphName, authKey, env.Logger, port)
                    .UseCosmosDbDeserializer()));
        }

        public static IGremlinQuerySource UseCosmosDbEmulator(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 8901)
        {
            return source.ConfigureEnvironment(env => env
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .UseCosmosDbSerializer()
                    .UseCosmosDbEmulatorExecutor(hostname, database, graphName, authKey, env.Logger, port)
                    .UseCosmosDbDeserializer()));
        }
    }
}
