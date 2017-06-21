using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphModelSchemaCreationExtensions
    {
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

        public static IEnumerable<IGremlinQuery<string>> CreateSchemaQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            var identifierFactory = IdentifierFactory.CreateDefault();
            
            model = model
                .EdgeConnectionClosure();

            return model
                .CreatePropertyKeyQueries(queryProvider)
                .Concat(model.CreateVertexLabelQueries(queryProvider))
                .Concat(model.CreateVertexMaterializedIndexQueries(queryProvider, identifierFactory))
                .Concat(model.CreateVertexSecondaryIndexQueries(queryProvider, identifierFactory))
                .Concat(model.CreateVertexSearchIndexQueries(queryProvider))
                .Concat(model.CreateEdgeLabelQueries(queryProvider))
                .Concat(model.CreateEdgeIndexQueries(queryProvider, identifierFactory));
        }

        private static IEnumerable<IGremlinQuery<string>> CreatePropertyKeyQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
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

            return propertyKeys
                .Select(propertyInfoKvp => GremlinQuery
                    .Create("schema", queryProvider)
                    .AddStep<string>("propertyKey", propertyInfoKvp.Key)
                    .AddStep<string>(NativeTypeSteps
                        .TryGetValue(propertyInfoKvp.Value)
                        .IfNone(() => throw new InvalidOperationException($"No native type found for {propertyInfoKvp.Value}.")))
                    .AddStep<string>("single")
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateVertexSecondaryIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider, IIdentifierFactory identifierFactory)
        {
            return model.CreateIndexQueries(model.SecondaryIndexes, "secondary", queryProvider, identifierFactory);
        }

        private static IEnumerable<IGremlinQuery<string>> CreateVertexMaterializedIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider, IIdentifierFactory identifierFactory)
        {
            return model.CreateIndexQueries(model.MaterializedIndexes, "materialized", queryProvider, identifierFactory);
        }

        private static IEnumerable<IGremlinQuery<string>> CreateVertexSearchIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.VertexLabels
                .Where(vertexKvp => !vertexKvp.Key.GetTypeInfo().IsAbstract)
                .Select(vertexKvp => (
                    Label: vertexKvp.Value,
                    IndexProperties: vertexKvp.Key
                        .GetTypeHierarchy(model)
                        .SelectMany(x => model.SearchIndexes
                        .TryGetValue(x)
                        .AsEnumerable())
                        .Take(1)
                        .Select(indexExpression => ((indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                        .ToImmutableList()))
                .Where(tuple => !tuple.IndexProperties.IsEmpty)
                .Select(tuple => tuple.IndexProperties
                    .Aggregate(
                        GremlinQuery
                            .Create("schema", queryProvider)
                            .AddStep<string>("vertexLabel", tuple.Label)
                            .AddStep<string>("index", "search")
                            .AddStep<string>("search"),
                        (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty))
                    .AddStep<string>("add"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateIndexQueries(this IDseGraphModel model, IImmutableDictionary<Type, IImmutableSet<Expression>> indexDictionary, string keyword, IGremlinQueryProvider queryProvider, IIdentifierFactory identifierFactory)
        {
            return model.VertexLabels
                .Where(vertexKvp => !vertexKvp.Key.GetTypeInfo().IsAbstract)
                .Select(vertexKvp => (
                    Label: vertexKvp.Value,
                    IndexProperties: vertexKvp.Key
                        .GetTypeHierarchy(model)
                        .SelectMany(x => indexDictionary
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
                            .AddStep<string>("vertexLabel", tuple.Label)
                            .AddStep<string>("index", identifierFactory.CreateIndexName())
                            .AddStep<string>(keyword),
                        (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty))
                    .AddStep<string>("add"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateVertexLabelQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.VertexLabels
                .Where(vertexKvp => !vertexKvp.Key.GetTypeInfo().IsAbstract)
                .Select(vertexKvp => vertexKvp.Key
                    .TryGetPartitionKeyExpression(model)
                    .Map(keyExpression => ((keyExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                    .AsEnumerable()
                    .Aggregate(
                        GremlinQuery
                            .Create("schema", queryProvider)
                            .AddStep<string>("vertexLabel", vertexKvp.Value),
                        (closureQuery, property) => closureQuery.AddStep<string>("partitionKey", property))
                    .ConditionalAddStep(
                        vertexKvp.Key.GetProperties().Any(),
                        query => query
                            .AddStep<string>("properties",
                                vertexKvp.Key
                                    .GetProperties()
                                    .Select(x => x.Name)
                                    .ToImmutableList<object>()))
                            .AddStep<string>("create"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateEdgeLabelQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.EdgeLabels
                .Where(edgeKvp => !edgeKvp.Key.GetTypeInfo().IsAbstract)
                .Select(edgeKvp => model.Connections
                    .TryGetValue(edgeKvp.Key)
                    .AsEnumerable()
                    .SelectMany(x => x)
                    .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract)
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
                            model.VertexLabels.TryGetValue(tuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ )),
                            model.VertexLabels.TryGetValue(tuple.Item2).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ ))))
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateEdgeIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider, IIdentifierFactory identifierFactory)
        {
            return model.EdgeIndexes.Keys
                .SelectMany(edgeType => model
                    .GetDerivedElementInfos(edgeType, true)
                    .Where(inheritedType => !inheritedType.GetTypeInfo().IsAbstract)
                    .SelectMany(inheritedType => model
                        .EdgeIndexes[inheritedType]
                        .Where(index => index.direction != EdgeDirection.None)
                        .SelectMany(index => model
                            .GetDerivedElementInfos(index.vertexType, true)
                            .Where(inheritedVertexType => !inheritedVertexType.GetTypeInfo().IsAbstract)
                            .Select(inheritedVertexType => GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", model.GetLabelOfType(inheritedVertexType))
                                .AddStep<string>("index", identifierFactory.CreateIndexName())
                                .AddStep<string>(
                                    index.direction == EdgeDirection.Out
                                        ? "outE"
                                        : index.direction == EdgeDirection.In
                                            ? "inE"
                                            : "bothE",
                                    model.GetLabelOfType(edgeType))
                                .AddStep<string>("by", ((index.indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                                .AddStep<string>("add")))));
        }

        private static IEnumerable<Type> GetTypeHierarchy(this Type type, IGraphModel model)
        {
            while (type != null && model.VertexLabels.ContainsKey(type) || model.EdgeLabels.ContainsKey(type))
            {
                yield return type;
                type = type.GetTypeInfo().BaseType;
            }
        }

        private static IGremlinQuery<TSource> ConditionalAddStep<TSource>(this IGremlinQuery<TSource> query, bool condition, Func<IGremlinQuery<TSource>, IGremlinQuery<TSource>> addStepFunction)
        {
            return condition ? addStepFunction(query) : query;
        }

        private static Option<Expression> TryGetPartitionKeyExpression(this Type vertexType, IDseGraphModel model)
        {
            return vertexType
                .GetTypeHierarchy(model).SelectMany(type => model.PrimaryKeys
                    .TryGetValue(type)
                    .AsEnumerable())
                .FirstOrDefault();
        }
    }
}