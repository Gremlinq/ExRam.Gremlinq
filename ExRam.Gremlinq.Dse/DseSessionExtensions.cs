using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                return model.VertexTypes.Values
                    .SelectMany(vertexInfo => this.CreateQueriesForVertexInfo(vertexInfo, model, queryProvider));
            }

            private IEnumerable<IGremlinQuery<string>> CreateQueriesForVertexInfo(VertexTypeInfo vertexTypeInfo, IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                var baseQuery = GremlinQuery
                    .Create("schema", queryProvider)
                    .AddStep<string>("vertexLabel", vertexTypeInfo.Label);

                var propertyQuery = baseQuery;

                this
                    .TryGetPartitionKeyExpression(model, vertexTypeInfo)
                    .IfSome(keyExpression =>
                    {
                        if (keyExpression is LambdaExpression lambdaExpression)
                        {
                            if (lambdaExpression.Body is MemberExpression memberExpression)
                            {
                                propertyQuery = propertyQuery.AddStep<string>("partitionKey", memberExpression.Member.Name);
                                return;
                            }
                        }

                        throw new NotSupportedException();
                    });

                var properties = vertexTypeInfo.ElementType.GetProperties().Select(property => property.Name).ToArray();
                if (properties.Length > 0)
                    // ReSharper disable once CoVariantArrayConversion
                    propertyQuery = propertyQuery.AddStep<string>("properties", properties);

                yield return propertyQuery
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create");

                var indexExpressions = this.GetSecondaryIndexExpression(model, vertexTypeInfo).ToArray();

                if (indexExpressions.Length > 0)
                {
                    var indexQuery = baseQuery
                        .AddStep<string>("index", Guid.NewGuid().ToString("N"))
                        .AddStep<string>("secondary");

                    foreach (var secondaryIndexExpression in indexExpressions)
                    {
                        if (secondaryIndexExpression is LambdaExpression lambdaExpression)
                        {
                            var body = lambdaExpression.Body.StripConvert();
                            if (body is MemberExpression member)
                            {
                                indexQuery = indexQuery
                                    .AddStep<string>("by", member.Member.Name);
                            }
                        }
                    }

                    yield return indexQuery
                        .AddStep<string>("add");
                }
            }

            private Option<Expression> TryGetPartitionKeyExpression(IGraphModel model, VertexTypeInfo vertexTypeInfo)
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
                                    .Bind(baseVertexInfo => TryGetPartitionKeyExpression(model, baseVertexInfo));
                            }

                            return Option<Expression>.None;
                        });
            }

            private IEnumerable<Expression> GetSecondaryIndexExpression(IGraphModel model, VertexTypeInfo vertexTypeInfo)
            {
                var ret = (IEnumerable<Expression>)vertexTypeInfo.SecondaryIndexes;
                var baseType = vertexTypeInfo.ElementType.GetTypeInfo().BaseType;

                if (baseType != null)
                {
                    ret = ret.Concat(model.VertexTypes
                        .TryGetValue(baseType)
                        .AsEnumerable()
                        .SelectMany(baseVertexInfo => this.GetSecondaryIndexExpression(model, baseVertexInfo)));
                }

                return ret;
            }

            private IEnumerable<IGremlinQuery<string>> CreateEdgeLabelQueries(IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                return model.EdgeTypes.Values
                    .Select(edgeInfo =>
                    {
                        var query = GremlinQuery
                            .Create("schema", queryProvider)
                            .AddStep<string>("edgeLabel", edgeInfo.Label)
                            .AddStep<string>("single");

                        var properties = edgeInfo.ElementType.GetProperties().Select(property => property.Name).ToArray();
                        if (properties.Length > 0)
                            // ReSharper disable once CoVariantArrayConversion
                            query = query.AddStep<string>("properties", properties);

                        return model.Connections
                            .Where(tuple => tuple.Item2 == edgeInfo.ElementType)
                            .Where(tuple => !tuple.Item1.GetTypeInfo().IsAbstract && !tuple.Item2.GetTypeInfo().IsAbstract && !tuple.Item3.GetTypeInfo().IsAbstract)
                            .Aggregate(
                                query,
                                (closureQuery, tuple) => closureQuery.AddStep<string>(
                                    "connection",
                                    model.TryGetLabelOfType(tuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                                    model.TryGetLabelOfType(tuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */))))
                            .AddStep<string>("ifNotExists")
                            .AddStep<string>("create");
                    });
            }

            private IEnumerable<IGremlinQuery<string>> CreatePropertyKeyQueries(IGraphModel model, IGremlinQueryProvider queryProvider)
            {
                var propertyKeys = new Dictionary<string, Type>();

                foreach (var vertexType in model.VertexTypes.Values.Cast<GraphElementInfo>().Concat(model.EdgeTypes.Values))
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
                        var query = GremlinQuery.Create("schema", queryProvider);
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
