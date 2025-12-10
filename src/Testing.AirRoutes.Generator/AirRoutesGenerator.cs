#pragma warning disable RS1035 // Do not use APIs banned for analyzers
#pragma warning disable RS1042 // Implementations of this interface are not allowed

using System.Xml.Serialization;

using Microsoft.CodeAnalysis;

namespace ExRam.Gremlinq.Testing.AirRoutes.Generator
{
    [Generator]
    public class AirRoutesGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();
#endif

            context
                .RegisterForPostInitialization(context =>
                {
                    context
                       .AddSource(
                           "RemoveAirRoutes",
                           """
                            #nullable enable
                            using ExRam.Gremlinq.Core;
                            using ExRam.Gremlinq.Core.Models;

                            namespace ExRam.Gremlinq.Testing.AirRoutes
                            {
                                public static partial class GremlinQuerySourceExtensions
                                {
                                    /// <summary>
                                    ///  Removes any AirRoutes data from the database.
                                    /// </summary>
                                    public static async Task RemoveAirRoutes(this IGremlinQuerySource source, CancellationToken ct = default)
                                    {
                                        await source
                                            .ConfigureEnvironment(env => env
                                                .UseModel(GraphModel.FromBaseTypes<Airport, Route>()))
                                            .V<Airport>()
                                            .Where(airport => airport.Id!.StartsWith("airport_"))
                                            .Drop();
                                    }
                                }
                            }
                            """);

                    context
                        .AddSource(
                            "Entities",
                            """
                            namespace ExRam.Gremlinq.Testing.AirRoutes
                            {
                                internal abstract class Element
                                {
                                    public string? Id { get; set; }
                                }

                                internal sealed class Airport : Element//Vertex
                                {
                                    public string? Code { get; set; }
                                    public string? ICAO { get; set; }
                                    public string? City { get; set; }
                                    public string? Region { get; set; }
                                    public string? Country { get; set; }
                                    public string? Description { get; set; }

                                    public int Runways { get; set; }
                                    public int Elevation { get; set; }
                                    public double Latitude { get; set; }
                                    public double Longitude { get; set; }
                                    public int LongestRunway { get; set; }
                                }

                                internal sealed class Route : Element//Edge
                                {
                                    public long Distance { get; set; }
                                }
                            }
                            """);
                });
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //context
            //    .AddSource("AirRoutes", Generate("CreateAirRoutes", "https://raw.githubusercontent.com/krlawrence/graph/refs/heads/master/sample-data/air-routes-latest.graphml"));

            context
                .AddSource(
                    "AirRoutesSmall",
                    Generate(
                        "CreateAirRoutesSmall",
                        """
                        /// Creates a small AirRoutes set if it not exists in the database.
                        /// This method is idempotent, however, commenting out its uses will save time once the database has been populated.
                        """,
                        "https://raw.githubusercontent.com/krlawrence/graph/refs/heads/master/sample-data/air-routes-small-latest.graphml"));
        }

        public string Generate(string methodName, string summary, string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var xmlString = httpClient
                    .GetStringAsync(uri).Result;

                using (var stringReader = new StringReader(xmlString))
                {
                    var serializer = new XmlSerializer(typeof(Graphml));
                    var graphml = (Graphml)serializer.Deserialize(stringReader);

                    var writer = CodeWriter
                        .Create()
                        .WriteLine("#nullable enable")
                        .WriteLine("using ExRam.Gremlinq.Core;")
                        .WriteLine("using ExRam.Gremlinq.Core.Models;")
                        .WriteLine()
                        .WriteLine("namespace ExRam.Gremlinq.Testing.AirRoutes")
                        .Block(writer => writer
                            .WriteLine("partial class GremlinQuerySourceExtensions")
                            .Block(writer => writer
                                .WriteLine("/// <summary>")
                                .WriteLine(summary)
                                .Write("/// AirRoutes data taken from ").WriteLine(uri)
                                .WriteLine("/// With many thanks to author Kelvin R. Lawrence.")
                                .WriteLine("/// </summary>")
                                .WriteLine($"public static async Task {methodName}(this IGremlinQuerySource source, CancellationToken ct = default)")
                                .Block(writer =>
                                {
                                    if (graphml.Graph?.Node is { } nodes && graphml.Graph?.Edge is { } edges)
                                    {
                                        writer = writer
                                            .WriteLine("source = source")
                                            .Indent(writer => writer
                                                .WriteLine(".ConfigureEnvironment(env => env")
                                                .Indent(writer => writer
                                                    .WriteLine(".UseModel(GraphModel.FromBaseTypes<Airport, Route>()));")))
                                            .WriteLine()
                                            .WriteLine("await source")              
                                            .Indent(writer =>
                                            {
                                                writer = writer
                                                    .WriteLine(".Inject(0)");

                                                foreach (var node in nodes)
                                                {
                                                    if (node.Data is { } nodeData)
                                                    {
                                                        var nodeId = $"airport_{node.Id}";

                                                        if (nodeData.Any(nodeDataKey => nodeDataKey.Key == "labelV" && nodeDataKey.Text == "airport"))
                                                        {
                                                            if (nodeData.FirstOrDefault(nodeDataKey => nodeDataKey.Key == "code") is { Text: { Length: > 0 } code })
                                                            {
                                                                writer = writer
                                                                    .WriteLine(".Coalesce(")
                                                                    .Indent(writer => writer
                                                                        .WriteLine("__ => __")
                                                                        .Indent(writer => writer
                                                                            .WriteLine($".V(\"{nodeId}\"),"))
                                                                        .WriteLine("__ => __")
                                                                        .Indent(writer => writer
                                                                            .WriteLine(".AddV(new Airport {")
                                                                            .Indent(writer =>
                                                                            {
                                                                                writer = writer
                                                                                    .WriteLine($"Id =\"{nodeId}\",")
                                                                                    .WriteLine($"Code = \"{code}\",");

                                                                                foreach (var data in nodeData)
                                                                                {
                                                                                    writer = data.Key switch
                                                                                    {
                                                                                        "icao" => writer.WriteLine($"ICAO = \"{data.Text}\","),
                                                                                        "city" => writer.WriteLine($"City = \"{data.Text}\","),
                                                                                        "desc" => writer.WriteLine($"Description = \"{data.Text}\","),
                                                                                        "region" => writer.WriteLine($"Region = \"{data.Text}\","),
                                                                                        "runways" => writer.WriteLine($"Runways = {data.Text},"),
                                                                                        "longestRunway" => writer.WriteLine($"LongestRunway = {data.Text},"),
                                                                                        "elev" => writer.WriteLine($"Elevation = {data.Text},"),
                                                                                        "country" => writer.WriteLine($"Country = \"{data.Text}\","),
                                                                                        "lat" => writer.WriteLine($"Latitude = {data.Text},"),
                                                                                        "lon" => writer.WriteLine($"Longitude = {data.Text},"),
                                                                                        _ => writer
                                                                                    };
                                                                                }

                                                                                return writer;
                                                                            })
                                                                            .WriteLine("}))")));
                                                            }
                                                        }
                                                    }
                                                }

                                                return writer
                                                    .WriteLine(".ToArrayAsync(ct);");
                                            });


                                        foreach (var edgeGroup in edges.GroupBy(x => x.Source))
                                        {
                                            writer = writer
                                                .WriteLine()
                                                .WriteLine("await source")
                                                .Indent(writer => writer
                                                    .WriteLine($".V<Airport>(\"airport_{edgeGroup.Key}\")")
                                                    .Do(writer =>
                                                    {
                                                        foreach (var edge in edgeGroup)
                                                        {
                                                            var routeId = $"airroute_{edge.Id}";

                                                            if (edge.Data is { } edgeData)
                                                            {
                                                                if (edgeData.Any(nodeDataKey => nodeDataKey.Key == "labelE" && nodeDataKey.Text == "route"))
                                                                {
                                                                    if (edgeData.FirstOrDefault(nodeDataKey => nodeDataKey.Key == "dist") is { Text: { Length: > 0 } dist })
                                                                    {
                                                                        writer = writer
                                                                            .WriteLine(".SideEffect(__ => __")
                                                                            .Indent(writer => writer
                                                                                .WriteLine(".Coalesce(")
                                                                                .Indent(writer => writer
                                                                                    .WriteLine("__ => __")
                                                                                    .Indent(writer => writer
                                                                                        .WriteLine(".OutE<Route>()")
                                                                                        .WriteLine($".Where(x => x.Id == \"{routeId}\"),"))
                                                                                    .WriteLine("__ => __")
                                                                                    .Indent(writer => writer
                                                                                        .WriteLine(".AddE(new Route {")
                                                                                           .Indent(writer => writer
                                                                                               .WriteLine($"Id = \"{routeId}\",")
                                                                                               .WriteLine($"Distance = {dist}"))
                                                                                           .WriteLine("})")
                                                                                           .WriteLine(".To(__ => __")
                                                                                           .Indent(writer => writer
                                                                                               .WriteLine($".V<Airport>(\"airport_{edge.Target}\"))))")))));
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        return writer
                                                            .WriteLine(".ToArrayAsync(ct);");
                                                    }));
                                        }
                                    }

                                    return writer;
                                })));

                    return writer.Code();
                }
            }
        }
    }
}
