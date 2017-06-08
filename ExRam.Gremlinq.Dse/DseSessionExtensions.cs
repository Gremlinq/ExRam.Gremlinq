using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dse.Graph;
using ExRam.Gremlinq;
using LanguageExt;

// ReSharper disable once CheckNamespace
namespace Dse
{
    public static class DseSessionExtensions
    {
        private sealed class DseGraphQueryProvider : IGremlinQueryProvider
        {
            private readonly IDseSession _session;

            public DseGraphQueryProvider(IDseSession session)
            {
                this._session = session;
            }

            public IGremlinQuery CreateQuery()
            {
                return GremlinQuery.Create((this._session.Cluster as IDseCluster)?.Configuration.GraphOptions.Source ?? "g", this);
            }

            public IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                if (typeof(T) != typeof(string))
                    throw new NotSupportedException("Only string queries are supported.");

                var executableQuery = query.Serialize(false);

                return this._session
                    .ExecuteGraphAsync(new SimpleGraphStatement(executableQuery
                        .parameters
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value is TimeSpan
                                ? Duration.FromTimeSpan((TimeSpan)kvp.Value)
                                : kvp.Value),
                            executableQuery.queryString))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => (T)(object)node.ToString());
            }

            public IGraphModel Model => GraphModel.Empty;
        }

        private static readonly IReadOnlyDictionary<Type, string> NativeTypeSteps = new Dictionary<Type, string>
        {
            { typeof(long), "Bigint" },
            { typeof(byte[]), "Blob" },
            { typeof(bool), "Boolean" },
            { typeof(decimal), "Decimal" },
            { typeof(double), "Double" },
            { typeof(TimeSpan), "Duration" },
            { typeof(IPAddress), "Inet" },
            { typeof(int), "Int" },
            //{ typeof(?), new GremlinStep("Linestring") },
            //{ typeof(?), new GremlinStep("Point") },
            //{ typeof(?), new GremlinStep("Polygon") },
            { typeof(short), "Smallint" },
            { typeof(string), "Text" },
            { typeof(DateTime), "Timestamp" },
            { typeof(Guid), "Uuid" }
            //{ typeof(?), new GremlinStep("Varint") },
        };

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session, IGraphModel model)
        {
            return new DseGraphQueryProvider(session)
                .WithModel(model)
                .WithJsonSupport();
        }

        public static async Task CreateSchema(this IDseSession session, IGraphSchema schema, CancellationToken ct)
        {
            var queryProvider = new DseGraphQueryProvider(session);

            var propertyQueries = schema.PropertySchemaInfos
                .Select(propertyInfo =>
                {
                    var query = GremlinQuery.Create("schema", queryProvider);

                    query = query
                        .AddStep<string>("propertyKey", propertyInfo.Name);

                    query = NativeTypeSteps
                        .TryGetValue(propertyInfo.Type)
                        .Match(
                            step => query.AddStep<string>(step),
                            () => throw new InvalidOperationException());

                    return query
                        .AddStep<string>("single")
                        .AddStep<string>("ifNotExists")
                        .AddStep<string>("create");
                });

            var vertexLabelQueries = schema.VertexSchemaInfos
                .Select(vertexSchemaInfo =>
                {
                    var query = GremlinQuery
                        .Create("schema", queryProvider)
                        .AddStep<string>("vertexLabel", vertexSchemaInfo.Label);

                    query = vertexSchemaInfo.PartitionKeyProperties
                        .Aggregate(
                            query,
                            (closureQuery, property) => closureQuery.AddStep<string>("partitionKey", property));

                    if (!vertexSchemaInfo.Properties.IsEmpty)
                        query = query.AddStep<string>("properties", vertexSchemaInfo.Properties.ToArray());

                    return query.AddStep<string>("create");
                });

            var indexQueries = schema.VertexSchemaInfos
                .Where(vertexSchemaInfo => !vertexSchemaInfo.IndexProperties.IsEmpty)
                .Select(vertexSchemaInfo =>
                {
                    var indexQuery = GremlinQuery
                        .Create("schema", queryProvider)
                        .AddStep<string>("vertexLabel", vertexSchemaInfo.Label)
                        .AddStep<string>("index", Guid.NewGuid().ToString("N"))
                        .AddStep<string>("secondary");

                    indexQuery = vertexSchemaInfo.IndexProperties
                        .Aggregate(
                            indexQuery,
                            (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty));

                    return indexQuery
                        .AddStep<string>("add");
                });

            var edgeLabelQueries = schema.EdgeSchemaInfos
                .Select(edgeSchemaInfo =>
                {
                    var query = GremlinQuery
                        .Create("schema", queryProvider)
                        .AddStep<string>("edgeLabel", edgeSchemaInfo.Label)
                        .AddStep<string>("single");

                    if (!edgeSchemaInfo.Properties.IsEmpty)
                        // ReSharper disable once CoVariantArrayConversion
                        query = query.AddStep<string>("properties", edgeSchemaInfo.Properties.ToArray());

                    return schema.Connections
                        .Where(tuple => tuple.Item2 == edgeSchemaInfo.Label)
                        .Aggregate(
                            query,
                            (closureQuery, tuple) => closureQuery.AddStep<string>(
                                "connection",
                                tuple.Item1,
                                tuple.Item3))
                        .AddStep<string>("ifNotExists")
                        .AddStep<string>("create");
                });

            var queries = propertyQueries.Concat(vertexLabelQueries).Concat(indexQueries).Concat(edgeLabelQueries).ToArray();

            foreach (var query in queries)
            {
                await query.Execute().LastOrDefault(ct);
            }
        }
    }
}
