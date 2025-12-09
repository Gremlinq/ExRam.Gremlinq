#pragma warning disable RS1035 // Do not use APIs banned for analyzers
#pragma warning disable RS1042 // Implementations of this interface are not allowed

using System;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                            "Airport",
                            """
                            namespace ExRam.Gremlinq.Testing.AirRoutes
                            {
                                public sealed class Airport
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
                            }
                            """);

                    context
                        .AddSource(
                            "Route",
                            """
                            namespace ExRam.Gremlinq.Testing.AirRoutes
                            {
                                public sealed class Route
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
            //    .AddSource("AirRoutes.g", Generate("CreateAirRoutes", "https://raw.githubusercontent.com/krlawrence/graph/refs/heads/master/sample-data/air-routes-latest.graphml"));

            context
                .AddSource("AirRoutesSmall.g", Generate("CreateAirRoutesSmall", "https://raw.githubusercontent.com/krlawrence/graph/refs/heads/master/sample-data/air-routes-small-latest.graphml"));
        }

        public string Generate(string methodName, string uri)
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
                        .WriteLine()
                        .WriteLine("namespace ExRam.Gremlinq.Testing.AirRoutes")
                        .Block(writer => writer
                            .WriteLine("public static partial class GremlinQuerySourceExtensions")
                            .Block(writer => writer
                                .WriteLine($"public static async Task {methodName}(this IGremlinQuerySource source, CancellationToken ct = default)")
                                .Block(writer =>
                                {
                                    var nodeCodes = new Dictionary<int, string>();

                                    if (graphml.Graph?.Node is { } nodes)
                                    {
                                        foreach (var node in nodes)
                                        {
                                            if (node.Data is { } nodeData)
                                            {
                                                if (nodeData.Any(nodeDataKey => nodeDataKey.Key == "labelV" && nodeDataKey.Text == "airport"))
                                                {
                                                    if (nodeData.FirstOrDefault(nodeDataKey => nodeDataKey.Key == "code") is { Text: { Length: > 0 } code })
                                                    {
                                                        nodeCodes[node.Id] = code;

                                                        writer = writer
                                                            .WriteLine("await source")
                                                            .Indent(writer => writer
                                                                .WriteLine(".AddV(new Airport {")
                                                                .Indent(writer =>
                                                                {
                                                                    writer = writer
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
                                                                .WriteLine("})")
                                                                .WriteLine(".ToArrayAsync(ct);"));

                                                        writer = writer
                                                            .WriteLine();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (graphml.Graph?.Edge is { } edges)
                                    {
                                        foreach (var edge in edges)
                                        {
                                            if (nodeCodes.TryGetValue(edge.Source, out var srcCode) && nodeCodes.TryGetValue(edge.Target, out var trgtCode))
                                            {
                                                if (edge.Data is { } edgeData)
                                                {
                                                    if (edgeData.Any(nodeDataKey => nodeDataKey.Key == "labelE" && nodeDataKey.Text == "route"))
                                                    {
                                                        if (edgeData.FirstOrDefault(nodeDataKey => nodeDataKey.Key == "dist") is { Text: { Length: > 0 } dist })
                                                        {
                                                            writer = writer
                                                                .WriteLine("await source")
                                                                .Indent(writer => writer
                                                                    .WriteLine(".V<Airport>()")
                                                                    .WriteLine($".Where(x => x.Code == \"{srcCode}\")")
                                                                    .WriteLine(".AddE(new Route {")
                                                                    .Indent(writer => writer
                                                                        .WriteLine($"Distance = {dist}"))
                                                                    .WriteLine("})")
                                                                    .WriteLine(".To(__ => __")
                                                                    .Indent(writer => writer
                                                                        .WriteLine(".V<Airport>()")
                                                                        .WriteLine($".Where(x => x.Code == \"{trgtCode}\"))"))
                                                                    .WriteLine(".ToArrayAsync(ct);"))
                                                                .WriteLine();
                                                        }
                                                    }
                                                }
                                            }
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
