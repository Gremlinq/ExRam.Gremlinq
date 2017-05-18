using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
                return GremlinQuery.ForGraph((this._session.Cluster as IDseCluster)?.Configuration.GraphOptions.Source ?? "g", this);
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

            public IGraphModel Model => GremlinModel.Empty;
        }

        private sealed class DseGraphSchemaCreator : IGraphSchemaCreator
        {
            private readonly IDseSession _session;

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

            public DseGraphSchemaCreator(IDseSession session)
            {
                this._session = session;
            }

            public async Task CreateSchema(IGraphModel model, CancellationToken ct)
            {
                var queryProvider = new DseGraphQueryProvider(this._session);
                var queries = this
                    .CreatePropertyKeyQueries(model, queryProvider)
                    .Concat(this.CreateVertexLabelQueries(model, queryProvider))
                    .Concat(this.CreateEdgeLabelQueries(model, queryProvider));

                foreach (var query in queries)
                {
                    await queryProvider
                        .Execute(query)
                        .LastOrDefault(ct);
                }
            }

            private IEnumerable<IGremlinQuery<string>> CreateVertexLabelQueries(IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                return model.VertexTypes
                    .Select(vertexInfo =>
                    {
                        var query = GremlinQuery
                            .ForGraph("schema", queryProvider)
                            .AddStep<string>("vertexLabel", vertexInfo.Label);

                        var properties = vertexInfo.ElementType.GetProperties().Select(property => property.Name).ToArray();
                        if (properties.Length > 0)
                            // ReSharper disable once CoVariantArrayConversion
                            query = query.AddStep<string>("properties", properties);

                        return query
                            .AddStep<string>("ifNotExists")
                            .AddStep<string>("create");
                    });
            }

            private IEnumerable<IGremlinQuery<string>> CreateEdgeLabelQueries(IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                return model.EdgeTypes
                    .Select(edgeInfo =>
                    {
                        var query = GremlinQuery
                            .ForGraph("schema", queryProvider)
                            .AddStep<string>("edgeLabel", edgeInfo.Label)
                            .AddStep<string>("single");

                        var properties = edgeInfo.ElementType.GetProperties().Select(property => property.Name).ToArray();
                        if (properties.Length > 0)
                            // ReSharper disable once CoVariantArrayConversion
                            query = query.AddStep<string>("properties", properties);

                        query = model.Connections
                            .Where(tuple => tuple.Item2.ElementType == edgeInfo.ElementType)
                            .Where(tuple => !tuple.Item1.ElementType.GetTypeInfo().IsAbstract && !tuple.Item2.ElementType.GetTypeInfo().IsAbstract && !tuple.Item3.ElementType.GetTypeInfo().IsAbstract)
                            .Aggregate(
                                query,
                                (closureQuery, tuple) => closureQuery.AddStep<string>(
                                    "connection",
                                    tuple.Item1.Label,
                                    tuple.Item3.Label));


                        return query
                            .AddStep<string>("ifNotExists")
                            .AddStep<string>("create");
                    });
            }

            private IEnumerable<IGremlinQuery<string>> CreatePropertyKeyQueries(IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                var propertyKeys = new Dictionary<string, Type>();

                foreach (var vertexType in model.VertexTypes.Concat(model.EdgeTypes))
                {
                    foreach (var property in vertexType.ElementType.GetProperties())
                    {
                        var propertyType = property.PropertyType;

                        while (true)
                        {
                            if (propertyType.GetTypeInfo().IsEnum)
                                propertyType = Enum.GetUnderlyingType(propertyType);
                            else
                            {
                                var maybeNullableType = Nullable.GetUnderlyingType(propertyType);
                                if (maybeNullableType != null)
                                    propertyType = maybeNullableType;
                                else
                                    break;
                            }
                        }

                        if (propertyKeys.TryGetValue(property.Name, out var existingType))
                        {
                            if (existingType != propertyType) //TODO: Support any kind of inheritance here?
                                throw new InvalidOperationException($"Property {property.Name} already exists with type {existingType.Name}.");
                        }
                        else
                        {
                            propertyKeys.Add(property.Name, propertyType);
                        }
                    }
                }

                return propertyKeys
                    .Select(kvp =>
                    {
                        var query = GremlinQuery.ForGraph("schema", queryProvider);
                        query = query
                            .AddStep<string>("propertyKey", kvp.Key);

                        query = NativeTypeSteps
                            .TryGetValue(kvp.Value)
                            .Match(
                                step => query.AddStep<string>(step),
                                () => throw new InvalidOperationException());

                        return query
                            .AddStep<string>("single")
                            .AddStep<string>("ifNotExists")
                            .AddStep<string>("create");
                    });
            }
        }

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session, IGraphModel model)
        {
            return new DseGraphQueryProvider(session)
                .WithModel(model)
                .WithJsonSupport();
        }

        public static IGraphSchemaCreator CreateGraphSchemaCreator(this IDseSession session)
        {
            return new DseGraphSchemaCreator(session);
        }
    }
}
