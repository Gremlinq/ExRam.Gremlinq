using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dse.Graph;
using ExRam.Gremlinq;
using ExRam.Gremlinq.Dse;
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

        public static async Task CreateSchema(this IDseSession session, IDseGraphModel model, CancellationToken ct)
        {
            var queryProvider = new DseGraphQueryProvider(session);
            model = model.EdgeConnectionClosure();

            var propertyKeys = new Dictionary<string, Type>();
            
            foreach (var type in model.VertexLabels.Keys.Concat(model.EdgeLabels.Keys))
            {
                foreach (var property in type.GetProperties())
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
                        propertyKeys.Add(property.Name, propertyType);
                }
            }

            await propertyKeys
                .Select(propertyInfoKvp => GremlinQuery
                    .Create("schema", queryProvider)
                    .AddStep<string>("propertyKey", propertyInfoKvp.Key)
                    .AddStep<string>(NativeTypeSteps
                        .TryGetValue(propertyInfoKvp.Value)
                        .IfNone(() => throw new InvalidOperationException($"No native type found for {propertyInfoKvp.Value}.")))
                    .AddStep<string>("single")
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"))
                .Concat(model.VertexLabels
                    .Where(vertexKvp => !vertexKvp.Key.GetTypeInfo().IsAbstract)
                    .Select(vertexKvp => vertexKvp.Key
                        .TryGetPartitionKeyExpression(model)
                        .Map(keyExpression => ((keyExpression as LambdaExpression)?.Body as MemberExpression)?.Member.Name)
                        .AsEnumerable()
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", vertexKvp.Value),
                            (closureQuery, property) => closureQuery.AddStep<string>("partitionKey", property))
                        .ConditionalAddStep(
                            vertexKvp.Key.GetProperties().Any(),
                            query => query.AddStep<string>(
                                "properties",
                                vertexKvp.Key
                                    .GetProperties()
                                    .Select(x => x.Name)
                                    .ToImmutableList<object>()))
                        .AddStep<string>("create")))
                .Concat(model.VertexLabels
                    .Select(vertexKvp => (
                        SchemaInfo: vertexKvp, 
                        IndexProperties: model
                            .GetElementInfoHierarchy(vertexKvp.Key)
                            .SelectMany(x => model.SecondaryIndexes
                                .TryGetValue(x)
                                .AsEnumerable()
                                .SelectMany(y => y))
                            .Select(indexExpression => ((indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                            .ToImmutableList()))
                    .Where(tuple => !tuple.IndexProperties.IsEmpty)
                    .Select(tuple => tuple.IndexProperties
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", tuple.SchemaInfo.Value)
                                .AddStep<string>("index", Guid.NewGuid().ToString("N"))
                                .AddStep<string>("secondary"),
                            (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty))
                        .AddStep<string>("add"))
                .Concat(model.EdgeLabels
                    .Where(edgeKvp => !edgeKvp.Key.GetTypeInfo().IsAbstract)
                    .Select(edgeKvp => model.Connections
                        .Where(tuple => tuple.Item2 == edgeKvp.Key)
                        .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract && !x.Item3.GetTypeInfo().IsAbstract)
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("edgeLabel", edgeKvp.Value)
                                .AddStep<string>("single")
                                .ConditionalAddStep(
                                    edgeKvp.Key
                                        .GetProperties()
                                        .Any(),
                                    query => query.AddStep<string>(
                                        "properties",
                                        edgeKvp.Key
                                            .GetProperties()
                                            .Select(property => property.Name)
                                            .ToImmutableList<object>())),
                            (closureQuery, tuple) => closureQuery.AddStep<string>(
                                "connection",
                                tuple.Item1,
                                tuple.Item3))
                        .AddStep<string>("ifNotExists")
                        .AddStep<string>("create"))))
                .ToAsyncEnumerable()
                .SelectMany(query => query.Execute())
                .LastOrDefault(ct);
        }

        private static IGremlinQuery<TSource> ConditionalAddStep<TSource>(this IGremlinQuery<TSource> query, bool condition, Func<IGremlinQuery<TSource>, IGremlinQuery<TSource>> addStepFunction)
        {
            if (condition)
                return addStepFunction(query);

            return query;
        }

        private static Option<Expression> TryGetPartitionKeyExpression(this Type vertexTypeInfo, IDseGraphModel model)
        {
            return model.PrimaryKeys.TryGetValue(vertexTypeInfo)
                .Match(
                    _ => (Option<Expression>)_,
                    () =>
                    {
                        var baseType = vertexTypeInfo.GetTypeInfo().BaseType;

                        if (baseType != null && model.VertexLabels.ContainsKey(baseType))
                            return baseType.TryGetPartitionKeyExpression(model);

                        return Option<Expression>.None;
                    });
        }

        private static IEnumerable<Type> GetElementInfoHierarchy(this IGraphModel model, Type type)
        {
            do
            {
                yield return type;
                var baseType = type.GetTypeInfo().BaseType;

                type = null;

                if (model.VertexLabels.ContainsKey(baseType) || model.EdgeLabels.ContainsKey(baseType))
                    type = baseType;
            } while (type != null);
        }
    }
}
