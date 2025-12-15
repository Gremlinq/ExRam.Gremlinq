using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.TEMPLATEPROVIDER;
using ExRam.Gremlinq.Testing.AirRoutes;
using ExRam.Gremlinq.Support.NewtonsoftJson;
#if (useTestContainers)
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
#elif (provider == "GremlinServer" && useTestContainers)
                    .UseGremlinServerModContainer()
#else
                    .AtLocalhost()
#endif
                    .UseNewtonsoftJson());
        }

        public async Task Run()
        {
#if (true)  // --8<-- [start:createAirRoutes]
            /* This call will check for existing routes and airports, so it can safely be called multiple times.
               Also, it can be commented out once it has run on the database */
            await _g
                .CreateAirRoutesSmall();
#endif      // --8<-- [end:createAirRoutes]

            /* Let's start out with some simple queries: */

            // Retrieve all airports from the database.
#if (true)  // --8<-- [start:allAirports]
            var airports = await _g
                .V<Airport>()
                .ToArrayAsync();

            airports
                .WriteToConsole("Here's a list of all the airports.");
#endif      // --8<-- [end:allAirports]

            // Get the number of airports in the database.
#if (true)  // --8<-- [start:howManyAirports]
            var airportCount = await _g
                .V<Airport>()
                .Count()
                .FirstAsync();

            Console
                .WriteLine($"There are {airportCount} airports in the database.\n");
#endif      // --8<-- [end:howManyAirports]

#if (true)  // --8<-- [start:codeStartingWithJ]
            // Let's find out about the airports with a code starting with the letter 'J'
            var airportCodesStartingWithLetterJ = await _g
                .V<Airport>()
                .Where(airport => airport.Code!.StartsWith("J"))
                .ToArrayAsync();

            airportCodesStartingWithLetterJ
                .WriteToConsole("Here's a list of airports whose code starts with the letter 'J'");
#endif      // --8<-- [end:codeStartingWithJ]

            // Where can we go to from Seattle?
#if (true)  // --8<-- [start:destinationsFromSEA]
            var destinationsFromSeattle = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .Out<Route>()
                .OfType<Airport>()
                .ToArrayAsync();

            destinationsFromSeattle
                .WriteToConsole("The following airports can be reached from Seattle (SEA)");
#endif      // --8<-- [end:destinationsFromSEA]

            // But which of these have a distance only up to 1500 miles?
#if (true)  // --8<-- [start:within1500Miles]
            var reachableWithin1500Miles = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .OutE<Route>()
                .Where(route => route.Distance <= 1500)
                .InV<Airport>()
                .ToArrayAsync();

            reachableWithin1500Miles
                .WriteToConsole("The following airports can be reached from Seattle (SEA) within 1500 miles distance");
#endif      // --8<-- [end:within1500Miles]

            // What routes go into SEA?
#if (true)  // --8<-- [start:departuresToSEA]
            var routesIntoSEA = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .In<Route>()
                .OfType<Airport>()
                .ToArrayAsync();

            routesIntoSEA
                .WriteToConsole("There's a number of airports that have routes to Seattle");
#endif      // --8<-- [end:departuresToSEA]

            // What airports  are reachable from SEA by taking two flights?
#if (true)  // --8<-- [start:twoFlights]
            var twoFlightsFromSeattle = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .Out<Route>()
                .OfType<Airport>()
                .ToArrayAsync();

            twoFlightsFromSeattle
                .WriteToConsole("The following airports can be reached from Seattle (SEA) by taking two flights");
#endif      // --8<-- [end:twoFlights]

            /* Now let's dive into some Gremlinq-operators with sub-queries! */

#if (true)  // --8<-- [start:oneOrTwoFlights]
            // Let's find out all the airports that are reachable from SEA by one or two flights.
            // There might be duplicates, so we take care of that with the Dedup-operator.
            var withinOneOrTwoFlights = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .Union(
                    __ => __
                        .Out<Route>(),
                    __ => __
                        .Out<Route>()
                        .Out<Route>())
                .OfType<Airport>()
                .Dedup()
                .ToArrayAsync();

            withinOneOrTwoFlights
                .WriteToConsole("Reachable from SEA with one or two consecutive flights");
#endif      // --8<-- [end:oneOrTwoFlights]

            // The same query can be written even shorter:
#if (true)  // --8<-- [start:simplerOneOrTwoFlights]
            await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .Out<Route>()
                .Union(
                    __ => __,
                    __ => __
                        .Out<Route>())
                .OfType<Airport>()
                .Dedup()
                .ToArrayAsync();
#endif      // --8<-- [end:simplerOneOrTwoFlights]

            // Now let's apply a filter defined by a traversal:
            // Which airports reachable from SEA have a subsequent route to Atlanta ?
#if (true)  // --8<-- [start:SEATwoHopsToAtlanta]
            var destinationsWithRoutesToAtlanta = await _g
                .V<Airport>()
                .Where(airport => airport.Code == "SEA")
                .Out<Route>()
                .OfType<Airport>()
                .Where(__ => __
                    .Out<Route>()
                    .OfType<Airport>()
                    .Where(airport => airport.Code == "ATL"))
                .ToArrayAsync();

            destinationsWithRoutesToAtlanta
                .WriteToConsole("From SEA, we can go to these airports and connect to ATL from there");
#endif      // --8<-- [end:SEATwoHopsToAtlanta]

            // Now we're gonna write a couple of queries with some orderings.

            // Order all the airports by their code
#if (true)  // --8<-- [start:airportsOrderedByCode]
            var orderedByCode = await _g
                .V<Airport>()
                .Order(o => o
                    .By(airport => airport.Code))
                .ToArrayAsync();

            orderedByCode
                .WriteToConsole("Airports, nicely ordered by their code");
#endif      // --8<-- [end:airportsOrderedByCode]

            // A somewhat more complex query: Order all airports by their longest route (longest first):
#if (true)  // --8<-- [start:airportsByLongestRoute]
            var orderedByLongestRoute = await _g
                .V<Airport>()
                .Order(o => o
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(route => route.Distance))
                        .Values(route => route.Distance)))
                .ToArrayAsync();

            orderedByLongestRoute
                .WriteToConsole("Airports ordered by their longest route");
#endif      // --8<-- [end:airportsByLongestRoute]

            // Order all airports by their longest route (descending), then lexically by their code (ascending)
#if (true)  // --8<-- [start:airportsByLongestRouteAndCode]
            var orderedByLongestRouteThenCode = await _g
                .V<Airport>()
                .Order(o => o
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(route => route.Distance))
                        .Values(route => route.Distance))
                    .By(airport => airport.Code))
                .ToArrayAsync();

            orderedByLongestRouteThenCode
                .WriteToConsole("Airports ordered by their longest route, then by their code");
#endif      // --8<-- [end:airportsByLongestRouteAndCode]

            // This is how we limit results:
            // Get only the 5 airports with the longest routes.
#if (true)  // --8<-- [start:firstFiveAirports]
            var onlyFiveAirportsOrderedByLongestRoute = await _g
                .V<Airport>()
                .Order(o => o
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(route => route.Distance))
                        .Values(route => route.Distance)))
                .Limit(5)
                .ToArrayAsync();

            onlyFiveAirportsOrderedByLongestRoute
                .WriteToConsole("Top 5 airports with the longest routes");
#endif      // --8<-- [end:firstFiveAirports]

#if (true)  // --8<-- [start:fiveAirportsWithRange]
            var results = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance)))
                .Range(0, 5)
                .ToArrayAsync();
#endif      // --8<-- [end:fiveAirportsWithRange]

#if (true)  // --8<-- [start:skipFirstFiveResults]
            var skipFirstFiveResults = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance)))
                .Skip(5)
                .ToArrayAsync();
#endif      // --8<-- [end:fiveAirportsWithRange]

#if (true)  // --8<-- [start:tailFiveResults]
            var tailFiveResults = await _g
                .V<Airport>()
                .Order(orderBuilder => orderBuilder
                    .ByDescending(__ => __
                        .OutE<Route>()
                        .Order(o => o
                            .ByDescending(x => x.Distance))
                        .Values(x => x.Distance)))
                .Tail(5)
                .ToArrayAsync();
#endif      // --8<-- [end:tailFiveResults]


            /* Dive into StepLabels: */

            // What airports are reachable from SEA within two hops? To avoid just returning to Seattle,
            // we capture SEA in a StepLabel and filter out our eventual destinations on those that are not SEA!
#if (true)  // --8<-- [start:withinTwoFlightsWithNoReturn]
            var withinTwoFlightsWithNoReturn = await _g
                .V<Airport>()
                .Where(departure => departure.Code == "SEA")
                .As((__, sea) => __
                    .Out<Route>()
                    .Out<Route>()
                    .OfType<Airport>()
                    .Where(destination => destination != sea.Value))
                .Dedup()
                .ToArrayAsync();

            withinTwoFlightsWithNoReturn
                .WriteToConsole("We can go, by two flights, from SEA to these airports without returning to SEA");
#endif      // --8<-- [end:withinTwoFlightsWithNoReturn]

            #region Projections
#if (true)  // --8<-- [start:projectToTuple]
            var projectToTuple = await _g
                .V<Airport>()
                .Project(p => p
                    .ToTuple()
                    .By(x => x.Code!)
                    .By(x => x.Description!))
                .ToArrayAsync();
#endif      // --8<-- [end:projectToTuple]

#if (true)  // --8<-- [start:projectToTupleWithSubQuery]
            var projectToTupleWithSubQuery = await _g
                .V<Airport>()
                .Project(p => p
                    .ToTuple()
                    .By(x => x.Code!)
                    .By(__ => __
                        .Out<Route>()
                        .OfType<Airport>()
                        .Values(x => x.Code!)
                        .Fold()))
                .ToArrayAsync();
#endif      // --8<-- [end:projectToTupleWithSubQuery]

#if (true)  // --8<-- [start:projectToDynamic]
            var projectToDynamic = await _g
               .V<Airport>()
               .Project(p => p
                   .ToDynamic()
                   .By(x => x.Code!)
                   .By(x => x.Description!))
               .ToArrayAsync();
#endif      // --8<-- [end:projectToDynamic]

#if (true)  // --8<-- [start:projectToDynamicExplicit]
            var projectToDynamicExplicit = await _g
               .V<Airport>()
               .Project(p => p
                   .ToDynamic()
                   .By(x => x.Code!)
                   .By(x => x.Description!))
               .ToArrayAsync();
#endif      // --8<-- [end:projectToDynamicExplicit]

#if (true)  // --8<-- [start:projectToRecords]
            var projectToRecords = await _g
                .V<Airport>()
                .Project(p => p
                    .To<DepartureAndDestinationRecord>()
                    .By(
                        x => x.DepartureCode,
                        x => x.Code)
                    .By(
                        x => x.DestinationCodes,
                        __ => __
                            .Out<Route>()
                            .OfType<Airport>()
                            .Values(x => x.Code!)
                            .Fold()))
               .ToArrayAsync();
#endif      // --8<-- [end:projectToRecords]
#endregion

            #region Aggregates
#if (true)  // --8<-- [start:fold]
            var fold = await _g
                .V<Airport>()
                .Map(__ => __
                    .Out<Route>()
                    .OfType<Airport>()
                    .Values(x => x.Code!)
                    .Fold())
                .ToArrayAsync();
#endif      // --8<-- [end:fold]

#if (true)  // --8<-- [start:sumOfRoutes]
            var sumOfRoutes = await _g
                .V<Airport>()
                .Where(x => x.Code == "SEA")
                .OutE<Route>()
                .Values(x => x.Distance)
                .Sum()
                .FirstAsync();
#endif      // --8<-- [end:sumOfRoutes]

#if (true)  // --8<-- [start:maximumDistanceFromSEA]
            var maximumDistanceFromSEA = await _g
                .V<Airport>()
                .Where(x => x.Code == "SEA")
                .Map(__ => __
                    .OutE<Route>()
                    .Values(x => x.Distance)
                    .Max())
                .FirstAsync();
#endif      // --8<-- [end:maximumDistanceFromSEA]
#endregion

            #region Groups
#if (true)  // --8<-- [start:groupByNumberOfRoutes]
            var groupByNumberOfRoutes = await _g
                .V<Airport>()
                .Group(g => g
                    .ByKey(__ => __
                        .OutE<Route>()
                        .Count()))
                .ToArrayAsync();
#endif      // --8<-- [end:groupByNumberOfRoutes]

#if (true)  // --8<-- [start:groupCodesByNumberOfRoutes]
            var groupCodesByNumberOfRoutes = await _g
                .V<Airport>()
                .Group(g => g
                    .ByKey(__ => __
                        .OutE<Route>()
                        .Count())
                    .ByValue(__ => __
                        .Values(x => x.Code!)))
                .ToArrayAsync();
#endif      // --8<-- [end:groupCodesByNumberOfRoutes]
#endregion

            #region Loops
#if (true)  // --8<-- [start:threeFlights]
            var threeFlights = await _g
                .V<Airport>()
                .Map(__ => __
                    .Loop(l => l
                        .Repeat(__ => __
                            .Out<Route>()
                            .OfType<Airport>())
                        .Times(3))
                    .Values(x => x.Code!)
                    .Fold())
                .ToArrayAsync();
#endif      // --8<-- [end:threeFlights]

#if (true)  // --8<-- [start:repeatEmitUntil]
            var repeatEmitUntil = await _g
                .V<Airport>()
                .Where(x => x.Code == "SEA")
                .Loop(l => l
                    .Repeat(__ => __
                        .Out<Route>()
                        .OfType<Airport>())
                    .Emit()
                    .Until(__ => __
                        .CyclicPath()))
                .Dedup()
                .Values(x => x.Code!)
                .ToArrayAsync();
#endif      // --8<-- [end:repeatEmitUntil]
#endregion

            #region Tree
#if (true)  // --8<-- [start:two-hop-untyped-tree]
            var twoHopUntypedTree = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .Out<Route>()
                .Out<Route>()
                .Tree()
                .FirstAsync();

            // Although there is no static type information,
            // the nodes are still deserialized to instances
            // of the correct type
            Debug.Assert(twoHopUntypedTree.Keys.First() is Airport);
#endif      // --8<-- [end:two-hop-untyped-tree]

#if (true)  // --8<-- [start:two-hop-tree]
            var twoHopTree = await _g
                .V<Airport>()
                .Where(a => a.Code == "SEA")
                .Out<Route>()
                .Out<Route>()
                .Tree<Airport>()
                .FirstAsync();
#endif      // --8<-- [end:two-hop-tree]

#if (true)  // --8<-- [start:mixed-type-tree]
            var mixedTypeTree = await _g
                .V<Airport>()
                .OutE<Route>()
                .InV<Airport>()
                .Tree(_ => _
                    .Of<Airport>()
                    .Of<Route>()
                    .Of<Airport>())
                .FirstAsync();
#endif      // --8<-- [end:mixed-type-tree]

#if (true)  // --8<-- [start:mixed-type-tree-with-by]
            var mixedTypeTreeWithBy = await _g
                .V<Airport>()
                .Out<Route>()
                .OfType<Airport>()
                .Tree(_ => _
                    .Of<Airport>().By(x => x.Code!)
                    .Of<Airport>().By(x => x.Description!))
                .FirstAsync();

            var destinationNamesOfSEA = mixedTypeTreeWithBy["SEA"].Keys.ToArray();
#endif      // --8<-- [end:mixed-type-tree-with-by]
#endregion

            #region Format
#if (true)  // --8<-- [start:formatted-airports]
            var formattedAirports = await _g
                .V<Airport>()
                .Format(airport => $"{airport.Code} ({airport.Description})")
                .ToArrayAsync();
#endif      // --8<-- [end:formatted-airports]
#endregion
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
