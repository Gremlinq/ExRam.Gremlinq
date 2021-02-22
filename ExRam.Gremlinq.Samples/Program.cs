#define GremlinServer
//#define CosmosDB
//#define AWSNeptune
//#define JanusGraph

using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Samples.Shared;
using Microsoft.Extensions.Logging;

// Put this into static scope to access the default GremlinQuerySource as "g". 
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Samples
{
    public class Program
    {
        private static async Task Main()
        {
            var gremlinQuerySource = g
                .ConfigureEnvironment(env => env //We call ConfigureEnvironment twice so that the logger is set on the environment from now on.
                    .UseLogger(LoggerFactory
                        .Create(builder => builder
                            .AddFilter(__ => true)
                            .AddConsole())
                        .CreateLogger("Queries")))
                .ConfigureEnvironment(env => env
                    //Since the Vertex and Edge classes contained in this sample implement IVertex resp. IEdge,
                    //setting a model is actually not required as long as these classes are discoverable (i.e. they reside
                    //in a currently loaded assembly). We explicitly set a model here anyway.
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())
                        //For CosmosDB, we exclude the 'PartitionKey' property from being included in updates.
                        .ConfigureProperties(model => model
                            .ConfigureElement<Vertex>(conf => conf
                                .IgnoreOnUpdate(x => x.PartitionKey))))
                    //Disable query logging for a noise free console output.
                    //Enable logging by setting the verbosity to anything but None.
                    .ConfigureOptions(options => options
                        .SetValue(WebSocketGremlinqOptions.QueryLogLogLevel, LogLevel.None))

#if GremlinServer
                    .UseGremlinServer(builder => builder
                        .AtLocalhost()));
#elif AWSNeptune
                    .UseNeptune(builder => builder
                        .AtLocalhost()));
#elif CosmosDB
                    .UseCosmosDb(builder => builder
                        .At(new Uri("wss://your_gremlin_endpoint.gremlin.cosmos.azure.com:443/"), "your database name", "your graph name")
                        .AuthenticateBy("your auth key")
                        .ConfigureWebSocket(_ => _
                            .ConfigureGremlinClient(client => client
                                .ObserveResultStatusAttributes((requestMessage, statusAttributes) =>
                                {
                                    //Uncomment to log request charges for CosmosDB.
                                    //if (statusAttributes.TryGetValue("x-ms-total-request-charge", out var requestCharge))
                                    //    env.Logger.LogInformation($"Query {requestMessage.RequestId} had a RU charge of {requestCharge}.");
                                })))));
#elif JanusGraph
                    .UseJanusGraph(builder => builder
                        .AtLocalhost()));
#endif

            await new Logic(gremlinQuerySource, Console.Out)
                .Run();

            Console.Write("Press any key...");
            Console.Read();
        }
    }
}
