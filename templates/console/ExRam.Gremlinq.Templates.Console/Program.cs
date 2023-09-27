using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Templates.Console
{
    public class Program
    {
        private readonly IGremlinQuerySource _g;

        public Program()
        {
            var _g = g
#if ProviderIsGremlinServer
                .UseGremlinServer<Vertex, Edge>(conf => conf
                    .AtLocalhost())
#elif ProviderIsNeptune
                .UseNeptune<Vertex, Edge>(conf => conf
                    .At(new Uri("wss://your.neptune.endpoint/")))
#elif ProviderIsCosmosDb
                .UseCosmosDb<Vertex, Edge>(conf => conf
                    .At(new Uri("wss://your.cosmosdb.endpoint/"))
                    .OnDatabase("your database name")
                    .OnGraph("your graph name")
                    .AuthenticateBy("your auth key")
                    .WithPartitionKey(x => x.PartitionKey))
#elif ProviderIsJanusGraph
                .UseJanusGraph<Vertex, Edge>(conf => conf
                    .AtLocalhost()))
#endif
                .ConfigureEnvironment(env => env
                    .UseNewtonsoftJson())
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

            Console.WriteLine($"There are {petCount} pets in the database.");
        }

        private static async Task Main()
        {
            var program = new Program();

            await program.Run();

            Console.Write("Press any key...");
            Console.Read();
        }
    }
}
