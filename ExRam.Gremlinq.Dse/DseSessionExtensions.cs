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

        public static async Task CreateSchema(this IDseSession session, DseGraphSchema schema, CancellationToken ct)
        {
            var queryProvider = new DseGraphQueryProvider(session);

            await schema.PropertySchemaInfos
                .Select(propertyInfo => GremlinQuery
                    .Create("schema", queryProvider)
                    .AddStep<string>("propertyKey", propertyInfo.Name)
                    .AddStep<string>(NativeTypeSteps
                        .TryGetValue(propertyInfo.Type)
                        .IfNone(() => throw new InvalidOperationException($"No native type found for {propertyInfo.Type}.")))
                    .AddStep<string>("single")
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"))
                .Concat(schema.Model.VertexTypes.Values
                    .Select(vertexType => vertexType
                        .TryGetPartitionKeyExpression(schema.Model)
                        .Map(keyExpression => ((keyExpression as LambdaExpression)?.Body as MemberExpression)?.Member.Name)
                        .AsEnumerable()
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", vertexType.Label),
                            (closureQuery, property) => closureQuery.AddStep<string>("partitionKey", property))
                        .ConditionalAddStep(
                            vertexType.ElementType.GetProperties().Any(),
                            query => query.AddStep<string>(
                                "properties", 
                                vertexType
                                    .ElementType.GetProperties()
                                    .Select(x => x.Name)
                                    .ToImmutableList<object>()))
                        .AddStep<string>("create")))
                .Concat(schema.Model.VertexTypes.Values
                    .Select(vertexType => (
                        SchemaInfo: vertexType, 
                        IndexProperties: schema.Model
                            .GetElementInfoHierarchy(vertexType)
                            .OfType<VertexTypeInfo>()
                            .SelectMany(x => x.SecondaryIndexes)
                            .Select(indexExpression => ((indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                            .ToImmutableList()))
                    .Where(tuple => !tuple.IndexProperties.IsEmpty)
                    .Select(tuple => tuple.IndexProperties
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", tuple.SchemaInfo.Label)
                                .AddStep<string>("index", Guid.NewGuid().ToString("N"))
                                .AddStep<string>("secondary"),
                            (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty))
                        .AddStep<string>("add"))
                .Concat(schema.EdgeSchemaInfos
                    .Select(edgeSchemaInfo => schema.Connections
                        .Where(tuple => tuple.Item2 == edgeSchemaInfo.Label)
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("edgeLabel", edgeSchemaInfo.Label)
                                .AddStep<string>("single")
                                .ConditionalAddStep(
                                    !edgeSchemaInfo.Properties.IsEmpty,
                                    query => query.AddStep<string>(
                                        "properties",
                                        edgeSchemaInfo
                                            .Properties
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

        private static Option<Expression> TryGetPartitionKeyExpression(this VertexTypeInfo vertexTypeInfo, IGraphModel model)
        {
            return vertexTypeInfo.PrimaryKey
                .Match(
                    _ => (Option<Expression>)_,
                    () =>
                    {
                        var baseType = vertexTypeInfo.ElementType.GetTypeInfo().BaseType;

                        if (baseType != null)
                        {
                            return model.VertexTypes
                                .TryGetValue(baseType)
                                .Bind(baseVertexInfo => baseVertexInfo.TryGetPartitionKeyExpression(model));
                        }

                        return Option<Expression>.None;
                    });
        }

        private static IEnumerable<GraphElementInfo> GetElementInfoHierarchy(this IGraphModel model, GraphElementInfo elementInfo)
        {
            do
            {
                yield return elementInfo;
                var baseType = elementInfo.ElementType.GetTypeInfo().BaseType;

                elementInfo = null;

                if (model.VertexTypes.TryGetValue(baseType, out var vertexInfo))
                    elementInfo = vertexInfo;
                else if (model.EdgeTypes.TryGetValue(baseType, out var edgeInfo))
                    elementInfo = edgeInfo;
            } while (elementInfo != null);
        }
    }
}
