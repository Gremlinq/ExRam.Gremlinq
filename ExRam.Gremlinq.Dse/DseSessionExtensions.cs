using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dse.Graph;
using ExRam.Gremlinq;
using LanguageExt;
using Unit = System.Reactive.Unit;

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
                    .ExecuteGraphAsync(new SimpleGraphStatement(executableQuery.parameters, executableQuery.queryString))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => (T)(object)node.ToString());
            }

            public IGraphModel Model => GremlinModel.Empty;
            public IGraphElementNamingStrategy NamingStrategy => GraphElementNamingStrategy.Simple;
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
                var propertyKeys = new Dictionary<string, Type>();
                var queryProvider = new DseGraphQueryProvider(this._session);

                foreach (var vertexType in model.VertexTypes.Concat(model.EdgeTypes))
                {
                    var type = vertexType;

                    while (type != typeof(object))
                    {
                        var typeInfo = type.GetTypeInfo();

                        foreach (var property in typeInfo.DeclaredProperties)
                        {
                            if (propertyKeys.TryGetValue(property.Name, out var existingType))
                            {
                                if (existingType != property.PropertyType)  //TODO: Support any kind of inheritance here?
                                    throw new InvalidOperationException($"Property {property.Name} already exists with type {existingType.Name}.");
                            }
                            else
                                propertyKeys.Add(property.Name, property.PropertyType);
                        }

                        type = typeInfo.BaseType;
                    }
                }

                var queries = propertyKeys
                    .Select(kvp =>
                    {
                        var query = GremlinQuery.ForGraph("schema", queryProvider);
                        query = query
                            .AddStep<Unit>("propertyKey", kvp.Key);

                        query = NativeTypeSteps
                            .TryGetValue(kvp.Value)
                            .Match(
                                step => query.AddStep<Unit>(step),
                                () => throw new InvalidOperationException());

                        return query
                            .AddStep<Unit>("single")
                            .AddStep<Unit>("ifNotExists")
                            .AddStep<Unit>("create");
                    })
                    .ToArray();

                foreach (var query in queries)
                {
                    await queryProvider
                        .Execute(query)
                        .LastOrDefault(ct);
                }
            }
        }

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session, IGraphModel model, IGraphElementNamingStrategy namingStrategy)
        {
            return new DseGraphQueryProvider(session)
                .WithModel(model)
                .WithNamingStrategy(namingStrategy)
                .WithJsonSupport();
        }

        public static IGraphSchemaCreator CreateGraphSchemaCreator(this IDseSession session)
        {
            return new DseGraphSchemaCreator(session);
        }
    }
}
