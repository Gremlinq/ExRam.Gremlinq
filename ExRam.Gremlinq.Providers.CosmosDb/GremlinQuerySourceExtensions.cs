using System;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource UseCosmosDb(this IGremlinQuerySource source, Uri uri, string database, string graphName, string authKey)
        {
            return source.ConfigureEnvironment(env => env
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .UseCosmosDbSerializer()
                    .UseCosmosDbExecutor(uri, database, graphName, authKey, env.Logger)
                    .UseCosmosDbDeserializer()));
        }
    }
}
