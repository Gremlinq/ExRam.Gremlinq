using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.TEMPLATEPROVIDER;
using ExRam.Gremlinq.Testing.AirRoutes;
using ExRam.Gremlinq.Support.NewtonsoftJson;
#if (provider == "GremlinServer")
using ExRam.Gremlinq.Support.TestContainers;
#endif

namespace ExRam.Gremlinq.Templates.Console
{
    public class Program
    {
        private readonly IGremlinQuerySource _g;

        public Program()
        {
            _g = GremlinQuerySource.g
                .UseTEMPLATEPROVIDER<Vertex, Edge>(configurator => configurator
#if (provider == "Neptune")
                    .At(new Uri("wss://your.neptune.endpoint/"))
                    .UseIAMAuthentication(_ => _
                        .UseSigV4()
                        .WithUri(new Uri("wss://your.neptune.endpoint/"))
                        .WithRegion("us-east-1")
                        .WithAccessKeyId("accessKeyId")
                        .WithSecretAccessKey("secretAccessKey"))
#elif (provider == "CosmosDb")
                    .At(new Uri("wss://your.cosmosdb.endpoint/"))
                    .OnDatabase("your database name")
                    .OnGraph("your graph name")
                    .WithPartitionKey(x => x.PartitionKey!)
                    .AuthenticateBy("your auth key")
#elif (provider == "GremlinServer")
                    .UseGremlinServerModContainer()
#else
                    .AtLocalhost()
#endif
                    .UseNewtonsoftJson());
        }

        public async Task Run()
        {
            /* This call will check for existing routes and airports, so it can safely be called multiple times.
               Also, it can be commented out once it has run on the database */
            await _g
                .CreateAirRoutesSmall();

            /* Let's start out with some simple queries: */

            // Get the number of airports in the database.
            var airports = await _g
                .V<Airport>()
                .ToArrayAsync();

            Console
                .WriteLine($"There are {airports.Length} airports in the database.\n");


            // Let's find out about the airports with a code starting with the letter 'J'
            var airportCodesStartingWithLetterJ = await _g
                .V<Airport>()
                .Where(a => a.Code!.StartsWith("J"))
                .ToArrayAsync();

            airportCodesStartingWithLetterJ
                .WriteToConsole("Here's a list of airports whose code starts with the letter 'J'");


            // Where can we go to from Seattle?
            var destinationsFromSeattle = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .Out<Route>()
                .OfType<Airport>()
                .ToArrayAsync();

            destinationsFromSeattle
                .WriteToConsole("The following airports can be reached from Seattle (SEA)");


            // But which of these have a distance only up to 1500 miles?
            var reachableWithin1500Miles = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .OutE<Route>()
                .Where(r => r.Distance <= 1500)
                .InV<Airport>()
                .ToArrayAsync();

            reachableWithin1500Miles
                .WriteToConsole("The following airports can be reached from Seattle (SEA) within 1500 miles distance");


            // What routes go into SEA?
            var routesIntoSEA = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .In<Route>()
                .OfType<Airport>()
                .ToArrayAsync();

            routesIntoSEA
                .WriteToConsole("There's a number of airports that have routes to Seattle");


            /* Now let's dive into some Gremlinq-operator with sub-queries! */

            // Let's find out all the airports that are reachable from SEA by one or two flights.
            // There might be duplicates, so we take care of that with the Dedup-operator.
            var whithinOneOrTwoFlights = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .Union(
                    __ => __
                        .Out<Route>(),
                    __ => __
                        .Out<Route>()
                        .Out<Route>())
                .OfType<Airport>()
                .Dedup()
                .ToArrayAsync();

            whithinOneOrTwoFlights
                .WriteToConsole("Reachable from SEA with one or two consecutive flights");


            // The same query can be written even shorter:
            await _g
                .V<Airport>()
                .Where(a => a.Code == "JFK")
                .Out<Route>()
                .Union(
                    __ => __,
                    __ => __
                        .Out<Route>())
                .OfType<Airport>()
                .Dedup()
                .ToArrayAsync();


            // Now let's apply a filter defined by a traversal:
            // Which airports reachable from SEA have a subsequent route to Atlanta ?

            var destinationsWithRoutesToAtlanta = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .Out<Route>()
                .OfType<Airport>()
                .Where(__ => __
                    .Out<Route>()
                    .OfType<Airport>()
                    .Where(a => a.Code == "ATL"))
                .ToArrayAsync();

            destinationsWithRoutesToAtlanta
                .WriteToConsole("From SEA, we can go to these airports and connect to ATL from there");


            // Now we're gonna write a couple of queries with some orderings.

            // Order all the airports by their code
            var orderedByCode = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .By(x => x.Code))
                .ToArrayAsync();

            orderedByCode
                .WriteToConsole("Airports, nicely ordered by their code");


            // A somewhat more complex query: Order all airports by their longest route (longest first):
            var orderedByLongestRoute = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance)))
                .ToArrayAsync();

            orderedByLongestRoute
                .WriteToConsole("Airports ordered by their longest route");


            // Order all airports by their longest route (descending), then lexically by their code (ascending)
            var orderedByLongestRouteThenCode = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance))
                    .By(x => x.Code))
                .ToArrayAsync();

            orderedByLongestRouteThenCode
                .WriteToConsole("Airports ordered by their longest route, then by their code");


            // This is how we limit results:
            // Get only the 5 airports with the longest routes. Also, don't get the whole airport
            // but only its description.
            var onlyFiveAirportsOrderedByLongestRoute = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance)))
                .Limit(5)
                .ToArrayAsync();

            onlyFiveAirportsOrderedByLongestRoute
                .WriteToConsole("Airports ordered by their longest route, then by their code");
            // Analogous to Limit, there is also Range, Skip and Tail. Try it out!


            /* Dive into StepLabels: */

            // What airports are reachable from SEA within two hops? To avoid just returning to Seattle,
            // we capture SEA in a StepLabel and filter out our eventual destinations on those that are not SEA!
            var withinTwoFlightsWithNoReturn = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .As((__, sea) => __
                    .Out<Route>()
                    .Out<Route>()
                    .OfType<Airport>()
                    .Where(destination => destination != sea.Value))
                .Dedup()
                .ToArrayAsync();

            withinTwoFlightsWithNoReturn
                .WriteToConsole("We can go, by two flights, from SEA to these airports without returning to SEA");

            /* There's more: Head over to https://docs.gremlinq.net/queries/ to learn about
                - Projections
                - Aggregates
                - Groupings
                - Loops, and
                - Trees
             */
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
