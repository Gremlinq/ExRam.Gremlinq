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
    public static class DseGraphModelExtensions
    {
        private sealed class DseGraphModel : IDseGraphModel
        {
            public DseGraphModel(
                IImmutableDictionary<Type, string> vertexLabels, 
                IImmutableDictionary<Type, string> edgeTypes, 
                IImmutableList<(Type, Type, Type)> connections, 
                IImmutableDictionary<Type, Expression> primaryKeys,
                IImmutableDictionary<Type, IImmutableList<Expression>> materializedIndexes,
                IImmutableDictionary<Type, IImmutableList<Expression>> secondaryIndexes)
            {
                this.VertexLabels = vertexLabels;
                this.EdgeLabels = edgeTypes;
                this.Connections = connections;
                this.PrimaryKeys = primaryKeys;
                this.MaterializedIndexes = materializedIndexes;
                this.SecondaryIndexes = secondaryIndexes;
            }

            public IImmutableDictionary<Type, string> VertexLabels { get; }

            public IImmutableDictionary<Type, string> EdgeLabels { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }

            public IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

            public IImmutableDictionary<Type, IImmutableList<Expression>> MaterializedIndexes { get; }

            public IImmutableDictionary<Type, IImmutableList<Expression>> SecondaryIndexes { get; }
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

        public static IDseGraphModel ToDseGraphModel(this IGraphModel model)
        {
            return new DseGraphModel(model.VertexLabels, model.EdgeLabels, ImmutableList<(Type, Type, Type)>.Empty, ImmutableDictionary<Type, Expression>.Empty, ImmutableDictionary<Type, IImmutableList<Expression>>.Empty, ImmutableDictionary<Type, IImmutableList<Expression>>.Empty);
        }

        public static IDseGraphModel EdgeConnectionClosure(this IDseGraphModel model)
        {
            foreach (var connection in model.Connections)
            {
                foreach (var outVertexClosure in model.GetDerivedElementInfos(connection.Item1, true))
                {
                    foreach (var edgeClosure in model.GetDerivedElementInfos(connection.Item2, true))
                    {
                        foreach (var inVertexClosure in model.GetDerivedElementInfos(connection.Item3, true))
                        {
                            model = model.AddConnection(outVertexClosure, edgeClosure, inVertexClosure);
                        }
                    }
                }
            }

            return model;
        }

        public static IDseGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IDseGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        public static IDseGraphModel PrimaryKey<T>(this IDseGraphModel model, Expression<Func<T, object>> expression)
        {
            return new DseGraphModel(model.VertexLabels, model.EdgeLabels, model.Connections, model.PrimaryKeys.SetItem(typeof(T), expression), model.MaterializedIndexes, model.SecondaryIndexes);
        }

        public static IDseGraphModel MaterializedIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            return new DseGraphModel(
                model.VertexLabels,
                model.EdgeLabels,
                model.Connections,
                model.PrimaryKeys,
                model.MaterializedIndexes.Add(typeof(T), indexExpression),
                model.SecondaryIndexes);
        }

        public static IDseGraphModel SecondaryIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            return new DseGraphModel(
                model.VertexLabels,
                model.EdgeLabels,
                model.Connections,
                model.PrimaryKeys,
                model.MaterializedIndexes,
                model.SecondaryIndexes.Add(typeof(T), indexExpression));
        }

        public static IEnumerable<IGremlinQuery<string>> CreateSchemaQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            model = model
                .EdgeConnectionClosure();

            return model
                .CreatePropertyKeyQueries(queryProvider)
                .Concat(model.CreateVertexLabelQueries(queryProvider))
                .Concat(model.CreateVertexMaterializedIndexQueries(queryProvider))
                .Concat(model.CreateVertexSecondaryIndexQueries(queryProvider))
                .Concat(model.CreateEdgeLabelQueries(queryProvider));
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

        private static IEnumerable<IGremlinQuery<string>> CreateVertexSecondaryIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.CreateIndexQueries(model.SecondaryIndexes, "secondary", queryProvider);
        }

        private static IEnumerable<IGremlinQuery<string>> CreateVertexMaterializedIndexQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.CreateIndexQueries(model.MaterializedIndexes, "materialized", queryProvider);
        }

        private static IEnumerable<IGremlinQuery<string>> CreateIndexQueries(this IDseGraphModel model, IImmutableDictionary<Type, IImmutableList<Expression>> indexDictionary, string keyword, IGremlinQueryProvider queryProvider)
        {
            return model.VertexLabels
                .Select(vertexKvp => (
                    Label: vertexKvp.Value,
                    IndexProperties: vertexKvp.Key.GetTypeHierarchy(model)
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
                            .AddStep<string>("index", Guid.NewGuid().ToString("N"))
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
                    .AddStep<string>("create"));
        }

        private static IEnumerable<IGremlinQuery<string>> CreateEdgeLabelQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            return model.EdgeLabels
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
                            model.VertexLabels.TryGetValue(tuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ )),
                            model.VertexLabels.TryGetValue(tuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ ))))
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"));
        }

        private static IDseGraphModel AddConnection(this IDseGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
        {
            model.VertexLabels
                .TryGetValue(outVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {outVertexType}."));

            model.VertexLabels
                .TryGetValue(inVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {inVertexType}."));

            model.EdgeLabels
                .TryGetValue(edgeType)
                .IfNone(() => throw new ArgumentException($"Model does not contain edge type {edgeType}."));

            var tuple = (outVertexType, edgeType, inVertexType);

            return model.Connections.Contains(tuple)
                ? model
                : new DseGraphModel(model.VertexLabels, model.EdgeLabels, model.Connections.Add(tuple), model.PrimaryKeys, model.MaterializedIndexes, model.SecondaryIndexes);
        }

        private static IGremlinQuery<TSource> ConditionalAddStep<TSource>(this IGremlinQuery<TSource> query, bool condition, Func<IGremlinQuery<TSource>, IGremlinQuery<TSource>> addStepFunction)
        {
            return condition ? addStepFunction(query) : query;
        }

        private static Option<Expression> TryGetPartitionKeyExpression(this Type vertexType, IDseGraphModel model)
        {
            return vertexType
                .GetTypeHierarchy(model)
                .SelectMany(type => model.PrimaryKeys
                    .TryGetValue(type)
                    .AsEnumerable())
                .FirstOrDefault();
        }

        private static IEnumerable<Type> GetTypeHierarchy(this Type type, IGraphModel model)
        {
            while (type != null && model.VertexLabels.ContainsKey(type) || model.EdgeLabels.ContainsKey(type))
            {
                yield return type;
                type = type.GetTypeInfo().BaseType;
            }
        }
    }
}