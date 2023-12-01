using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
#if (provider == "GremlinServer")
using ExRam.Gremlinq.Providers.GremlinServer;
#elif (provider == "Neptune")
using ExRam.Gremlinq.Providers.Neptune;
#elif (provider == "CosmosDb")
using ExRam.Gremlinq.Providers.CosmosDb;
#elif (provider == "JanusGraph")
using ExRam.Gremlinq.Providers.JanusGraph;
#endif
using ExRam.Gremlinq.Support.NewtonsoftJson;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Templates.Console
{
    public class Program
    {
        private readonly IGremlinQuerySource _g;

        public Program()
        {
            _g = g
#if (provider == "GremlinServer")
                .UseGremlinServer<Vertex, Edge>(configurator => configurator
                    .AtLocalhost())
#elif (provider == "Neptune")
                .UseNeptune<Vertex, Edge>(configurator => configurator
                    .At(new Uri("wss://your.neptune.endpoint/")))
#elif (provider == "CosmosDb")
                .UseCosmosDb<Vertex, Edge>(configurator => configurator
                    .At(new Uri("wss://your.cosmosdb.endpoint/"))
                    .OnDatabase("your database name")
                    .OnGraph("your graph name")
                    .WithPartitionKey(x => x.PartitionKey)
                    .AuthenticateBy("your auth key"))
#elif (provider == "JanusGraph")
                .UseJanusGraph<Vertex, Edge>(configurator => configurator
                    .AtLocalhost())
#endif
                .UseNewtonsoftJson();
        }

        public async Task Run()
        {
            var petCount = await _g
                .V<Pet>()
                .Count()
                .FirstAsync();

            /***** Uncomment to actually add Jenny and her cat to the database
            var jenny = await _g
                .AddV(new Person { Name = "Jenny", Age = 41 })
                .SideEffect(__ => __
                    .AddE<Owns>()
                    .To(__ => __
                        .AddV(new Cat() { Name = "Catman John" })))
                .FirstAsync();
            ********/

            System.Console.WriteLine($"There are {petCount} pets in the database.");
        }

        private static async Task Main()
        {
            var program = new Program();

            await program.Run();

            System.Console.Write("Press any key...");
            System.Console.Read();
        }
    }
}
